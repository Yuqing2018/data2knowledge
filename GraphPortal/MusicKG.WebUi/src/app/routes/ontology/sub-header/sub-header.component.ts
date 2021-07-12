import { Component, OnInit,TemplateRef, Input } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NzModalRef, NzModalService,NzMessageService,UploadFile } from 'ng-zorro-antd';
import { WorkspaceService } from 'src/app/services/workspace.service';
import { switchMap , tap,takeUntil} from 'rxjs/operators';
import { Observable,Subject } from 'rxjs';
import { saveAs } from 'file-saver';
import { OntologyService } from 'src/app/services/ontology.service';
const Menu = [
  {
    title: '实体类型',
    link: '../',
    type: ['5c414c0a5a01cf00010f9661']
  },
  // {
  //   title: '关系',
  //   link: '../relation',
  //   type: ['5c414c0a5a01cf00010f9661']
  // },
  // {
  //   title: '图',
  //   link: '../figure',
  //   type: []
  // },
];

@Component({
  selector: 'km-sub-header',
  templateUrl: './sub-header.component.html',
  styleUrls: ['./sub-header.component.less']
})
export class SubHeaderComponent implements OnInit {
  private unsubscribe = new Subject<void>();
  tabIndex: number;
  attachmentList: any[] = [];
  tplModal: NzModalRef;//模态框对象
  // isOkLoading = false;//模态框按钮是否显示加载中
  mainFile: any;
  fileList: UploadFile[] = []; // 要上传的文件
  uploading :boolean= false; // 上传按钮loading;
  workspaceId: string;

  menuData: any;
  workspaceType: string; 
  type:string=Menu[0].type[0];

  @Input() labelType: string = ''
  


  constructor(
    private router: Router,
    private modalService: NzModalService,
    private route: ActivatedRoute,
    private workspaceService: WorkspaceService,
    private msg: NzMessageService,
    private OntologyService: OntologyService,
  ) { }

  ngOnInit() {
    this.route.pathFromRoot[1].params.pipe(
      takeUntil(this.unsubscribe),
      switchMap(w => this.workspaceService.info(w.workspace))
    ).subscribe(res => {
      this.workspaceType = res.type.id;
      this.menuData = Menu.filter(m => m.type.some(t => t == this.workspaceType));
      let len = window.location.href.split('/').length
      if (window.location.href.split('/')[len - 1] === 'ontology') {
        this.tabIndex = 0;
      }
      if (window.location.href.split('/')[len - 1] === 'relation') {
        this.tabIndex = 1;
      }
      if (window.location.href.split('/')[len - 1] === 'figure') {
        this.tabIndex = 2;
      }
    });
  }

  ngOnDestroy() {
    this.unsubscribe.next();
  }
  // 页面跳转
  skip(a: string) {
    this.router.navigate([a], { relativeTo: this.route });
  }
        // 打开上传文件模态框
    createTplModal(tplTitle: TemplateRef<{}>, tplContent: TemplateRef<{}>, tplFooter: TemplateRef<{}>): void {
      this.tplModal = this.modalService.create({
        nzTitle: tplTitle,
        nzContent: tplContent,
        nzFooter: tplFooter,
        nzMaskClosable: false,
        nzClosable: false,
        nzWidth:"50%"
      });
    }
    
         // 取消关闭模态框
      handleCancel(): void {
        this.uploading = false;
        this.fileList = [];
        this.tplModal.destroy();
      }

      // 上传文件
    beforeUpload = (file: any) :Observable<boolean> | boolean => {
      var filepath = file.name;
      var fileend = filepath.substring(filepath.lastIndexOf("."));
      var reader = new FileReader();//新建一个FileReader
          reader.readAsText(file, "UTF-8");//读取文件 
      // console.log(fileend)
      if(fileend !== '.json'){
        this.msg.error('上传文件只接收 json 格式');
        return false;
      }else{
        return Observable.create(observer => {
          reader.onloadend = (evt:any)=>{ //读取完文件之后会回来这里
              var fileString = evt.target.result; // 读取文件内容
              if (typeof fileString == 'string') {
                try {
                  var fileJson = JSON.parse(fileString);
                    if(typeof fileJson == 'object' && fileJson && Object.getOwnPropertyNames(fileJson).length == 2){
                      if(fileJson.hasOwnProperty("Entities") && fileJson.hasOwnProperty("Relations")){
                        this.fileList.push(file);
                        observer.next('false');
                        observer.complete();
                        return;
                      }else{
                        this.msg.error('上传文件不符合要求');
                      }
                    }
                } catch(e) {
                  this.msg.error('文件内容不符合 json 格式');
                }
              }
            }
          });
      }
    }
    removeFile(index:number): void{
      this.fileList.splice(index,1);
    }

    // 提交 上传
    handleUpload(): void {
      const formData = new FormData();
      this.fileList.forEach((file: any) => {
        formData.append('file', file);
      });
      this.uploading = true;
      this.route.pathFromRoot[1].params.pipe(
        takeUntil(this.unsubscribe),
        tap(param => { this.workspaceId = param.workspace }),
        switchMap(param => this.OntologyService.uploadOntology(param.workspace, formData))
      ).subscribe(res => {
        // console.log(res)
        let len = window.location.href.split('/').length
        this.msg.success('上传成功');
        this.uploading = false;
        if (window.location.href.split('/')[len - 1] === 'ontology') {
          // console.log(window.location.href)
          this.tabIndex = 0;
          this.router.navigate(['../relation'], { relativeTo: this.route ,skipLocationChange: true,queryParams: { id: 1 } });
          // this.router.navigate(['../'], { relativeTo: this.route });
        }
        this.handleCancel();
        
      }, err => {
        this.uploading = false;
        this.msg.create(err.type, err.msg);
      });
    }
    // 导出 ontology
    downloadOntology(): void{
      this.route.pathFromRoot[1].params.pipe(
        takeUntil(this.unsubscribe),
        tap(param => { this.workspaceId = param.workspace }),
        switchMap(param => this.OntologyService.downloadOntology(param.workspace))
      ).subscribe(res => {
        // console.log(res)
        // console.log(res)
        if (res && res.Entities.length > 0) {
          let downContent = {
            Entities: res.Entities.map((item: any, index: number) => {
              return item;
            }),
            Relations:res.Relations.map((item: any, index: number) => {
              return item;
            })
          };
          var blob = new Blob([JSON.stringify(downContent)], { type: "text/plain;charset=utf-8" });
          saveAs(blob, `OntologyEntity.json`);
          // this.msg.success('下载成功');
        } else {
          this.msg.warning('暂无实体类型数据'); 
        }
      }, err => {
        this.msg.create(err.type, err.msg);
      });
    }

}
