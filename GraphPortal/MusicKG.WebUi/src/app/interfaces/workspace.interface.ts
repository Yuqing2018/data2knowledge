import { UserInfo } from '../interfaces/user.interface';

export interface Workspace {
    id?: string;
    name: string;
    type: WorkspaceType | any; 
    language: any;
    description?: string;
    createBy?: UserInfo;
}

export interface WorkspaceType {
    id: string;
    name: string;
    workflowName?: string;
}