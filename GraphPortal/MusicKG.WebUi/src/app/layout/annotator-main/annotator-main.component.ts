import { Component, OnInit, TemplateRef, ViewChild, OnDestroy } from '@angular/core';
import { NzNotificationService } from 'ng-zorro-antd';
import { timer, of } from 'rxjs';
import { switchMapTo, switchMap, tap, filter, finalize } from 'rxjs/operators';
import { Router } from '@angular/router';
import { CicrularService } from 'src/app/services/cicrular.service';

@Component({
  selector: 'km-annotator-main',
  templateUrl: './annotator-main.component.html',
  styleUrls: ['./annotator-main.component.less']
})
export class AnnotatorMainComponent implements OnInit, OnDestroy {
  @ViewChild(TemplateRef) template: TemplateRef<{}>;
  count: any;
  oldCount: number;

  getCount$: any;

  constructor(
    private cicrularService: CicrularService,
    private notificationService: NzNotificationService,
    private router: Router
  ) {
    this.notificationService.config({
      nzPlacement: 'bottomRight',
      nzDuration: 0,
      nzMaxStack: 1
    });
  }

  ngOnInit() {
    this.getCount$ = timer(0, 1000 * 60 * 2).pipe(
      switchMap(() => this.cicrularService.warningCount()),
      tap(count => {
        this.oldCount = +(localStorage.getItem('warningCount') || 0);
        localStorage.setItem('warningCount', (count || 0) + '')
      }),
      filter(count => count > 0),
      filter(count => {
        this.count = count;
        const prev = JSON.parse(localStorage.getItem('warningConfirm'));
        if (!prev || !prev.confirm) {
          return true;
        }
        if (count > this.oldCount) {
          localStorage.setItem('warningConfirm', JSON.stringify({
            count: count,
            confirm: false
          }))
        }
        return count > this.oldCount;
      }),
    ).subscribe(res => {
      this.notificationService.template(this.template, {
        nzData: {
          count: res
        }
      });
    });
  }

  close() {
    this.notificationService.remove();
    localStorage.setItem('warningConfirm', JSON.stringify({
      count: this.count,
      confirm: true
    }))
  }

  goto() {
    if (this.router.url.indexOf('classification') == -1) {
      this.router.navigateByUrl('/', { skipLocationChange: true }).then(() =>
        this.router.navigate(['/annotator/circular/track'])
      );
    } else {
      this.router.navigate(['/annotator/circular/track']);
    }
    this.close();
  }

  ngOnDestroy() {
    this.getCount$.unsubscribe();
    this.notificationService.remove();
  }

}
