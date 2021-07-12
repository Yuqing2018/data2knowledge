import { Component, OnInit ,Input} from '@angular/core';
import  * as differenceInCalendarDays  from 'date-fns/difference_in_calendar_days';
import { CollectionService } from 'src/app/services/collection.service';
import { confData } from 'src/app/interfaces/collection.interface';

@Component({
  selector: 'km-collection-form',
  templateUrl: './collection-form.component.html',
  styleUrls: ['./collection-form.component.less']
})
export class CollectionFormComponent implements OnInit {
  today :Date = new Date(); //当天时间
  timeArray:Array<any>=[]; //时间数组

  @Input() data: any;//编辑时传来的数据
  @Input() week: Array<{value:number,label:string,checked?:boolean}>;//打开弹框时传来的数据
  @Input() configData: confData[];//打开弹框时传来的数据
  formData:any={
    id:'',  //编辑时的任务id
    sched_name:'',
    conf:null, //这里是conf_id的字符串
    startDate:null, //开始日期
    endDate:null, //结束日期
    isforever:false, 
    day_of_week:'',
    repeatCycle:'' //重复周期
  };

  constructor(
    private collectionService:CollectionService,
  ) {   }

  ngOnInit() {
    // console.log(this.data)
    // 如果是编辑过来的
    if(this.data){
      this.formData.id = this.data._id;
      this.formData.sched_name = this.data.sched_name;
      this.formData.conf = this.data.conf;
      this.formData.startDate = new Date(this.data.start_date);
      if(this.data.end_date === '2090-01-02 00:00:00'){
        this.formData.isforever = true;
      } else {
        this.formData.endDate = new Date(this.data.end_date);
      }
      let arr=this.data.day_of_week.split(',').map((item:any)=> Number(item))
      this.week.map(item=>{
        if(arr.includes(item.value)){
          item.checked = true
        }
      })
      //重复周期赋值
      this.log(this.week) 
      //重复周期 时间 赋值
      this.data.hm.forEach((item:string)=>{
        let str=this.formatHM(item)
        this.addTimeArray(new Date(str))
      })
    }
  }

// 勾选重复周期
  log(val: Array<{value:number,label:string,checked?:boolean}>): void {
    // console.log(val);
    let arr=[];
    let arr2=[];
    val.forEach(item=>{
      if(item.checked){
        arr.push(item.value)
        arr2.push(item.label)
      }
    })
    // console.log(arr2)
    this.formData.repeatCycle=arr2.join(',');
    this.formData.day_of_week=arr.join(',');
  }

  // 开始日期禁用 今天之前的
  disabledDate = (current: Date): boolean => {
    // Can not select days before today and today
    return differenceInCalendarDays(current, this.today) < 0;
  };
  // 结束日期禁用 今天之前的和 小于开始日期
  disabledEndDate = (endDate: Date): boolean => {
    if (this.formData.startDate==null){
      return differenceInCalendarDays(endDate, this.today) < 0;
    }
    return endDate.getTime() <= this.formData.startDate.getTime();
  };
  // 新建时间
  addTimeArray (val?:Date) :void{
    let time:Date;
    if(val){
       time=new Date(val)
    }else{
      time=new Date()
    }
    let obj={
      "value":time
    }
    this.timeArray.push(obj);
    // console.log(this.timeArray)
  }
  // 删除时间
  delTimeArray(item:string,index:number) :void{
    // console.log(item);
    // console.log(index);
    this.timeArray.splice(index,1)
  }

  formatHM (val:string):string {
    if(val){
      let value = new Date(val)
      let year = value.getFullYear()
      let month = (value.getMonth() + 1).toString().padStart(2, '0')
      let day = value.getDate().toString().padStart(2, '0')
      return year + '-' + month + '-' + day + ' '+ val
    }
  }

}
