<script lang="ts">
	import {
		currentSequence$,
		CurrentSequence,
		type PlayableSequence,
		PlaybackSequence
	} from '$lib/stores/RecordPlayer';
	import type { Record, Touch } from '$lib/types/Record';
	import { get, type Unsubscriber } from 'svelte/store';
	import Button from './ui/button/button.svelte';
	import Progress from './ui/progress/progress.svelte';
	import * as Popover from '$lib/components/ui/popover';
	import { Input } from './ui/input';
	import { Label } from './ui/label';

	export let className: string = '';

	export let record: Record;

	let surface: HTMLElement;

	let status: string = '';

	let readingProgression: number = 0;

	let touchPoints: { fingerId: number; touchX: number; touchY: number }[] = [];

	let isRecording: boolean = false;

	let isPlayback: boolean = false;

	let recordName = '';

	let subscriptions: Unsubscriber[] = [];

	$: record,
		(isPlayback = record.id !== -1),
		(() => {
			subscriptions.forEach((s) => s());
			subscriptions = [];
			handleRecordChange();
		})();

	// https://www.retinasocal.com/3d-images/burning-eyes-general.jpg
	// FIXME - refactor
	function handleRecordChange(): void {
		subscriptions.push(
			currentSequence$.subscribe((seq) => {
				subscriptions.push(seq.status$.subscribe((s) => (status = s)));
				const temp = (seq as PlaybackSequence).readingProgression$?.subscribe(
					(p) => (readingProgression = p)
				);
				if (temp) subscriptions.push(temp);
				subscriptions.push(
					seq.currentTouches$.subscribe((touches) =>
						touches.forEach((touch) => {
							drawFinger(touch.fingerId, touch.x, touch.y, touch.isBeingTouched);
						})
					)
				);
			})
		);
	}

	function drawFinger(
		fingerId: number,
		coordinatesX: number,
		coordinatesY: number,
		isBeingTouched: boolean
	) {
		const cubeWidth = surface.offsetWidth;
		const cubeHeight = surface.offsetHeight;
		const touchX = (coordinatesX / 15.0) * cubeWidth;
		const touchY = (coordinatesY / 15.0) * cubeHeight;

		const existingTouchPointIndex = touchPoints.findIndex((tp) => tp.fingerId === fingerId);
		if (!isBeingTouched) {
			if (existingTouchPointIndex !== -1) {
				touchPoints.splice(existingTouchPointIndex, 1);
			}
		} else {
			if (existingTouchPointIndex === -1) {
				touchPoints.push({ fingerId, touchX, touchY });
			} else {
				touchPoints[existingTouchPointIndex] = { fingerId, touchX, touchY };
			}
		}
	}

	function startRecord() {
		if (isRecording) throw new Error('Already recording');
		if (isPlayback) throw new Error('Cannot record while playing back a sequence');
		isRecording = true;
		(get(currentSequence$) as CurrentSequence).startRecording();
	}

	function stopRecord() {
		if (!isRecording) throw new Error('Nothing to stop recording');
		if (isPlayback) throw new Error('Cannot record while playing back a sequence');
		isRecording = false;
		const touchSequence = (get(currentSequence$) as CurrentSequence).stopRecording();
		download(`${recordName}.json`, JSON.stringify(touchSequence));
	}

	function download(filename: string, data: any) {
		const element = document.createElement('a');
		element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(data));
		element.setAttribute('download', filename);

		element.style.display = 'none';
		document.body.appendChild(element);

		element.click();

		document.body.removeChild(element);
	}
</script>

<div class="container {className}">
	<h4 class="mb-2 text-lg font-medium leading-none">Selection</h4>
	<div>
		<small>{record.name}</small>
	</div>
	<div>
		<small>{status}</small>
	</div>

	<div id="cube" class="card" bind:this={surface}>
		{#each touchPoints as touchPoint}
			<div
				class="touch-point-{touchPoint.fingerId}"
				style="left: {touchPoint.touchX}px; top: {touchPoint.touchY}px;"
			></div>
		{/each}
	</div>
	<div class="mt-8 flex justify-center">
		{#if !isPlayback && !isRecording}
			<Popover.Root>
				<Popover.Trigger asChild let:builder>
					<Button builders={[builder]}>Record</Button>
				</Popover.Trigger>
				<Popover.Content>
					<div class="grid grid-cols-3 items-center">
						<Label for="record-name">Name</Label>
						<Input id="record-name" bind:value={recordName} class="col-span-2 h-8" />
					</div>
					<div class="mt-5 flex justify-end">
						<Button
							on:click={startRecord}
							size="sm"
							variant="outline"
							disabled={recordName.length === 0}>Record current</Button
						>
					</div>
				</Popover.Content>
			</Popover.Root>
		{:else if !isPlayback && isRecording}
			<Button on:click={stopRecord}>Stop recording {recordName}</Button>
		{:else if record.id !== -1}
			<Progress value={readingProgression} />
		{/if}
	</div>
</div>

<style>
	.container {
		max-width: max-content;
		min-width: 35vh;
	}

	#cube {
		margin-top: 1rem;
		height: calc(35vh * 16 / 9);
		width: 35vh;
		background-color: rgba(255, 255, 255, 0.8);
		border-radius: 2px;
		transition: opacity 0.5s;
		position: relative;
	}

	.hidden {
		visibility: hidden;
		opacity: 0;
	}

	[class^='touch-point-'] {
		position: absolute;
		width: 10px;
		height: 10px;
		background-color: #312f2f;
		border-radius: 50%;
	}
</style>
