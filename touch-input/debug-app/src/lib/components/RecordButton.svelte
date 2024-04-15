<script lang="ts">
	import * as Popover from '$lib/components/ui/popover';
	import { CurrentSequence, currentSequence$ } from '$lib/stores/RecordPlayer';
	import type { Record } from '$lib/types/Record';
	import { get } from 'svelte/store';
	import Button from './ui/button/button.svelte';
	import { Input } from './ui/input';
	import { Label } from './ui/label';

	export let record: Record;

	export let status: string;

	let name: string = '';

	let isRecording: boolean = false;

	function startRecord() {
		if (isRecording) throw new Error('Already recording');
		isRecording = true;
		(get(currentSequence$) as CurrentSequence).startRecording();
	}

	function stopRecord() {
		if (!isRecording) throw new Error('Nothing to stop recording');
		isRecording = false;
		const touchSequence = (get(currentSequence$) as CurrentSequence).stopRecording();
		download(`${name}.json`, JSON.stringify(touchSequence));
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

{#if isRecording}
	<Button on:click={stopRecord}>Stop recording {name}</Button>
{:else}
	<Popover.Root>
		<Popover.Trigger asChild let:builder>
			<Button builders={[builder]} disabled={status !== 'Connected.'}>Record interaction</Button>
		</Popover.Trigger>
		<Popover.Content>
			<div class="grid grid-cols-3 items-center">
				<Label for="record-name">Name</Label>
				<Input id="record-name" bind:value={name} class="col-span-2 h-8" />
			</div>
			<div class="mt-5 flex justify-end">
				<Button
					on:click={startRecord}
					size="sm"
					variant="outline"
					disabled={name.length === 0}>Record current</Button
				>
			</div>
		</Popover.Content>
	</Popover.Root>
{/if}
