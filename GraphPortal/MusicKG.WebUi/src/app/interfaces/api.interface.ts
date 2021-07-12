export interface ResponseError {
    requestId: string;
    message: string;
}

export interface Page<T> {
    totalCount: number;
    from: number;
    count: number;
    items: Array<T>;
}   

export interface Option {
    type: string;
    value: string;
    displayName: string;
}