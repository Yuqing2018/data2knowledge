import { Component, OnInit, TemplateRef } from '@angular/core';
import { NzModalService, NzModalRef, NzMessageService, UploadFile } from 'ng-zorro-antd';
import { PreMarkService } from 'src/app/services/pre-mark.service';
import { Option } from 'src/app/interfaces/api.interface';
import { DocumentData } from 'src/app/interfaces/pre-mark.interface';
import { ActivatedRoute, Router } from "@angular/router";
import { catchError, switchMap, tap, takeUntil } from 'rxjs/operators';
import { of, Observable, Subject } from 'rxjs';
import { TaskFormComponent } from 'src/app/shared/task-form/task-form.component';
import { Task } from 'src/app/interfaces/task.interface';
import { TaskService } from 'src/app/services/task.service';
import { WorkspaceService } from 'src/app/services/workspace.service';
import { WOKSPACR_TYPE_LIST, isVehicleFailureClassification } from 'src/app/core/common';
import { ajv } from 'src/app/app.component';
import { Platform } from '@angular/cdk/platform';

@Component({
  selector: 'km-document-list',
  templateUrl: './document-list.component.html',
  styleUrls: ['./document-list.component.less']
})
export class DocumentListComponent implements OnInit {
  private unsubscribe = new Subject<void>();
  error: string;
  DocumentData: DocumentData[] = [];
  tplModal: NzModalRef;//模态框对象
  pageIndex: number = 1;
  pageSize: number = 10;
  total: number = 0;
  loading: boolean = true; //表格加载loading
  startTime = '';
  endTime = '';
  statuses = [];
  tagsArray: Array<{ value: string }> = []; // 上传文档标签数组
  workspaceId: string = '';
  fileList: UploadFile[] = []; // 要上传的文件
  uploading: boolean = false; // 上传按钮loading;
  documentName: string = ''; //查询的文档名
  edit_document_data: DocumentData; //修改文档 时 传的数据
  jsonContentHtml: string = '';
  listOfOption: Option[];
  isDown = false;

  allChecked = false;
  indeterminate = false;
  tag: string;

  taskDocCheckedList: DocumentData[] = [];

  datePicker: Date[]

  workspaceType: string;
  isGraph: boolean = false
  graphSrc: string

  docItems: Array<string> = [];

  isVehicleFailureClassification = true;

  constructor(
    private PreMarkService: PreMarkService,
    private taskService: TaskService,
    private modalService: NzModalService,
    private route: ActivatedRoute,
    private router: Router,
    private msg: NzMessageService,
    private workspaceService: WorkspaceService,
    private platform: Platform
  ) {
    this.route.pathFromRoot[1].params.pipe(
      tap(r => { this.workspaceId = r.workspace }),
      switchMap(() => this.workspaceService.info(this.workspaceId))
    ).subscribe(r => {
      this.workspaceType = r.type.id;
      this.isGraph = this.workspaceType === WOKSPACR_TYPE_LIST[7].value;
      this.isVehicleFailureClassification = isVehicleFailureClassification(this.workspaceType);
    });
  }

  get tagValidate() {
    return this.tagsArray[0] && this.tagsArray[0].value.length > 20;
  }

  get addTaskDisabled() {
    return this.taskDocCheckedList.length == 0;
  }

  addTask() {
    let modal = this.modalService.create({
      nzTitle: '新建任务',
      nzWidth: 600,
      nzContent: TaskFormComponent,
      nzComponentParams: {
        task: <Task>{
          documentIds: this.taskDocCheckedList.map(d => d.id),
          overlap: 100,
          expectedDueAt: new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate() + 1)
        },
        workspaceId: this.workspaceId,
        workspaceType: this.workspaceType,
        docList: this.taskDocCheckedList
      },
      nzOnOk: (componentInstance: TaskFormComponent) => {
        let form = componentInstance.taskForm;
        for (const i in form.controls) {
          form.controls[i].markAsDirty();
          form.controls[i].updateValueAndValidity();
        }
        if (form.valid) {
          this.route.pathFromRoot[1].params.pipe(
            switchMap((param: any) => this.taskService.add(param.workspace, form.value))
          ).subscribe(() => {
            modal.destroy();
            this.router.navigate([`/workspace/${this.workspaceId}/task`]);
          }, err => {
            this.msg.create(err.type, err.msg);
          });
        }
        return false;
      }
    })
  }

  checkOne(data: any) {
    if (data.checked) {
      this.taskDocCheckedList = this.taskDocCheckedList.filter(doc => doc.id !== data.id).concat([data]);
    } else {
      this.taskDocCheckedList = this.taskDocCheckedList.filter(doc => doc.id !== data.id);
    }
    this.refreshStatus();
  }

  refreshStatus() {
    const allChecked = this.DocumentData.length > 0 && this.DocumentData.every((value: any) => value.checked === true);
    const allUnChecked = this.DocumentData.every((value: any) => !value.checked);
    this.allChecked = allChecked;
    this.indeterminate = (!allChecked) && (!allUnChecked);
  }

  checkAll(value: boolean) {
    if (value) {
      this.taskDocCheckedList = this.taskDocCheckedList.filter(doc => !this.DocumentData.find(d => d.id == doc.id)).concat(this.DocumentData);
    } else {
      this.taskDocCheckedList = this.taskDocCheckedList.filter(doc => !this.DocumentData.find(d => d.id == doc.id));
    }
    this.DocumentData.forEach((data: any) => {
      data.checked = value;
    });
    this.refreshStatus();
  }

  ngOnInit() {
    this.PreMarkService.getDocumentStatus().subscribe(res => {
      let arr = res
      this.listOfOption = arr.filter(option => option.value == 'Assigned' || option.value == 'Preannotated')
    }, err => {
      this.msg.create(err.type, err.msg);
    })
    this.searchData()
  }
  // 取消订阅
  ngOnDestroy() {
    this.unsubscribe.next();
  }
  // 获取表格数据
  searchData(reset: boolean = false): void {
    // console.log(reset)
    // { from, size, keyword,fromUploadedAt,toUploadedAt,statuses,isToAutoRecommendate}
    // let workspaceId = '5c3da59a4020220001fb02f7';

    if (Boolean(this.startTime) !== Boolean(this.endTime)) {
      this.msg.error('请选择上传时间')
      return
    }
    if (this.startTime && this.endTime && new Date(this.startTime) > new Date(this.endTime)) {
      this.msg.error('请选择正确的时间范围')
      return
    }
    if (this.startTime && this.endTime) {
      this.onChange([this.startTime, this.endTime])
    }
    this.loading = true;
    if (reset) {
      this.pageIndex = 1;
    }
    let para = {
      from: (this.pageIndex - 1) * this.pageSize,
      size: this.pageSize,
      keyword: this.documentName,
      fromUploadedAt: this.startTime,
      toUploadedAt: this.endTime,
      statuses: this.statuses,
      isToAutoRecommendate: false,
      tag: this.tag ? this.tag : ''
    }
    this.route.pathFromRoot[1].params.pipe(
      takeUntil(this.unsubscribe),
      tap(param => { this.workspaceId = param.workspace }),
      switchMap(param => this.PreMarkService.getDocumentList(param.workspace, para))
    ).subscribe(res => {
      // console.log(res.items)
      this.DocumentData = res.items.map((d: any) => {
        if (this.taskDocCheckedList.find(doc => doc.id == d.id)) {
          d.checked = true;
        }
        return d;
      });
      this.loading = false;
      this.total = res.totalCount;
      this.refreshStatus();
    }, err => {
      this.msg.create(err.type, err.msg);
    });
  }
  // 重置
  resetData(): void {
    this.tag = '';
    this.statuses = [];
    this.startTime = '';
    this.endTime = '';
    this.documentName = '';
    this.datePicker = []
    this.searchData()
  }


  // 打开上传文件模态框
  createTplModal(tplTitle: TemplateRef<{}>, tplContent: TemplateRef<{}>, tplFooter: TemplateRef<{}>): void {
    if (this.tagsArray.length === 0) {
      this.addTag();
    }
    this.tplModal = this.modalService.create({
      nzTitle: tplTitle,
      nzContent: tplContent,
      nzFooter: tplFooter,
      nzMaskClosable: false,
      nzClosable: false,
      nzWidth: "60%"
    });
  }
  // 编辑文档 editTitle, editContent, editFooter
  editTplModal(editTitle: TemplateRef<{}>, editContent: TemplateRef<{}>, editFooter: TemplateRef<{}>, data: any): void {
    this.edit_document_data = data;
    if (data.tags && data.tags.length > 0) {
      // console.log('1')
      data.tags.forEach(item => {
        let obj = {
          value: item
        }
        this.tagsArray.push(obj)
      })
    } else {
      let obj = {
        value: ''
      }
      this.tagsArray.push(obj)
    }
    this.tplModal = this.modalService.create({
      nzTitle: editTitle,
      nzContent: editContent,
      nzFooter: editFooter,
      nzMaskClosable: false,
      nzClosable: false,
      nzWidth: "60%"
    });
  }

  // 取消关闭模态框
  handleCancel(): void {
    this.uploading = false;
    this.tagsArray = [];
    this.fileList = [];
    this.docItems = []
    this.tplModal.destroy();
  }

  // 增加一个标签
  addTag(e?: MouseEvent) {
    if (e) {
      e.preventDefault();
    }
    let obj = {
      "value": ''
    }
    this.tagsArray.push(obj)
  }
  // 删除一个标签
  removeTag(i: { id: number, name: string, value: string, selectedValue: string }, e: MouseEvent, index) {
    e.preventDefault();
    this.tagsArray.splice(index, 1);
  }

  // 上传文件
  beforeUpload = (file: any): Observable<boolean> | boolean => {
    var filepath = file.name;
    var fileend = filepath.substring(filepath.lastIndexOf("."));
    var reader = new FileReader();//新建一个FileReader
    reader.readAsText(file, "UTF-8");//读取文件 
    if (this.workspaceType === WOKSPACR_TYPE_LIST[7].value || this.isVehicleFailureClassification) {
      return Observable.create(observer => {
        this.fileList.push(file);
        this.docItems.push('0');
        observer.next('false');
        observer.complete();
        return;
      })
    }
    if (fileend !== '.json') {
      if (fileend == '.txt' && this.workspaceType == WOKSPACR_TYPE_LIST[2].value) {
        return Observable.create(observer => {
          this.fileList.push(file);
          this.docItems.push('0');
          observer.next('false');
          observer.complete();
          return;
        })
      }
      this.msg.error('上传文件只接收 json 格式');
      return false;
    } else {
      return Observable.create(observer => {
        reader.onloadend = (evt: any) => { //读取完文件之后会回来这里
          var fileString = evt.target.result; // 读取文件内容
          if (typeof fileString == 'string') {
            try {
              var fileJson = JSON.parse(fileString.trim());
              if (typeof fileJson == 'object' && fileJson) {
                if (this.validateJsonContent(fileJson)) {
                  this.docItems.push(fileJson.Items.length)
                  this.fileList.push(file);
                  observer.next('false');
                  observer.complete();
                  return;
                } else {
                  console.log(ajv.errors);
                  this.msg.error('上传文件不符合要求');
                }
              }
            } catch (e) {
              this.msg.error('文件内容不符合 json 格式');
            }
          }
        }
      });
    }
  }

  validateJsonContent(content: string) {
    let workspace = WOKSPACR_TYPE_LIST.find(v => v.value == this.workspaceType);
    return ajv.validate(workspace.annotation, content);
  }

  removeFile(index: number) {
    this.fileList.splice(index, 1);
    this.docItems.splice(index, 1)
  }

  // 提交 上传
  handleUpload(): void {
    const formData = new FormData();
    this.fileList.forEach((file: any) => {
      formData.append('files', file, file.name);
    });
    this.tagsArray.filter(tag => !!tag.value).forEach((tag: any) => {
      formData.append('tags', tag.value);
    });
    this.docItems.forEach(val => {
      formData.append('documentItems', val)
    })
    this.uploading = true;
    this.PreMarkService.uploadDocument(this.workspaceId, formData).subscribe((res) => {
      this.msg.success('上传成功');
      this.uploading = false;
      this.handleCancel();
      this.searchData();
    }, err => {
      this.uploading = false;
      this.msg.create(err.type, err.msg);
    }, () => {
      this.docItems = []
    });
  }
  // 确定修改 document
  updateDocument(): void {
    let tagArr = []
    this.tagsArray.forEach((tag: any) => {
      tagArr.push(tag.value)
    });
    let para = {
      name: this.edit_document_data.name,
      status: this.edit_document_data.status,
      tags: tagArr
    }
    this.uploading = true;
    this.PreMarkService.updateDocument(this.workspaceId, para, this.edit_document_data.id).pipe(
      catchError(error => {
        this.error = error.message;
        return of(false);
      })
    ).subscribe((res) => {
      // console.log(res)
      this.msg.success('修改成功');
      this.uploading = false;
      this.handleCancel();
      this.searchData();
    }, err => {
      this.msg.create(err.type, err.msg);
    });

  }

  // 格式化时间
  formatTime(val: string): string {
    this.formatTimeIE()
    if (val) {
      let value = new Date(val)
      let year = value.getFullYear()
      let month = (value.getMonth() + 1).toString().padStart(2, '0')
      let day = value.getDate().toString().padStart(2, '0')
      return year + '-' + month + '-' + day
    } else {
      return ''
    }
  }
  // 删除一条文档
  onDelete(id: string): void {
    this.PreMarkService.deleteDocument(this.workspaceId, id).pipe(
      catchError(error => {
        this.error = error.message;
        return of(false);
      })
    ).subscribe((res) => {
      // console.log(res)
      this.msg.success('删除成功');
      this.taskDocCheckedList = this.taskDocCheckedList.filter(doc => doc.id !== id);
      if (this.DocumentData.length === 1) {
        this.searchData(true);
      } else {
        this.searchData();
      }
    }, err => {
      this.msg.create(err.type, err.msg);
    });
  }
  // 查看文档
  downloadDocument(workspaceId: string, id: string, jsonContent: TemplateRef<{}>) {
    if (this.workspaceType === WOKSPACR_TYPE_LIST[7].value) {
      this.PreMarkService.getGraph(workspaceId, id).subscribe(res => {
        let src = ''
        let bytes = new Uint8Array(res)
        for (let len = bytes.length, i = 0; i < len; i++) {
          src += String.fromCharCode(bytes[i])
        }
        this.graphSrc = 'data:image/png;base64,' + window.btoa(src)
        this.modalService.create({
          nzTitle: "查看文档",
          nzContent: jsonContent,
          nzFooter: null,
          nzMaskClosable: false,
          nzClosable: true,
          nzWidth: "60%"
        });
      })
      return
    }
    this.PreMarkService.downloadDocument(workspaceId, id).subscribe((res) => {
      this.jsonContentHtml = JSON.stringify(res, undefined, 2);
      this.modalService.create({
        nzTitle: "查看文档",
        nzContent: jsonContent,
        nzFooter: null,
        nzMaskClosable: false,
        nzClosable: true,
        nzWidth: "60%"
      });
    }, err => {
      this.msg.create(err.type, err.msg);
    });
  }

  onChange(result): void {
    let start = this.formatTime(result[0])
    let end = this.formatTime(result[1])

    if (this.platform.TRIDENT) {
      start = start.replace(/-/g, '/')
      end = end.replace(/-/g, '/')
    }

    this.startTime = new Date(start + ' 00:00:00').toISOString()

    let endTime = new Date(end + ' 23:59:59')
    endTime.setMilliseconds(999)
    this.endTime = endTime.toISOString()
  }

  formatTimeIE() {
    if (!String.prototype.padStart) {
      String.prototype.padStart = function padStart(targetLength, padString) {
        targetLength = targetLength >> 0; //truncate if number, or convert non-number to 0;
        padString = String(typeof padString !== 'undefined' ? padString : ' ');
        if (this.length >= targetLength) {
          return String(this);
        } else {
          targetLength = targetLength - this.length;
          if (targetLength > padString.length) {
            padString += padString.repeat(targetLength / padString.length); //append to original to ensure we are longer than needed
          }
          return padString.slice(0, targetLength) + String(this);
        }
      };
    }
  }

}
