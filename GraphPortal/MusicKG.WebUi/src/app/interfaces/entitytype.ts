export interface EntitytypeData {
    Id: string,
    Source: string,
    EntityType: string,
    Entries: Entrie[],
    SchemaType: string,
    AgentId: string,
    parentId: string
}

export interface Entrie {
    Value: string[]
    Synonyms: string[][]
}

export interface Menus {
    AgentId: string,
    EntityType: string,
    Entries: Entrie[],
    Id: string,
    SchemaType: string,
    Source: string,
    children: Menus[]
    disabled: boolean
    level: number
    open: boolean
    parentId: string
    selected: boolean
}
