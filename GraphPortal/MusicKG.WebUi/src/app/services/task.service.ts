import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Task, TaskInfo, TaskUpdateModel, DocumentResult, TaskDocument } from '../interfaces/task.interface';
import { HttpClient } from '@angular/common/http';
import { Page } from '../interfaces/api.interface';
import { ViewContainerData } from '@angular/core/src/view';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  constructor(
    private http: HttpClient
  ) { }

  /**
   * Get page task list by workspace
   *
   * @param {string} workspaceId
   * @param {number} [from=0]
   * @param {number} [size=10]
   * @returns {Observable<Page<Task>>}
   * @memberof TaskService
   */
  getTaskListByWorkspace(workspaceId: string, from: number = 0, size: number = 10, status: string): Observable<Page<TaskInfo>> {
    return this.http.get<Page<TaskInfo>>(`/Workspace/${workspaceId}/Task?from=${from}&size=${size}&${status}`);
  }

  /**
   * Get user task
   *
   * @param {number} [from=0]
   * @param {number} [size=10]
   * @returns {Observable<Page<TaskInfo>>}
   * @memberof TaskService
   */
  getTaskList(from: number = 0, size: number = 10, status: string, keyword: string = ''): Observable<Page<TaskInfo>> {
    return this.http.get<Page<TaskInfo>>(`/Task?from=${from}&size=${size}&${status}&keyword=${keyword}`);
  }

  /**
   * Add task
   *
   * @param {string} workspaceId
   * @param {Task} task
   * @returns {Observable<void>}
   * @memberof TaskService
   */
  add(workspaceId: string, task: Task): Observable<void> {
    return this.http.post<void>(`/Workspace/${workspaceId}/Task`, task);
  }

  /**
   * Update task
   *
   * @param {string} workspaceId
   * @param {string} taskId
   * @param {TaskUpdateModel} taskUpdate
   * @returns {Observable<void>}
   * @memberof TaskService
   */
  update(workspaceId: string, taskId: string, taskUpdate: TaskUpdateModel): Observable<void> {
    return this.http.put<void>(`/Workspace/${workspaceId}/Task/${taskId}`, taskUpdate);
  }

  /**
   * Delete task
   *
   * @param {string} workspaceId
   * @param {string} taskId
   * @returns {Observable<void>}
   * @memberof TaskService
   */
  delete(workspaceId: string, taskId: string): Observable<void> {
    return this.http.delete<void>(`/Workspace/${workspaceId}/Task/${taskId}`);
  }

  /**
   * Get task info
   *
   * @param {string} workspaceId
   * @param {string} taskId
   * @returns {Observable<TaskInfo>}
   * @memberof TaskService
   */
  get(workspaceId: string, taskId: string): Observable<TaskInfo> {
    return this.http.get<TaskInfo>(`/Workspace/${workspaceId}/Task/${taskId}`);
  }

  /**
   * Get doc annotation result
   *
   * @param {string} workspaceId
   * @param {string} taskId
   * @param {string} documentId
   * @returns {Observable<DocumentResult>}
   * @memberof TaskService
   */
  getAnnotationResult(workspaceId: string, taskId: string, documentId: string): Observable<DocumentResult> {
    return this.http.get<DocumentResult>(`/Workspace/${workspaceId}/Task/${taskId}/Document/${documentId}/Result`);
  }

  /**
   * Get task document
   *
   * @param {string} workspaceId
   * @param {string} taskId
   * @returns {Observable<Page<DocumentResult>>}
   * @memberof TaskService
   */
  getTaskDoc(workspaceId: string, taskId: string): Observable<Page<TaskDocument>> {
    return this.http.get<Page<TaskDocument>>(`/Workspace/${workspaceId}/Task/${taskId}/Document`);
  }

  /**
   * Save annotator result
   *
   * @param {string} workspaceId
   * @param {string} taskId
   * @param {string} documentId
   * @param {*} result
   * @returns {Observable<string>}
   * @memberof TaskService
   */
  saveAnnotationResult(workspaceId: string, taskId: string, documentId: string, result: any): Observable<string> {
    return this.http.post<string>(`/Workspace/${workspaceId}/Task/${taskId}/Document/${documentId}/Result`, result);
  }

  /**
   * Commit task
   *
   * @param {string} workspaceId
   * @param {string} taskId
   * @returns {Observable<string>}
   * @memberof TaskService
   */
  commit(workspaceId: string, taskId: string): Observable<string> {
    return this.http.post<string>(`/Workspace/${workspaceId}/Task/${taskId}/Commit`, null);
  }

  getTaskCreateRule() {
    return this.http.get<any>(`/Workspace/${environment.workspaceId}/TaskCreationRule`);
  }

  putTaskCreateRule(data: any) {
    return this.http.put<any>(`/Workspace/${environment.workspaceId}/TaskCreationRule`, data);
  }

  submitTask(taskList: any) {
    return this.http.post<any>(`/Workspace/${environment.workspaceId}/Task/Results`, taskList);
  }

  upload(type: string, file: any) {
    return this.http.post<any>(`/honda/v1/vehiclefault/rawdata?dataSource=${type}`, file);
  }

}
