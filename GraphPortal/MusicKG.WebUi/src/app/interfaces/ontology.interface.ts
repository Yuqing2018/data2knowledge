export interface OntologyEntityInfo {
    id: string;
    name: string;
    properties: any;
    description: string;
    color: string;
}
export interface RelationData {
    id: string;
    name: string;
    firsEntityId: string;
    firstEntityName:string;
    secondEntityId: string;
    secondEntityName:string;
    describe: string;
}