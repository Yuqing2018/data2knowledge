import { Injectable } from '@angular/core';
import { Observable, forkJoin } from 'rxjs';
import { Page } from '../interfaces/api.interface';
import { Workspace } from '../interfaces/workspace.interface';
import { HttpClient } from '@angular/common/http';
import { switchMap, map } from 'rxjs/operators';
import { OptionService } from './option.service';

@Injectable({
  providedIn: 'root'
})
export class WorkspaceService {

  constructor(
    private http: HttpClient,
    private optionService: OptionService
  ) { }

  /**
   * Get Workspace List
   *
   * @returns {Observable<Page<Workspace>>}
   * @memberof WorkspaceService
   */
  getList(): Observable<Page<Workspace>> {
    return this.http.get<Page<Workspace>>(`/Workspace`);
  }

  /**
   * Get Workspace info
   *
   * @param {string} workspaceId
   * @returns {Observable<Page<Workspace>>}
   * @memberof WorkspaceService
   */
  info(workspaceId: string): Observable<Workspace> {
    return this.http.get<Workspace>(`/Workspace/${workspaceId}`);
  }

  /**
   * Add Workspace
   *
   * @param {Workspace} workspace
   * @returns {Observable<void>}
   * @memberof WorkspaceService
   */
  add(workspace: Workspace): Observable<void> {
    return this.http.post<void>(`/Workspace`, workspace);
  }

  /**
   * Delete Workspace
   *
   * @param {string} workspaceId
   * @returns {Observable<void>}
   * @memberof WorkspaceService
   */
  delete(workspaceId: string): Observable<void> {
    return this.http.delete<void>(`/Workspace/${workspaceId}`);
  }

  /**
   * Update Workspace
   *
   * @param {Workspace} workspace
   * @returns {Observable<void>}
   * @memberof WorkspaceService
   */
  update(workspace: Workspace): Observable<void> {
    return this.http.put<void>(`/Workspace/${workspace.id}`, workspace);
  }

  /**
   * Get Workspace type display name 
   *
   * @param {string} workspaceId
   * @returns {Observable<string>}
   * @memberof WorkspaceService
   */
  getWorkspaceTypeName(workspaceId: string): Observable<string> {
    return forkJoin(
      this.optionService.getWorkspaceType(),
      this.info(workspaceId)
    ).pipe(
      map(([options,workspace]) => {
        return options.find(p => p.value == workspace.type).displayName;
      })
    );
  }

}
