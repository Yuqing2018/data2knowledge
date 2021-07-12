import { Injectable } from '@angular/core';
import { CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { NzModalService } from 'ng-zorro-antd';

export interface AnnotationCanDeactivate {
  canDeactivate(): boolean;
}

@Injectable({
  providedIn: 'root'
})
export class CanDeactivateGuard implements CanDeactivate<AnnotationCanDeactivate> {

  constructor(
    private modalService: NzModalService
  ) { }

  canDeactivate(component: AnnotationCanDeactivate): Observable<boolean> | Promise<boolean> | boolean {
    if (component.canDeactivate()) {
      return true;
    }
    return Observable.create(observer => {
      this.modalService.confirm({
        nzTitle: '提示信息',
        nzContent: '内容还未保存，确认离开吗 ?',
        nzOnCancel: () => {
          observer.next(false);
          observer.complete();
        },
        nzOnOk: () => {
          observer.next(true);
          observer.complete();
        }
      });
    });
  }
}
