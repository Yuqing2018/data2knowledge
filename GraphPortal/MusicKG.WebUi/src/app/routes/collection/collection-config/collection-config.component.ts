import { Component, OnInit } from '@angular/core';
import { NzModalService, NzMessageService } from 'ng-zorro-antd';
import { ConfigFormComponent } from '../config-form/config-form.component';
import { CollectionService } from 'src/app/services/collection.service';
import { confData } from 'src/app/interfaces/collection.interface';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: 'km-collection-config',
  templateUrl: './collection-config.component.html',
  styleUrls: ['./collection-config.component.less']
})
export class CollectionConfigComponent implements OnInit {
  isOkLoading:boolean = false;
  error: string;
  tableData:confData[]=[];
  workspaceId:string;

  constructor(
    private modalService: NzModalService,
    private collectionService:CollectionService,
    private message: NzMessageService,
    private route: ActivatedRoute
  ) { 
    this.route.pathFromRoot[1].params.subscribe(r => {
      // console.log(r)
      this.workspaceId = r.workspace;
    });
  }

  ngOnInit() {
    // console.log(this.workspaceId )
    this.getCollectionConf()
  }
  // 获取配置列表
  getCollectionConf():void{
    this.collectionService.getCollectionConf().pipe(
      catchError(error => {
        this.error = error.message;
        return of(false);
      })
    ).subscribe(res => {
      if (res.code==200) {
        this.tableData=res.message;
        // console.log(this.tableData)
      }
    });
  }

  // 新建 or 编辑 配置信息
  addConfig(data?: object): void{
    // console.log(data);
    let title = data===undefined ? '新建配置' : '修改配置';
    const modal = this.modalService.create({
      nzTitle: title,
      nzWidth: '60%',
      nzContent: ConfigFormComponent,
      nzComponentParams: {data:data},
      nzMaskClosable:false,
      nzFooter: [
        {
          label: '取消',
          shape: 'default',
          onClick: () => modal.destroy()
        },
        {
          label: '确定',
          shape: 'primary',
          onClick: (componentInstance) => new Promise(resolve => {
            // window.setTimeout(resolve, 2000)
            // console.log(componentInstance)
            if(componentInstance.id){
              console.log('编辑')
              let para:any = {}
              para.conf_name = componentInstance.validateForm.value.configName;
              para.desc = componentInstance.validateForm.value.desc;
              para.workspace_id = this.workspaceId;
              para.items = [];
              let url=componentInstance.id;
              // let arr=[]
              componentInstance.areaForm.forEach( item =>{
                para.items .push(item.value)
              })
              // console.log(para)
              // 请求接口 保存数据
              this.collectionService.editConf(url,para).pipe(
                catchError(error => {
                  this.error = error.message;
                  return of(false);
                })
              ).subscribe(res => {
                // console.log(res)
                if (res.code==200) {
                  this.isOkLoading=false;
                  this.message.create('success', `This is a message of ${res.message}`);
                  modal.destroy()
                  this.getCollectionConf()
                }else{
                  this.message.create('error', `This is a message of ${res.message}`);
                  resolve()
                }
              });
            } else {
              console.log('新建')
              let para:any = {}
              para.conf_name = componentInstance.validateForm.value.configName;
              para.desc = componentInstance.validateForm.value.desc;
              para.workspace_id = this.workspaceId;
              para.items = [];
              // let arr=[]
              componentInstance.areaForm.forEach( item =>{
                para.items .push(item.value)
              })
              // console.log(para)
              // 请求接口 保存数据
              this.collectionService.createConf(para).pipe(
                catchError(error => {
                  this.error = error.message;
                  return of(false);
                })
              ).subscribe(res => {
                if (res.code==201) {
                  this.isOkLoading=false;
                  this.message.create('success', `This is a message of ${res.message}`);
                  modal.destroy()
                  this.getCollectionConf()
                }else{
                  this.message.create('error', `This is a message of ${res.message}`);
                  resolve()
                }
              });
            }
          })
        }]
      });
    }
    // 删除一条配置信息
    onDelete (id:string) :void{
      let url=id;
      this.collectionService.deleteconf(url).pipe(
        catchError(error => {
          this.error = error.message;
          return of(false);
        })
      ).subscribe(res => {
        // console.log(res)
        if (res.code==200) {
          this.message.create('success', `This is a message of ${res.message}`);
          this.getCollectionConf()
        }else{
          this.message.create('error', `This is a message of ${res.message}`);
        }
      });

    }


}
