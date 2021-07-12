import { Component, OnInit, TemplateRef } from '@angular/core';
import { NzModalService, NzModalRef, UploadFile, NzMessageService } from 'ng-zorro-antd';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { DictionaryService } from 'src/app/services/dictionary.service';
import { DictItem } from 'src/app/interfaces/dict.interface';
import {
  FormBuilder,
  FormGroup,
  Validators
} from '@angular/forms';
import { Platform } from '@angular/cdk/platform';

@Component({
  selector: 'km-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.less']
})
export class ListComponent implements OnInit {

  dataSet: Array<DictItem>
  workspaceId: string
  tplModal: NzModalRef;
  tplName: NzModalRef
  deleteModal: NzModalRef
  fileList: UploadFile[] = [];
  uploading: boolean = false
  validateForm: FormGroup;
  isUpload: boolean
  pageIndex: number
  dictTotal: number = 0
  resetNameId: string
  from: number = 0
  loading: boolean = false
  originName: string = null
  renameLoading: boolean = false
  isIE: boolean

  get uploadBtn() {
    return this.fileList.length === 0 || !this.validateForm.valid
  }

  constructor(
    private modal: NzModalService,
    private route: ActivatedRoute,
    private message: NzMessageService,
    private fb: FormBuilder,
    private dictService: DictionaryService,
    private platform: Platform
  ) { }

  ngOnInit() {
    this.isIE = this.platform.TRIDENT
    this.route.pathFromRoot[1].params.subscribe(res => {
      this.workspaceId = res.workspace
    })
    this.getList()
    this.validateForm = this.fb.group({
      dictName: [null, [Validators.required, Validators.maxLength(20), Validators.pattern(/^[\s\S]*.*[^\s][\s\S]*$/)]]
    });
  }

  getList(): void {
    this.dictService.getList(this.workspaceId, this.from).subscribe(res => {
      this.dataSet = res.items
      this.dictTotal = res.totalCount
      this.loading = false
      if (this.dataSet.length === 0) {
        this.message.info('暂无数据')
      }
    }, err => {
      this.message.error(err.msg)
    })
  }

  createTplModal(tplTitle: TemplateRef<{}>, tplContent: TemplateRef<{}>, tplFooter: TemplateRef<{}>): void {
    this.isUpload = true
    this.tplModal = this.modal.create({
      nzTitle: tplTitle,
      nzContent: tplContent,
      nzFooter: tplFooter,
      nzMaskClosable: false,
      nzClosable: false,
      nzWidth: "60%"
    });
  }

  beforeUpload = (file: any): Observable<boolean> | boolean => {
    var filepath = file.name;
    var fileend = filepath.substring(filepath.lastIndexOf("."));
    var reader = new FileReader();//新建一个FileReader
    reader.readAsText(file, "UTF-8");//读取文件 
    if (fileend !== '.txt') {
      this.message.error('仅接受 txt 文件');
      return false;
    } else {
      return Observable.create(observer => {
        reader.onloadend = (evt: any) => { //读取完文件之后会回来这里
          this.fileList.push(file)
          observer.next('false')
          observer.complete()
        }
      });
    }
  }

  removeFile(index: number): void {
    this.fileList.splice(index, 1);
  }

  handleUpload(): void {
    const formData = new FormData();
    this.fileList.forEach((file: any) => {
      formData.append('files', file, file.name);
    });
    formData.append('name', this.validateForm.value.dictName);
    this.uploading = true
    this.dictService.uploadDict(this.workspaceId, formData).subscribe(res => {
      this.dataSet = res.items
      this.uploading = false
      this.tplModal.destroy()
      this.validateForm.reset()
      this.fileList = []
      this.message.success('上传成功')
    }, err => {
      this.uploading = false
      this.message.error(err.msg)
    })
  }

  handleCancel(): void {
    this.fileList = []
    this.uploading = false
    this.validateForm.reset()
    this.tplModal.destroy()
  }

  showDeleteConfirm(id, event): void {
    event.stopPropagation()
    this.deleteModal = this.modal.confirm({
      nzTitle: '确认删除该字典吗？',
      nzOkType: 'danger',
      nzOnOk: () => new Promise((resolve, reject) => {
        this.dictService.deleteDict(this.workspaceId, id).subscribe(res => { resolve(res) }, err => { reject(err) })
      }).then(res => {
        this.dataSet = this.dataSet.filter(val => val.id !== id)
        this.message.success('删除成功')
        this.deleteModal.destroy()
      }).catch(err => {
        this.message.error(err.msg)
      })
    });
  }

  editDic(tplTitle: TemplateRef<{}>, tplContent: TemplateRef<{}>, tplFooter: TemplateRef<{}>, id, name, event): void {
    window.event ? window.event.cancelBubble = true : event.stopPropagation();
    this.isUpload = false
    this.resetNameId = id
    this.originName = name
    this.tplName = this.modal.create({
      nzTitle: tplTitle,
      nzContent: tplContent,
      nzFooter: tplFooter,
      nzMaskClosable: false,
      nzClosable: false,
      nzWidth: 600
    });
  }

  rename(status: string): void {
    if (status === 'cancel') {
      this.validateForm.reset()
      this.tplName.destroy()
      this.originName = null
    } else {
      for (const i in this.validateForm.controls) {
        this.validateForm.controls[i].markAsDirty();
        this.validateForm.controls[i].updateValueAndValidity();
      }
      if (this.validateForm.valid) {
        this.renameLoading = true
        this.dictService.updateDict(this.workspaceId, this.resetNameId, this.validateForm.value.dictName).subscribe(res => {
          this.dataSet.map(val => {
            if (val.id === this.resetNameId) {
              val.name = this.validateForm.value.dictName
            }
          })
          this.resetNameId = null
          this.validateForm.reset()
          this.tplName.destroy()
          this.message.success('更改成功')
          this.renameLoading = false
        }, err => {
          this.message.error(err.msg)
          this.renameLoading = false
        })
      }
    }
  }

  pageIndexChange(index: number): void {
    this.loading = true
    this.from = (index - 1) * 10
    this.getList()
  }

  hasError(controlName: string, error: string): void {
    let formControl = this.validateForm.get(controlName);
    return formControl.dirty && formControl.errors && formControl.errors[error];
  }

}
