
export type Record = {
    id: number;
    name: string;
    date: Date;
    data: TouchSequence[] | undefined;
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
