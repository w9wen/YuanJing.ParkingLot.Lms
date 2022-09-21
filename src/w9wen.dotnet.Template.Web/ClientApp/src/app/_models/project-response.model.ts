export interface ProjectResponse {
    id: number;
    name: string;
    items: Item[];
}

export interface Item {
    id: number;
    title: string;
    description: string;
    isDone: boolean;
}