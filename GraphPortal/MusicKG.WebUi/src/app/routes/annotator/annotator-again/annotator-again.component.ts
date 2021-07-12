import { Component, OnInit } from '@angular/core';
import { AgainService} from 'src/app/services/again.service'
import { NzModalService,NzMessageService } from 'ng-zorro-antd';
@Component({
  selector: 'km-annotator-again',
  templateUrl: './annotator-again.component.html',
  styleUrls: ['./annotator-again.component.less']
})
export class AnnotatorAgainComponent implements OnInit {
  modalTitle = '新增'
  items: Array<any> = []
  itemTO:Array<any> = []
  total: number = 1
  searchLoading: boolean = false
  showAll:boolean = true
  searchFrom: number = 0
  pageIndex: number = 1

  isVisible = false
  rwName:string = ''
  rwDescription:string = ''
  startGZ = ''
  endGZ = ''
  activeItem:any

  deleteItem:any
  selectedValue = '全部'
  selectedStatus = ''
  constructor(
    private message: NzMessageService,
    private AgainService: AgainService,
    private modal: NzModalService
  ) { }
  
  pageIndexChange(index: number) {
    this.searchFrom = (index - 1) * 10
    this.searchValue()
  }
  searchValue() {
    this.searchLoading = true
    if(this.selectedValue == '全部'){
      this.selectedStatus = ''
    }else{
      this.selectedStatus = this.selectedValue
    }
    this.AgainService.getAgainList(this.searchFrom, 10,this.selectedStatus,this.showAll).subscribe(res => {
      this.items = res.items
      this.total = res.totalCount
      this.searchLoading = false
    }, err => {
      this.searchLoading = false
      this.message.error(err.msg)
    })
  }
  
  searchSure() {
    this.pageIndex = 1
    this.searchFrom = 0
    this.searchValue()
  }

  resett() {
    this.searchFrom = 0
    this.selectedValue = '全部'
    this.pageIndex = 1
    this.searchValue();
  }
  
  add() {
    this.modalTitle = '新增'
    this.isVisible = true
    this.rwName = ''
    this.rwDescription = ''
    this.startGZ = ''
    this.endGZ = ''
  }
  handleAddOk() {
    this.items.map(item =>{
      const to = new Date(item.to)
      const itemto = to.getFullYear() + '-' + (to.getMonth() + 1) + '-' + to.getDate() + ' ';
      this.itemTO.push(itemto)
    });
      const d = new Date(this.startGZ);  
      const resDate = d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDate() + ' '; 
    if (!this.rwName || !this.rwName.replace(/\s+/g, '') && this.rwName) {
      this.message.warning('请输入任务名称')
      return
    }
    else if (!this.rwDescription || !this.rwDescription.replace(/\s+/g, '') && this.rwDescription) {
      this.message.warning('请输入任务描述')
      return
    }
    else if(this.startGZ == "" || this.endGZ == ""){
      this.message.warning('请输入日期') 
    }
    else if(this.startGZ >= this.endGZ){
      this.message.warning('起始故障时间必须小于结束故障时间！')
        return
    }else if(this.itemTO.indexOf(resDate) != -1){
          this.message.warning('故障日期重叠,请重新输入日期！')
      return
    }
    else {
      this.AgainService.addAgainData({
        name:this.rwName,
        description:this.rwDescription,
        from: this.startGZ,
        to: this.endGZ
      }).subscribe(res => {
        this.isVisible = false
        this.resett()
        this.message.success('新增成功')
      }, err => {
        this.message.error(err.msg);
      })
    }
  }

  handleCancel() {
    this.isVisible = false
  }

  edit(item: any) {
    this.activeItem = item
    this.rwName = item.name
    this.rwDescription = item.description
    this.startGZ = item.from
    this.endGZ = item.to
    this.modalTitle = '修改'
    this.isVisible = true
  }

  handleUpdateOk() {
    this.AgainService.updateAgainData(this.activeItem.id, {
      name: this.rwName,
      description: this.rwDescription,
      from: this.startGZ,
      to: this.endGZ
    }).subscribe(res => {
      this.handleCancel()
      this.message.success('修改成功')
      this.activeItem.name = this.rwName
      this.activeItem.description = this.rwDescription
      this.activeItem.from = this.startGZ
      this.activeItem.to = this.endGZ
    }, err => {
      this.message.error(err.msg);
    })
  }
 
  onDelete(data: any) {
    this.AgainService.delAgain(data.id).subscribe(res => {
      this.message.info('删除成功');
      if(this.total % 10 == 1){
        this.pageIndex = this.pageIndex-1
        this.searchFrom = (this.pageIndex-1)*10
      }
      this.searchValue()
    });
  }

  ngOnInit() {
    this.searchValue();
  }

}
