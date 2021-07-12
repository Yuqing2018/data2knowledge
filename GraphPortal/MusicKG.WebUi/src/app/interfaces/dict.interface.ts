export interface Dict {
    name: string,
    className?: string,
    entutyName?: string
}

export interface DictItem {
    entityId: string;
    entityName: string;
    entriesCount: number;
    id: string;
    name: string;
    workspaceId: string
}

export interface DictList {
    count: number;
    from: number;
    items: DictItem[];
    totalCount: number;
}

export interface DictDetail {
    createdAt: string;
    createdBy: string;
    entityId: string;
    entityName: string;
    entries: any;
    id: string;
    name: string;
    workspaceId: string
}