import { Component, OnInit,Input } from '@angular/core';
import { NzMessageService } from 'ng-zorro-antd';
@Component({
  selector: 'km-analysis-manualadd-modal',
  templateUrl: './analysis-manualadd-modal.component.html',
  styleUrls: ['./analysis-manualadd-modal.component.less']
})
export class AnalysisManualaddModalComponent implements OnInit {

  @Input() cntrMesrType:string;

  @Input() cntrMesrReasonDesc:string;

  @Input() permanentCntr:string;

  @Input() permanentCntrTime:string;

  constructor(
    private msg:NzMessageService
  ) { }

  ngOnInit() {
  }

  myFunction(){
    if(!(/(^[1-9]\d*$)/.test(this.cntrMesrType))){
      this.msg.warning("原因区分应为正整数");
      this.cntrMesrType = null;
  }
  }
}
