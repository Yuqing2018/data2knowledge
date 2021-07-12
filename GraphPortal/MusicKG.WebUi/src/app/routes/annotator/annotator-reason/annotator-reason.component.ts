import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { combineLatest } from 'rxjs';
import { DictionaryService } from 'src/app/services/dictionary.service';
import { NzMessageService } from 'ng-zorro-antd';
import { DictDetail } from 'src/app/interfaces/dict.interface';
import { environment } from 'src/environments/environment';
import { CicrularService } from 'src/app/services/cicrular.service';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'km-annotator-reason',
  templateUrl: './annotator-reason.component.html',
  styleUrls: ['./annotator-reason.component.less']
})
export class AnnotatorReasonComponent implements OnInit {

  workspaceId: string
  items: Array<any> = []
  value: string
  total: number = 1
  searchLoading: boolean = false
  searchFrom: number = 0
  pageIndex: number = 1

  isVisible = false
  newValue = ''
  newBadGrade = 'A'

  updateBadGrade = 'A'

  isUpdateVisible = false
  updateValue = ''
  activeItem: any;

  modelLoading = false;

  constructor(
    private route: ActivatedRoute,
    private dictService: DictionaryService,
    private message: NzMessageService,
    private cicrularService: CicrularService
  ) { }

  ngOnInit() {
    this.searchValue()
  }

  search() {
    if (!this.value || !this.value.replace(/\s+/g, '') && this.value) {
      this.message.warning('空白字符不是合法词条')
      return
    }
    this.pageIndex = 1
    this.searchFrom = 0
    this.searchValue()
  }

  inputKeydown(event: any) {
    if (event.keyCode === 13) {
      this.search()
    }
  }

  searchValue() {
    this.searchLoading = true
    this.cicrularService.getSyndrome(this.value || '', this.searchFrom, 10).subscribe(res => {
      this.items = res.items
      this.total = res.totalCount
      this.searchLoading = false
    }, err => {
      this.searchLoading = false
      this.message.error(err.msg)
    })
  }

  pageIndexChange(index: number) {
    this.searchFrom = (index - 1) * 10
    this.searchValue()
  }

  reset() {
    this.searchFrom = 0
    this.value = ''
    this.pageIndex = 1
    this.searchValue();
  }

  add() {
    this.isVisible = true
    this.newValue = ''
    this.newBadGrade = 'A'
  }

  handleOk() {
    if (this.newValue && this.newValue.trim().length > 0) {
      this.modelLoading = true;
      this.cicrularService.addSyndrome({
        name: this.newValue,
        badGrade: this.newBadGrade
      }).pipe(
        finalize(() => this.modelLoading = false)
      ).subscribe(res => {
        this.message.success('新增成功')
        this.isVisible = false
        this.reset()
      }, err => {
        this.message.error(err.msg);
      });
    } else {
      this.message.warning('请输入不良症状');
    }
  }

  handleCancel() {
    this.isVisible = false
    this.isUpdateVisible = false
  }

  edit(item: any) {
    this.activeItem = item
    this.updateValue = item.name
    this.updateBadGrade = item.badGrade
    this.isUpdateVisible = true
  }

  handleUpdateOk() {
    if (this.updateValue && this.updateValue.length > 0) {
      this.modelLoading = true;
      this.cicrularService.updateSyndrome(this.activeItem.id, {
        name: this.updateValue,
        badGrade: this.updateBadGrade
      }).pipe(
        finalize(() => this.modelLoading = false)
      ).subscribe(res => {
        this.handleCancel()
        this.message.success('修改成功')
        this.activeItem.name = this.updateValue
        this.activeItem.badGrade = this.updateBadGrade
      }, err => {
        this.message.error(err.msg);
      })
    } else {
      this.message.warning('请输入不良症状');
    }
  }

}
