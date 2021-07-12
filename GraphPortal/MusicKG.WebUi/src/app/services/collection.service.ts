import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CollectionService {
  //  URL:string='http://10.94.104.79:8800'

  constructor(
    private http: HttpClient
  ) { }
// /confs  查询所有conf
  getCollectionConf(): Observable<any> {
    return this.http.get<any>(`/confs`);
  }
// /createconf  创建conf
  createConf(params:object): Observable<any> {
    const url = `/createconf`;
    return this.http.post<any>(url, params);
  }
  // /editconf?conf_id=   编辑某个conf
  editConf(suburl:string,params:object): Observable<any> {
    const url = `/editconf?conf_id=${suburl}`;
    return this.http.post<any>(url, params);
  }
  // /deleteconf?conf_id=  删除某个conf，当conf被sched使用时不能被删除
  deleteconf(suburl:string): Observable<any> {
    const url = `/deleteconf?conf_id=${suburl}`;
    return this.http.get<any>(url);
  }
  // /createsched  创建一个定时计划sched
  createsched(params:object): Observable<any> {
    const url = `/createsched`;
    return this.http.post<any>(url, params);
  }
  // /editsched?sched_id=
  editsched(suburl:string,params:object): Observable<any> {
    const url = `/editsched?sched_id=${suburl}`;
    return this.http.post<any>(url, params);
  }
  // scheds
  getScheds(): Observable<any> {
    const url = `/scheds`;
    return this.http.get<any>(url);
  }
  // /deletesched?sched_id=
  deletesched(suburl:string): Observable<any> {
    const url = `/deletesched?sched_id=${suburl}`;
    return this.http.get<any>(url);
  }

  // /conf?conf_id=   查询某个conf
  getConfByid(suburl:string): Observable<any> {
    const url = `/conf?conf_id=${suburl}`;
    return this.http.get<any>(url);
  }

}
