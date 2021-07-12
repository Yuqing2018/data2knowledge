import { Component, OnInit } from '@angular/core';
import { NzModalService } from 'ng-zorro-antd';
import { RuleFormComponent } from '../rule-form/rule-form.component';
import { RuleService } from 'src/app/services/rule.service';
import { Dict } from '../../../interfaces/dict.interface';

@Component({
  selector: 'km-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.less']
})
export class ListComponent implements OnInit {

  okText: string
  cancel: string
  dictData: Dict[]

  constructor(
    private modalService: NzModalService,
    private ruleService: RuleService
  ) { }

  ngOnInit() {
    this.ruleService.getData().subscribe(res => {
      this.dictData = res
    })
  }

  addRule(type) {
    if (type === 'dict') {
      this.getModal('新建字典', type)
    } else {
      this.getModal('新建正则表达式', type)
    }
  }

  getModal(msg, type) {
    this.modalService.create({
      nzTitle: msg,
      nzContent: RuleFormComponent,
      nzWidth: 600,
      nzComponentParams: {
        type: type
      },
      nzOnOk: () => {
        console.log(22222)
      }
    })
  }

}
