import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class StatisticService {

  constructor(
    private http: HttpClient
  ) { }

  getList(fromDate: string, toDate: string, workspaceTypeId?: string): Observable<any> {
    if (workspaceTypeId === 'total') {
      return this.http.get<any>(`/Statistic/Overview?fromDate=${fromDate}&toDate=${toDate}`)
    } else {
      return this.http.get<any>(`/Statistic/Overview?fromDate=${fromDate}&toDate=${toDate}&workspaceTypeId=${workspaceTypeId}`)
    }
  }

  getDetail(userId: string, fromDate: string, toDate: string, workspaceTypeId?: string): Observable<any> {
    if (workspaceTypeId === 'total') {
      return this.http.get<any>(`/Statistic/${userId}?fromDate=${fromDate}&toDate=${toDate}`)
    } else {
      return this.http.get<any>(`/Statistic/${userId}?fromDate=${fromDate}&toDate=${toDate}&workspaceTypeId=${workspaceTypeId}`)
    }
  }
}
