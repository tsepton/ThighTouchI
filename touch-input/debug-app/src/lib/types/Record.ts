
export type Record = {
    id: number;
    name: string;
    date: Date;
};

export type Touch = {
    fingerId: number;
    x: number;
    y: number;
    isBeingTouched: boolean;
};

export type TouchSequence = {
	timestamp: number;
	touches: Touch[];
};
