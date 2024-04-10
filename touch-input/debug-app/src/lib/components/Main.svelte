<script lang="ts">
	import Listing from './Listing.svelte';
	import { currentRecord$ } from '$lib/stores/CurrentRecord';
	import type { Record } from '$lib/types/Record';
	import { Sequence } from '$lib/stores/RecordPlayer';
	import SequencePlayer from './SequencePlayer.svelte';

	let record: Record | undefined;

	currentRecord$.subscribe((r) => {
		record = r;
	});
</script>

<div class="container">
	<Listing className="card"></Listing>
	{#if record}
		<SequencePlayer className="card" {record}></SequencePlayer>
	{/if}
</div>

<style>
	.container {
		display: flex;
		flex-direction: row;
		justify-content: space-between;
		width: 100vw;
		height: 100vh;
		gap: 2rem;
		padding: 2rem;
	}
</style>
