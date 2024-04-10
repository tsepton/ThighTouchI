import type { Record } from "$lib/types/Record";
import type { Writable } from "svelte/store";
import { writable } from "svelte/store";


export const currentRecord$: Writable<Record> = writable();