import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Task } from '../interfaces/task.interface';
import { Dict } from '../interfaces/dict.interface';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RuleService {

  constructor(
    private http: HttpClient
  ) { }

  getList(workspaceId: string) {
    return this.http.get(`/Workspace/${workspaceId}/Dictionary`);
  }

  getContent(wId: string, dId: string) {
    return this.http.get(`/Workspace/${wId}/Dictionary/Entries/${dId}`);
  }

  getData(): Observable<Dict[]> {
    return of([
      { name: 'Dict1', className: '', entityName: ''},
      { name: 'Dict2', className: '', entityName: ''},
      { name: 'Dict3', className: '', entityName: ''},
      { name: 'Dict4', className: '', entityName: ''},
      { name: 'Dict5', className: '', entityName: ''}
    ])
  }
}
