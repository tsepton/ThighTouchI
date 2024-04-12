<script lang="ts">
	import { currentSequence$ } from '$lib/stores/RecordPlayer';
	import type { Record } from '$lib/types/Record';
	import type { Unsubscriber } from 'svelte/store';

	export let touchPoints: { fingerId: number; x: number; y: number }[] = [];

	let surface: HTMLElement;

	let subscriptions: Unsubscriber[] = [];

	export let record: Record;

	$: record,
		(() => {
			subscriptions.forEach((s) => s());
			subscriptions = [];
			touchPoints = [];
			handleRecordChange();
		})();

	// https://www.retinasocal.com/3d-images/burning-eyes-general.jpg
	// FIXME - refactor
	function handleRecordChange(): void {
		subscriptions.push(
			currentSequence$.subscribe((seq) => {
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
		const x = coordinatesX * cubeWidth;
		const y = coordinatesY * cubeHeight;

		const existingTouchPointIndex = touchPoints.findIndex((tp) => tp.fingerId === fingerId);
		if (!isBeingTouched) {
			if (existingTouchPointIndex !== -1) {
				touchPoints.splice(existingTouchPointIndex, 1);
			}
		} else {
			if (existingTouchPointIndex === -1) {
				touchPoints.push({ fingerId, x, y });
			} else {
				touchPoints[existingTouchPointIndex] = { fingerId, x, y };
			}
		}
	}
</script>

<div id="cube" class="card" bind:this={surface}>
	{#each touchPoints as touchPoint}
		<div
			class="touch-point-{touchPoint.fingerId}"
			style="left: {touchPoint.x}px; top: {touchPoint.y}px;"
		></div>
	{/each}
</div>

<style>
	#cube {
		position: relative;
		margin-top: 1rem;
		width: calc(70vh * 9 / 16); /* 16:9 aspect ratio */
		height: 70vh;
		background-color: rgba(255, 255, 255, 0.8);
		border-radius: 2px;
		transition: opacity 0.5s;
		position: relative;
		box-shadow: none;
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
