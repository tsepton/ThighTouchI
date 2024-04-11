import { browser } from '$app/environment';
import type { Touch, TouchSequence } from '$lib/types/Record';
import type { Readable } from 'svelte/motion';
import { derived, writable, type Writable } from 'svelte/store';
import { currentRecord$ } from './CurrentRecord';

export const currentSequence$: Readable<PlayableSequence> = derived(
	currentRecord$,
	($record, set) => {
		// Transformation logic here
		let seq: PlayableSequence;
		if ($record.id === -1) seq = new CurrentSequence();
		else seq = new PlaybackSequence($record.data!);
		set(seq);
	}
);

export interface PlayableSequence {
	status$: Writable<string>;
	currentTouches$: Writable<Touch[]>;
	_history: TouchSequence[];
}

export class CurrentSequence implements PlayableSequence {
	status$: Writable<string> = writable('Connecting...');
	currentTouches$: Writable<Touch[]> = writable([]);

	_history: TouchSequence[] = [];
	_recordTimestamp: number | undefined = 0;
	_socket: WebSocket;

	constructor() {
		if (!browser) throw new Error('This class can only be used in the browser.');

		this._socket = new WebSocket('ws://192.168.0.148:8001/ws');

		this._socket.addEventListener('message', (event) => {
			const data = JSON.parse(event.data);

			data.shift(); // Remove the first element, which indicates that it is being touched.

			const touches: Touch[] = [];
			for (let index = 0; index < event.data.length; index += 6) {
				const touchPointData = data.slice(index, index + 6);
				if (touchPointData.length < 6) break;
				if (
					touchPointData.reduce(
						(accumulator: any, currentValue: any) => accumulator + currentValue
					) === 0
				)
					break;

				const isBeingTouched = touchPointData[0] === 7;
				const fingerId = touchPointData[1];
				const coordinatesX = touchPointData[3];
				const offsetX = touchPointData[2];
				const coordinatesY = touchPointData[5];
				const offsetY = touchPointData[4];

				touches.push({
					// TODO
					fingerId,
					x: coordinatesX + offsetX / 256.0,
					y: coordinatesY + offsetY / 256.0,
					isBeingTouched
				});
			}
			this.currentTouches$.set(touches);
			this._history.push({ touches, timestamp: Date.now() });
		});

		this._socket.addEventListener('open', () => {
			console.log('WebSocket connection established.');
			this.status$.set('Connected');
		});

		this._socket.addEventListener('close', () => {
			console.log('WebSocket connection closed.');
			this.status$.set('Connection closed.');
		});

		this._socket.addEventListener('error', (error) => {
			console.error('WebSocket error:', error);
			this.status$.set('Connection error.');
		});
	}

	startRecording(): void {
		this._recordTimestamp = Date.now();
	}

	stopRecording(): TouchSequence[] {
		if (!this._recordTimestamp) throw new Error('Recording has not started.');

		const historyToSave = this._history.filter(
			(touchSequence) => touchSequence.timestamp > this._recordTimestamp!
		);
		this._recordTimestamp = undefined;
		return historyToSave;
	}
}

export class PlaybackSequence implements PlayableSequence {
	status$: Writable<string> = writable('Playback.');
	currentTouches$: Writable<Touch[]> = writable([]);
	readingProgression$: Writable<number> = writable(0);

	_history: TouchSequence[] = [];

	constructor(history: TouchSequence[]) {
		this._history = history;
		if (this._history.length === 0) this.status$.set('Empty record');
		else setTimeout(() => this.play(), 100);
	}

	play(index: number = 0): void {
		this.status$.set('Playing.');
		const toPlay = this._history[index];
		this.currentTouches$.set(toPlay.touches); // TODO  +1 on each call + delay call
		// TODO timeout should be based upon the diff between timestamps
		if (index == this._history.length - 1) setTimeout(() => this.play(0), 2000);
		else
			setTimeout(
				() => this.play(index + 1),
				(toPlay.timestamp - this._history[index + 1].timestamp) * 1000
			);
		this.readingProgression$.set((index / this._history.length) * 100);
	}
}
