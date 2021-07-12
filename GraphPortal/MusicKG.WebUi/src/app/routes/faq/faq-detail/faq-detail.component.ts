import { Component, OnInit } from '@angular/core';
import { FaqService, FaqInfoModel } from 'src/app/services/faq.service';
import { ActivatedRoute } from '@angular/router';
import { map, switchMap } from 'rxjs/operators';

@Component({
  selector: 'km-faq-detail',
  templateUrl: './faq-detail.component.html',
  styleUrls: ['./faq-detail.component.less']
})
export class FaqDetailComponent implements OnInit {
  faqInfo: FaqInfoModel;

  constructor(
    private faqService: FaqService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.route.params.pipe(
      map(p => p.id),
      switchMap(id => this.faqService.detail(id))
    ).subscribe(res => {
      this.faqInfo = res.bean;
    });
  }

  goBack() {
    window.history.back();
  }

}
