import { Component, OnInit } from '@angular/core';
import { NzModalService, NzMessageService } from 'ng-zorro-antd';
import { schedData,confData } from 'src/app/interfaces/collection.interface';
import { CollectionFormComponent } from '../collection-form/collection-form.component';
import { CollectionService } from 'src/app/services/collection.service';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
  selector: 'km-collection-list',
  templateUrl: './collection-list.component.html',
  styleUrls: ['./collection-list.component.less']
})
export class CollectionListComponent implements OnInit {
  configData:confData[]; // 配置信息
  error: string;
  // tableData:Array<any>=[]; // 配置信息
  tableData:schedData[]=[]; //定时任务数据
  week:Array<{value:number,label:string,checked?:boolean}>=[{value:0,label:"星期一"},{value:1,label:"星期二"},{value:2,label:"星期三"},{value:3,label:"星期四"},{value:4,label:"星期五"},{value:5,label:"星期六"},{value:6,label:"星期日"}];

  constructor(
    private collectionService:CollectionService,
    private modalService: NzModalService,
    private message: NzMessageService
  ) { }

  ngOnInit() {
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
          this.configData=res.message;
          this.getScheds();
        }
      });
    }
  // 获取定时任务列表
   getScheds(){
    this.collectionService.getScheds().pipe(
      catchError(error => {
        this.error = error.message;
        return of(false);
      })
    ).subscribe(res => {
      if (res.code==200) {
        this.tableData=res.message;
        this.tableData.map(item=>{
          if(item.day_of_week){
            let arr=item.day_of_week.split(',').map((item:any)=> Number(item));
            let arr2=[]
            this.week.map(item=>{
              if(arr.includes(item.value)){
                arr2.push(item.label)
              }
            })
            item.week=arr2.join(',')
          }
          if(this.configData.length>0){
            let arr=[];
            let arr2=[];
            this.configData.forEach(item=>{
              arr.push(item._id)
              arr2.push(item.conf_name)
            })
            if(arr.includes(item.conf)){
              item.conf_name=arr2[arr.indexOf(item.conf)]
            }
          }
        })
      }
    });
  }
  
    // 新建 任务
    addTask(data?: object): void{
      let title = data===undefined ? '新建任务' : '修改任务';
      const modal = this.modalService.create({
          nzTitle: title,
          nzWidth: '60%',
          nzContent: CollectionFormComponent,
          nzComponentParams: {data:data,week:this.week, configData: this.configData},
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
                if(componentInstance.formData.id){
                  let para:any = {}
                  para.sched_name = componentInstance.formData.sched_name;
                  para.conf = componentInstance.formData.conf;
                  para.start_date = this.formatStartDate(componentInstance.formData.startDate);
                  para.day_of_week = componentInstance.formData.day_of_week;
                  if(!componentInstance.formData.isforever){
                    para.end_date = this.formatEndDate(componentInstance.formData.endDate);
                  }else{
                    para.end_date = ''
                  }
                  let hmArr=[];
                  componentInstance.timeArray.forEach((item)=>{
                    hmArr.push(this.formatHM(item.value))
                  })
                  para.hm = hmArr;
                  let url = componentInstance.formData.id;
                  // 请求接口 保存数据
                  this.collectionService.editsched(url,para).pipe(
                    catchError(error => {
                      this.error = error.message;
                      return of(false);
                    })
                  ).subscribe(res => {
                    if (res.code==200) {
                      this.message.create('success', `This is a message of ${res.message}`);
                      modal.destroy()
                      this.getScheds()
                    }else{
                      this.message.create('error', `This is a message of ${res.message}`);
                      resolve()
                    }
                  });
                } else {
                  let para:any = {}
                  para.sched_name = componentInstance.formData.sched_name;
                  para.conf = componentInstance.formData.conf;
                  para.start_date = this.formatStartDate(componentInstance.formData.startDate);
                  para.day_of_week = componentInstance.formData.day_of_week;
                  if(!componentInstance.formData.isforever){
                    para.end_date = this.formatEndDate(componentInstance.formData.endDate);
                  }
                  let hmArr=[];
                  componentInstance.timeArray.forEach((item)=>{
                    hmArr.push(this.formatHM(item.value))
                  })
                  para.hm = hmArr;
                  // 请求接口 保存数据
                  this.collectionService.createsched(para).pipe(
                    catchError(error => {
                      this.error = error.message;
                      return of(false);
                    })
                  ).subscribe(res => {
                    if (res.code==201) {
                      this.message.create('success', `This is a message of ${res.message}`);
                      modal.destroy()
                      this.getScheds()
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
    // 删除一条任务信息
    onDelete (id:string) :void{
      let url=id;
      this.collectionService.deletesched(url).pipe(
        catchError(error => {
          this.error = error.message;
          return of(false);
        })
      ).subscribe(res => {
        if (res.code==200) {
          this.message.create('success', `This is a message of ${res.message}`);
          this.getScheds()
        }else{
          this.message.create('error', `This is a message of ${res.message}`);
        }
      });

    }

    formatStartDate (val:Date):string {
      if(val){
        let value = new Date(val)
        let year = value.getFullYear()
        let month = (value.getMonth() + 1).toString().padStart(2, '0')
        let day = value.getDate().toString().padStart(2, '0')
        return year + '-' + month + '-' + day + ' '+ '00:00:00'
      }
    }
    formatEndDate (val:Date):string {
      if(val){
        let value = new Date(val)
        let year = value.getFullYear()
        let month = (value.getMonth() + 1).toString().padStart(2, '0')
        let day = value.getDate().toString().padStart(2, '0')
        return year + '-' + month + '-' + day + ' ' +'23:59:59'
      }
    }
    formatHM (val:Date):string {
      if(val){
        let value = new Date(val)
        let hour = value.getHours().toString().padStart(2, '0')
        let minutes = value.getMinutes().toString().padStart(2, '0')
        return hour + ':' + minutes
      }
    }
}
