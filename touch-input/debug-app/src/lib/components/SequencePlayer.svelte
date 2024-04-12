<script lang="ts">
	import { PlaybackSequence, currentSequence$ } from '$lib/stores/RecordPlayer';
	import type { Record } from '$lib/types/Record';
	import { type Unsubscriber } from 'svelte/store';
	import RecordButton from './RecordButton.svelte';
	import Surface from './Surface.svelte';
	import Progress from './ui/progress/progress.svelte';

	export let className: string = '';

	export let record: Record;

	let status: string = '';

	let readingProgression: number = 0;

	let isPlayback: boolean = false;

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
			})
		);
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

	<Surface {record}></Surface>
	<div class="mt-8 flex justify-center">
		{#if !isPlayback}
			<RecordButton {record}></RecordButton>
		{:else}
			<Progress value={readingProgression} />
		{/if}
	</div>
</div>

<style>
	.container {
		position: relative;
		max-width: max-content;
		min-width: fit-content;
		max-height: 100%;
	}
</style>
