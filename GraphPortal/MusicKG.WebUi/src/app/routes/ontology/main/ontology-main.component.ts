import { Component, OnInit,SimpleChanges } from '@angular/core';
import {  NzModalRef, NzModalService,NzMessageService } from 'ng-zorro-antd';
import {  FormBuilder,  FormGroup,  Validators,FormControl} from '@angular/forms';
import { OntologyFormComponent } from '../ontology-form/ontology-form.component';
import { ActivatedRoute } from '@angular/router';
import { switchMap, tap, takeUntil, mergeMap } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { OntologyEntityInfo } from 'src/app/interfaces/ontology.interface';
import { OntologyService } from 'src/app/services/ontology.service';
import { Option } from 'src/app/interfaces/api.interface';
import { WorkspaceService } from 'src/app/services/workspace.service';
import { WOKSPACR_TYPE_LIST } from 'src/app/core/common'
const Menu = [
  {
    title: '实体类型',
    link: '../',
    type: ['5c414c0a5a01cf00010f9661']
  },
  {
    title: '关系',
    link: '../relation',
    type: ['5c414c0a5a01cf00010f9661']
  },
  {
    title: '图',
    link: '../figure',
    type: []
  },
];
 

@Component({
  selector: 'km-ontology-main',
  templateUrl: './ontology-main.component.html',
  styleUrls: ['./ontology-main.component.less']
})
export class OntologyMainComponent implements OnInit {
  private unsubscribe = new Subject<void>();
  entityData: OntologyEntityInfo[]; // 实体数据
  total:number = 0;
  pageIndex:number = 1;
  workspaceId: string;

  // propertityForm:FormGroup;
  validateForm: FormGroup;
  propertityForm:Array<FormGroup>=[];//验证 属性 的数组
  // 属性数据
  controlArray: Array<{ name:string,description:string,type:string}> = [];
  // 属性类型数据
  listOfOption: Option[];
  defaultType: string;
  tplModal: NzModalRef;//模态框对象
  isOkLoading = false;//模态框按钮是否显示加载中
  mainFile: any;
  attachmentList: any[] = [];//上传 文件的文档列表
  // actived node
  defaultColor:string="#ffff00";
  // 选中的entity 默认第一个
  activeIndex:number=0;
  // 选中的实体对象
  activeEntity: OntologyEntityInfo;//选中的实体数据
  oldResult: string;
  // msg :boolean=true;
  // timer=null;
  type:string=Menu[0].type[0];//是否显示属性
  labelType: string = ''

  color: string;
  workspaceType: string
  
  constructor(
    private modalService: NzModalService,
    private OntologyService: OntologyService,
    private route: ActivatedRoute,
    private msgService: NzMessageService,
    private fb: FormBuilder,
    private workspaceService: WorkspaceService
    ) {
      //  this.timer=setInterval(() => {
      //   console.log(this.msg)
      //   this.msg=this.canDeactivate();
      // }, 1000)
  
     }

  ngOnInit(): void {
    this.route.pathFromRoot[1].params.pipe(
      mergeMap(param => this.workspaceService.info(param.workspace))
    ).subscribe(res => {
      this.labelType = res.type.id === WOKSPACR_TYPE_LIST[7].value ? '标签' : '实体'
      this.workspaceType = res.type.id
    })
    this.OntologyService.getEntityPropertyType().subscribe(res => {
      // console.log(res)
      this.listOfOption = res;
      this.defaultType = this.listOfOption[0].value;
    }, err => {
      this.msgService.create(err.type, err.msg);
    })
    this.getOntologyEntity();
  }

  checkName= (control: FormControl): { [ s: string ]: boolean } => {
    // console.log(control.value)
    if (!control.value) {
      return { required: true };
    } else if (!/^[a-zA-Z0-9_]+$/.test(control.value)) {
      return { checkName: true , error: true };
    }
  };
  // 取消订阅
  ngOnDestroy() {
    // clearInterval(this.timer)
    this.unsubscribe.next();
  }
  
  getOntologyEntity(pageIndex: number = 1) {
    this.pageIndex = pageIndex;
    this.entityData = [];
    this.activeEntity = null;
    this.route.pathFromRoot[1].params.pipe(
      takeUntil(this.unsubscribe),
      tap(param => { this.workspaceId = param.workspace }),
      switchMap(param => this.OntologyService.getOntologyEntity(param.workspace, (this.pageIndex - 1) * 10))
    ).subscribe(res => {
      // console.log(res.items)
      this.entityData = res.items;
      this.total = res.totalCount;
      if(this.entityData.length>0){
        this.selectEntity(this.entityData[0].id,0);
      }
    }, err => {
      this.msgService.create(err.type, err.msg);
    });
  }
 
  // 新建 Entity
  addEntity(): void{
    // this.dropdown.close();
    let modal =  this.modalService.create({
        nzTitle: '新建' + this.labelType,
        nzContent: OntologyFormComponent,
        nzMaskClosable:false,
        nzComponentParams: {
          labelType: this.labelType
        },
        nzOnOk: (componentInstance:OntologyFormComponent) => {
          // console.log(componentInstance.ontologyForm.value);
          let form = componentInstance.ontologyForm;
          for (const i in form.controls) {
            form.controls[i].markAsDirty();
            form.controls[i].updateValueAndValidity();
          }
          if (form.valid) {
            let para = componentInstance.ontologyForm.value;
            para.color = this.workspaceType === WOKSPACR_TYPE_LIST[7].value ? '#1890ff' : this.defaultColor
            this.route.pathFromRoot[1].params.pipe(
              takeUntil(this.unsubscribe),
              switchMap((param: any) => this.OntologyService.createOntologyEntity(param.workspace, para))
            ).subscribe(res => {
              modal.destroy();
              this.msgService.success('创建' + this.labelType + '类型成功');
              this.getOntologyEntity();
            }, err => {
              this.msgService.create(err.type, err.msg);
            });
          }
          return false;
        }
    });
  }
  // 选择entity
  selectEntity (id:string,index:number): void {
    this.activeIndex=index;
    this.route.pathFromRoot[1].params.pipe(
      takeUntil(this.unsubscribe),
      switchMap((param: any) => this.OntologyService.getOntologyEntityByid(param.workspace, id))
    ).subscribe(res => {
      this.activeEntity=res;
      this.oldResult = JSON.stringify(res);
      this.controlArray = this.activeEntity.properties;
      this.validateForm = this.fb.group({
        activeEntitydescription:[ this.activeEntity.description ],
        activeEntitycolor:[ this.activeEntity.color ]
      });
      this.propertityForm = [];
      this.controlArray.forEach((item,i)=>{
        // console.log(this.areaForm[i])
          this.propertityForm[i] = this.fb.group({
            name:[ this.controlArray[i].name, [ this.checkName ] ],
            type: [ this.controlArray[i].type, [ Validators.required ] ],
            description:[this.controlArray[i].description]
          })
      })
    }, err => {
      this.msgService.create(err.type, err.msg);
    });
    
  }

  // 删除 Entity
  deleteEntity(item:any,index:number){
    this.route.pathFromRoot[1].params.pipe(
      takeUntil(this.unsubscribe),
      switchMap((param: any) => this.OntologyService.deleteOntologyEntity(param.workspace, item.id))
    ).subscribe(res => {
      this.msgService.create('success','删除成功');
      this.entityData = [];
      this.getOntologyEntity();
    }, err => {
      this.msgService.create(err.type, err.msg);
    });
  }

  // checkName= (control: FormControl): { [ s: string ]: boolean } => {
  //   console.log(control.value)
  //   if (!control.value) {
  //     return { required: true };
  //   } else if (!/^[a-zA-Z0-9_]+$/.test(control.value)) {
  //     return { checkName: true , error: true };
  //   }
  // };

// 添加属性
  addField(): void {
    let obj={
      name:'',
      type:this.defaultType,
      description:''
    }
    this.controlArray.push(obj);
    this.controlArray.forEach((item,i)=>{
      if(this.propertityForm[i] !== undefined){
        this.propertityForm[i] = this.fb.group({
          name:[ this.propertityForm[i].value.name, [ this.checkName ] ],
          type: [ this.propertityForm[i].value.type, [ Validators.required ] ],
          description:[this.propertityForm[i].value.description]
        })
      }else{
        this.propertityForm[i] = this.fb.group({
          name:[ '', [ this.checkName ] ],
          type: [ this.defaultType, [ Validators.required ] ],
          description:['']
        })
      }
    })


  }
// 删除属性
  removeField(i: { name:string,description:string,type:string }, e: MouseEvent,index:number): void {
    e.preventDefault();
      this.controlArray.splice(index, 1);
      this.propertityForm.splice(index,1);
  }

  // 保存
  updateOntologyEntity(): void {
    let valid:Array<Boolean> = [];
    if(this.controlArray.length>0){
      this.controlArray.forEach((item,i) => {
        let form = this.propertityForm[i];
        for (const i in form.controls) {
            form.controls[i].markAsDirty();
            form.controls[i].updateValueAndValidity();
          }
          valid.push(form.valid);
        })
    } 
    if (!valid.some(v => v == false)) {
      if (!this.canDeactivate()) {
        let para = {
          name : this.activeEntity.name,
          description:this.validateForm.value.activeEntitydescription,
          color: this.activeEntity.color,
          properties:this.controlArray
        }
        // console.log(para)
        this.route.pathFromRoot[1].params.pipe(
          takeUntil(this.unsubscribe),
          tap(param => { this.workspaceId = param.workspace }),
          switchMap(param => this.OntologyService.updateOntologyEntity(param.workspace, this.activeEntity.id , para))
        ).subscribe(res => {
          // console.log(res)
          this.msgService.create('success','保存成功');
          this.entityData[this.activeIndex]={ ...res };
          this.activeEntity=res;
          this.oldResult = JSON.stringify(res);
          this.controlArray = this.activeEntity.properties;
        }, err => {
          this.msgService.create(err.type, err.msg);
        });
      } 
    }
    
  }
  // 取消
  reset(): void {
    this.selectEntity(this.activeEntity.id,this.activeIndex)
    // this.activeEntity=JSON.parse(this.oldResult);
    // this.controlArray = this.activeEntity.properties;
  }

  confimDeactivate(fun: string, id?:string,index?:number) {
    if (!this.canDeactivate()) {
      this.modalService.confirm({
        nzTitle: '提示信息',
        nzContent: '存在未保存内容，确认忽略这些更改吗 ?',
        nzOnOk: () => {
          this[fun](id,index);
        }
      });
    } else {
      this[fun](id,index);
    }
  }
  // 判断是否存在未保存内容
  canDeactivate() : boolean{
    if(this.entityData.length === 0){
      return true
    } else {
      // console.log(this.oldResult)
      // console.log(this.parseResult())
      return this.oldResult == JSON.stringify(this.parseResult());
    }
  }

  parseResult() {
    this.controlArray.map((item,i)=>{
      // console.log(this.propertityForm[i].value)
      this.controlArray[i] = this.propertityForm[i].value
    })
    return {
      id : this.activeEntity.id,
      name : this.activeEntity.name,
      properties:this.controlArray,
      description:this.validateForm.value.activeEntitydescription,
      color: this.activeEntity.color
    };
  }

  hasError(control: string , i:number) {
    return this.propertityForm[i].get(control).dirty && this.propertityForm[i].get(control).errors;
  }


}
