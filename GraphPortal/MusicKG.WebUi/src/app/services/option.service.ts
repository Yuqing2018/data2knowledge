import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Option } from '../interfaces/api.interface';
import { map, shareReplay } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class OptionService {
  private cache: Observable<Option[]>;

  constructor(
    private http: HttpClient
  ) { }

  /**
   * Get all option list from cache
   *
   * @returns {Observable<Option[]>}
   * @memberof OptionService
   */
  getAllOptions(): Observable<Option[]> {
    if (!this.cache) {
      this.cache = this.http.get<Option[]>(`/Option?culture=zh-CN`).pipe(
        shareReplay(1)
      );
    }             
    return this.cache;
  }

  /**
   * Get user role option list
   *
   * @returns {Observable<Option>}
   * @memberof OptionService
   */
  getUserRole(): Observable<Option[]> {
    return this.getAllOptions().pipe(
      map(optionList => {
        return optionList.filter(option => option.type == 'UserRole');
      })
    );
  }

  /**
   * Get language option list
   *
   * @returns {Observable<Option>}
   * @memberof OptionService
   */
  getLanguage(): Observable<Option[]> {
    return this.getAllOptions().pipe(
      map(optionList => {
        return optionList.filter(option => option.type == 'Language');
      })
    );
  }

  /**
   * Get workspace type option list
   *
   * @returns {Observable<Option[]>}
   * @memberof OptionService
   */
  getWorkspaceType(): Observable<Option[]> {
    return this.getAllOptions().pipe(
      map(optionList => {
        return optionList.filter(option => option.type == 'WorkspaceType');
      })
    );
  }

  /**
   * Get task status option list
   *
   * @returns {Observable<Option[]>}
   * @memberof OptionService
   */
  getTaskStatus(): Observable<Option[]> {
    return this.getAllOptions().pipe(
      map(optionList => {
        return optionList.filter(option => option.type == 'TaskStatus');
      })
    );
  }

  /**
   * Get document status option list
   *
   * @returns {Observable<Option[]>}
   * @memberof OptionService
   */
  getDocumentStatus(): Observable<Option[]> {
    return this.getAllOptions().pipe(
      map(optionList => {
        return optionList.filter(option => option.type == 'DocumentStatus');
      })
    );
  }

  /**
   * Get task document status option list
   *
   * @returns {Observable<Option[]>}
   * @memberof OptionService
   */
  getTakDocumentStatus(): Observable<Option[]> { 
    return this.getAllOptions().pipe(
      map(optionList => {
        return optionList.filter(option => option.type == 'TaskDocumentStatus');
      })
    );
  }

  /**
   * Get task type
   *
   * @returns {Observable<Option[]>}
   * @memberof OptionService
   */
  getTaskType() : Observable<Option[]> { 
    return this.getAllOptions().pipe(
      map(optionList => {
        return optionList.filter(option => option.type == 'TaskTypes');
      })
    );
  }

}
