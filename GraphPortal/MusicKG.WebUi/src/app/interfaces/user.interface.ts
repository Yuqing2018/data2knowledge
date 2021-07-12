export interface UserLoginInfo {
    token: string;
    tokenExpiredAt: Date;
    id: string;
    name: string;
    roles: string[];
    status: string;
    createBy: string;
    createAt: Date;
}

export interface UserInfo {
    createdAt?: string;
    createdBy?: any;
    id: string;
    name: string;
    roles: string[];
    status: string;
}

export interface UserList {
    totalCount: number;
    from: number;
    count: number;
    items: UserInfo[]

}