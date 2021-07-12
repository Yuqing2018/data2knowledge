import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DictList, DictDetail } from 'src/app/interfaces/dict.interface';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DictionaryService {

  constructor(
    private http: HttpClient
  ) { }

  uploadDict(workspaceId: string, file: any): Observable<any> {
    const url = `/Workspace/${workspaceId}/Dictionary/Content`
    return this.http.post<any>(url, file)
  }

  getList(workspaceId: string, from: number = 0, size: number = 10): Observable<DictList> {
    const url = `/Workspace/${workspaceId}/Dictionary?from=${from}&size=${size}`
    return this.http.get<DictList>(url)
  }

  deleteDict(workspaceId: string, dictionaryId: string): Observable<any> {
    const url = `/Workspace/${workspaceId}/Dictionary/${dictionaryId}`
    return this.http.delete<any>(url)
  }

  updateDict(workspaceId: string, dictionaryId: string, name: string): Observable<any> {
    const url = `/Workspace/${workspaceId}/Dictionary/${dictionaryId}`
    return this.http.put<any>(url, { name })
  }

  get(workspaceId: string, dictionaryId: string, from: number = 0, size: number = 10): Observable<DictDetail> {
    const url = `/Workspace/${workspaceId}/Dictionary/${dictionaryId}?from=${from}&size=${size}`
    return this.http.get<DictDetail>(url)
  }

  getEntries(workspaceId: string, dictionaryId: string, filterString: string, from: number = 0, size: number = 10): Observable<any> {
    const url = `/Workspace/${workspaceId}/Dictionary/Entries/${dictionaryId}?filterString=${filterString}&from=${from}&size=${size}`
    return this.http.get<any>(url)
  }

  getPartName(workspaceId: string) {
    const url = `/Workspace/${workspaceId}/Dictionary/${environment.partId}`
    return this.http.get<DictDetail>(url)
  }

  getSearchPartName(filterString: string) {
    return this.getEntries(environment.workspaceId, environment.partId, filterString);
  }

  getReason(workspaceId: string) {
    const url = `/Workspace/${workspaceId}/Category`
    return this.http.get<DictDetail>(url)
  }

  getReasonAll(workspaceId: string) {
    const url = `/Workspace/${workspaceId}/Category/All`
    return this.http.get<any>(url)
  }

  getCategory(workspaceId: string, keyword: string, from: number = 0, size: number = 10): Observable<any> {
    const url = `/Workspace/${workspaceId}/Category?from=${from}&size=${size}&keyword=${keyword}`
    return this.http.get<any>(url)
  }

  getCategorySearch(keyword: string) {
    return this.getCategory(environment.workspaceId, keyword);
  }

  getCategoryInfo(id: string) {
    const url = `/Workspace/${environment.workspaceId}/Category/${id}`
    return this.http.get<any>(url)
  }

  addReason(workspaceId: string, newValue: string[]) {
    const url = `/Workspace/${workspaceId}/Category`
    return this.http.post<DictDetail>(url, newValue)
  }

  updateReason(workspaceId: string, categoryId: string, name: string) {
    const url = `/Workspace/${workspaceId}/Category/${categoryId}`
    const formData = new FormData();
    formData.append('name', name);
    return this.http.put<DictDetail>(url, formData)
  }

}
