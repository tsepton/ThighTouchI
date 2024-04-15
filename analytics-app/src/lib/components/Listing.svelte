<script lang="ts">
	import * as Table from '$lib/components/ui/table/index.js';
	import { currentRecord$ } from '$lib/stores/CurrentRecord';
	import type { Record } from '$lib/types/Record';
	import { onMount } from 'svelte';
	import { Button } from './ui/button';
	import CirclePlus from 'lucide-svelte/icons/circle-plus';

	export let className: string = '';

	let recordings: Record[] = [
		{
			id: -1,
			name: 'Current interaction',
			date: new Date(),
			data: undefined
		}
	];

	let current: Record = recordings[0];
	currentRecord$.subscribe((record) => {
		current = record;
	});

	let hiddenInput: HTMLInputElement;

	onMount(() => {
		currentRecord$.set(recordings[0]);
	});

	function handleSelection(selection: Record) {
		currentRecord$.set(selection);
	}

	function handleFileChange(event: any) {
		const file = event.target.files[0];
		const reader = new FileReader();
		reader.onload = () => {
			recordings = [
				...recordings,
				{
					id: recordings.length,
					name: file.name,
					date: new Date(file.lastModified),
					data: JSON.parse(reader.result as string)
				}
			];
		};
		reader.readAsText(file);
	}
</script>

<div class="container {className}">
	<h4 class="text-lg font-medium leading-none">Recordings</h4>
	<Table.Root>
		<Table.Header>
			<Table.Row>
				<Table.Head>Name</Table.Head>
				<Table.Head>Date</Table.Head>
				<Table.Head>
					<Button
					variant="ghost"
						size="icon"
						on:click={() => {
							hiddenInput.click();
						}}
					>
						<CirclePlus class="h-4 w-4" />
					</Button>
				</Table.Head>
			</Table.Row>
		</Table.Header>
		<Table.Body>
			{#each recordings as record, i (i)}
				<Table.Row
					data-state={current?.id === record.id ? 'selected' : ''}
					on:click={() => handleSelection(record)}
				>
					<Table.Cell class="font-medium">{record.name}</Table.Cell>
					<Table.Cell>{record.date}</Table.Cell>
					<Table.Cell></Table.Cell>
				</Table.Row>
			{/each}
		</Table.Body>
	</Table.Root>
	<input id="hidden-file-input" type="file" bind:this={hiddenInput} on:change={handleFileChange} />
</div>

<style>
	.container {
		display: flex;
		flex-direction: column;
		gap: 3rem;
		height: 100%;
	}

	#hidden-file-input {
		display: none;
	}
</style>
