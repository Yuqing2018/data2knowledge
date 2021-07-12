import { UserInfo } from './user.interface';
import { Workspace } from './workspace.interface';

export interface Task {
    name: string;
    overlap: number;
    annotatorIds: string[];
    expectedDueAt: Date;
    documentIds: string[];
    taskType?: string;
}

export interface TaskInfo {
    id: string;
    name: string;
    overlap: number;
    annotators: UserInfo[];
    documents: any;
    actualDueAt: Date;
    expectedDueAt: Date;
    workspace?: Workspace;
    status?: string;
    taskType?: string;
    dictionaryIds?: string[];
}

export interface TaskDocument {
    id: string;
    name: string;
    status: string;
    uploadedAt: Date;
    latestResultSavedAt: Date;
    annotators: UserInfo[];
    checked?: boolean
}

export interface DocumentResult {
    documentId: string;
    taskDocumentStatus: string;
    resultDocumentId: string;
    resultType: string;
    annotatedAt: Date;
    annotatedBy: UserInfo[];
}

export interface TaskUpdateModel {
    name?: string;
    expectedDueAt?: Date;
}

export interface TestSimilarityDocContent {
    Guideline: string;
    Items: TestSimilarityItem[];
}

export interface TestSimilarityItem {
    Id: string;
    TextSource: string;
    TextTargets: string[];
    Labels: Label[];
}

export interface Label {
    Text: string;
    Result: number;
    IsAdded?: boolean;
}
