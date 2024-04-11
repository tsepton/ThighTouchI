<script lang="ts">
	import { currentSequence$, type PlayableSequence } from '$lib/stores/RecordPlayer';
	import type { Record, Touch } from '$lib/types/Record';
	import Button from './ui/button/button.svelte';
	import Progress from './ui/progress/progress.svelte';
	export let className: string = '';

	export let record: Record;

	let surface: HTMLElement;

	let status: string = '';

	let touchPoints: { fingerId: number; touchX: number; touchY: number }[] = [];

	let isRecording: boolean = false;

	let isPlayback: boolean = false;

	$: record, isPlayback = record.id !== -1;

	currentSequence$.subscribe((seq) => {
		seq.status$.subscribe((s) => status = s);
		seq.currentTouches$.subscribe(touches => touches.forEach((touch) => {
			drawFinger(touch.fingerId, touch.x, touch.y, touch.isBeingTouched);
		}));
	});

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
</script>

<div class="container {className}">
	<h4 class="mb-2 text-lg font-medium leading-none">Selection</h4>
	{#if !isPlayback}
		<div>
			<small>{record.name}</small>
		</div>
		<div>
			<small>{status}</small>
		</div>
	{:else}
		<small>{record.name}</small>
	{/if}
	<div id="cube" class="card" bind:this={surface}>
		{#each touchPoints as touchPoint}
			<div
				class="touch-point-{touchPoint.fingerId}"
				style="left: {touchPoint.touchX}px; top: {touchPoint.touchY}px;"
			></div>
		{/each}
	</div>
	{#if !isPlayback && !isRecording}
		<Button>Record current</Button>
	{:else if !isPlayback && isRecording}
		<Button>Stop recording</Button>
	{:else if record.id !== -1}
		<Progress value={33} />
	{/if}
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
