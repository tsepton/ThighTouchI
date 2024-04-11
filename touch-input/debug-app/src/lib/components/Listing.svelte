<script lang="ts">
	import * as Table from '$lib/components/ui/table/index.js';
	import { currentRecord$ } from '$lib/stores/CurrentRecord';
	import type { Record } from '$lib/types/Record';
	import { onMount } from 'svelte';

	export let className: string = '';

    const recordings: Record[] = [
		{
			id: -1,
			name: 'Current interaction',
			date: new Date()
		},
		// TODO - these should be fetched
		{
			id: 1,
			name: 'Mathieu',
			date: new Date('2024-10-01')
		},
		{
			id: 2,
			name: 'Jean',
			date: new Date('2024-10-02')
		}
	];

	let current: Record = recordings[0];
	currentRecord$.subscribe((record) => {
		current = record;
	});

	
	onMount(() => {
		currentRecord$.set(recordings[0]);
	});

	function handleSelection(selection: Record) {
		currentRecord$.set(selection);
	}
</script>

<div class="container {className}">
	<h4 class="text-lg font-medium leading-none">Recordings</h4>
	<Table.Root>
		<Table.Header>
			<Table.Row>
				<Table.Head>Name</Table.Head>
				<Table.Head>Date</Table.Head>
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
				</Table.Row>
			{/each}
		</Table.Body>
	</Table.Root>
</div>

<style>
	.container {
		display: flex;
		flex-direction: column;
		gap: 3rem;
		height: 100%;
	}
</style>
