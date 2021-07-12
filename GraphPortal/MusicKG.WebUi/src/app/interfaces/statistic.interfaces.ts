import { UserInfo } from './user.interface';

export interface StatisticOverview {
    annotatedDocumentCount: number;
    annotatedItemCount: number;
    annotatedTaskCount: number;
    annotators: Statistic[];
    managers: Statistic[];
}

export interface Statistic {
    statisitics: Statisitics;
    user: UserInfo;
}

export interface Statisitics {
    createdTaskCount: number;
    finishedDocumentCount: number;
    finishedItemCount: number;
    finishedTaskCount: number;
    notFinishedDocumentCount: number;
    notFinishedItemCount: number;
    notFinishedTaskCount: number;
}
