export interface schedData {
    _id: string;
    conf: string;
    conf_name?:string;
    create_time: string;
    day: string;
    day_of_week: string;
    end_date:string;
    hour:string;
    jitter:string;
    minute:string;
    month:string;
    sched_name:string;
    second:string;
    start_date:string;
    status:string;
    timezone:string;
    week?:string;
    year:string;
}
export interface confData {
    _id: string;
    conf_name:string;
    create_time: string;
    desc: string;
    day_of_week: string;
    items:Array<any>;
    usages:Array<string>;
}