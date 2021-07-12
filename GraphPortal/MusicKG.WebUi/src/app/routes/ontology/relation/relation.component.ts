import { Component, OnInit } from '@angular/core';
import {  NzModalService,NzMessageService  } from 'ng-zorro-antd';
import { RelationFormComponent } from '../relation-form/relation-form.component';
import { OntologyEntityInfo,RelationData } from 'src/app/interfaces/ontology.interface';
import { ActivatedRoute,Router } from '@angular/router';
import { switchMap, tap,takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { OntologyService } from 'src/app/services/ontology.service';


@Component({
  selector: 'km-relation',
  templateUrl: './relation.component.html',
  styleUrls: ['./relation.component.less']
})
export class RelationComponent implements OnInit {
  private unsubscribe = new Subject<void>();
  relationDataSet: RelationData[]=[];
  entityData: OntologyEntityInfo[]; // 实体数据
  workspaceId: string;
  total:number = 0;
  pageIndex:number = 1;
  entityPageIndex:number = 1;
  pageSize:number = 10;
  loading:boolean = true; //表格加载loading
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private OntologyService: OntologyService,
    private msgService: NzMessageService,
    private modalService: NzModalService
  ) { 
    route.queryParams.subscribe(queryParams => {
      // console.log(queryParams)
      if(queryParams.id==1){
        this.router.navigate(['../'], { relativeTo: this.route });
      }
    })
  }

  ngOnInit() {
    this.searchData()
    this.getOntologyEntity()
  }
  ngOnDestroy() {
    this.unsubscribe.next();
  }
  // 获取表格数据
  searchData(reset: boolean = false): void {
    // console.log(reset)
    if (reset) {
      this.pageIndex = 1;
    }
    this.loading = true;
    this.route.pathFromRoot[1].params.pipe(
      takeUntil(this.unsubscribe),
      tap(param => { this.workspaceId = param.workspace }),
      switchMap(param => this.OntologyService.getOntologyRelationship(param.workspace, (this.pageIndex - 1) * 10, 10))
    ).subscribe(res => {
      // console.log(res)
      this.relationDataSet = res.items;
      this.loading = false;
      this.total = res.totalCount;
    }, err => {
      this.msgService.create(err.type, err.msg);
    });
  }

  // 新建 Entity
  addRelation(data?: any) {
    let title = data===undefined ? '新建关系' : '修改关系';
    var modal = this.modalService.create({
        nzTitle: title,
        nzContent: RelationFormComponent,
        nzComponentParams: {data:data, entityData: this.entityData},
        nzMaskClosable:false,
        nzOnOk: (componentInstance) => { 
          //  console.log(componentInstance.ontologyForm.value);
           let form = componentInstance.ontologyForm;
           for (const i in form.controls) {
             form.controls[i].markAsDirty();
             form.controls[i].updateValueAndValidity();
           }
           if (form.valid) {
            //  console.log(data)
             if(data){
               let para = componentInstance.ontologyForm.value;
               this.route.pathFromRoot[1].params.pipe(
                takeUntil(this.unsubscribe),
                 switchMap((param: any) => this.OntologyService.updateOntologyRelation(param.workspace, data.id, para))
               ).subscribe(res => {
                //  console.log(res)
                 this.msgService.success('修改成功');
                 modal.destroy();
                 this.searchData();
                }, err => {
                  this.msgService.create(err.type, err.msg);
                });
              }else {
                let para = componentInstance.ontologyForm.value;
                this.route.pathFromRoot[1].params.pipe(
                  takeUntil(this.unsubscribe),
                  switchMap((param: any) => this.OntologyService.createOntologyRelation(param.workspace, para))
                ).subscribe(res => {
                  // console.log(res)
                  this.msgService.success('创建成功');
                  modal.destroy();
                  this.searchData();
                 }, err => {
                   this.msgService.create(err.type, err.msg);
                 });
               }
               
             } 
             return false;
        }
      })
  }
  // 获取Entity节点数据
  getOntologyEntity(entityPageIndex: number = 1) {
    this.entityPageIndex = entityPageIndex;
    this.entityData = [];
    this.route.pathFromRoot[1].params.pipe(
      takeUntil(this.unsubscribe),
      tap(param => { this.workspaceId = param.workspace }),
      switchMap(param => this.OntologyService.getOntologyEntity(param.workspace, (this.entityPageIndex - 1) * 10))
    ).subscribe(res => {
      // console.log(res.items)
      this.entityData = res.items;
      // console.log(this.activeEntity.color)
    }, err => {
      this.msgService.create(err.type, err.msg);
    });
  }
  // deleteOntologyRelation
    // 删除 关系
    onDelete(relationId:string,){
      this.route.pathFromRoot[1].params.pipe(
        takeUntil(this.unsubscribe),
        switchMap((param: any) => this.OntologyService.deleteOntologyRelation(param.workspace, relationId))
      ).subscribe(res => {
        // console.log(res)
        this.msgService.create('success','删除成功');
        if(this.relationDataSet.length === 1){
          this.searchData(true);
        } else {
          this.searchData();
        }
      }, err => {
        this.msgService.create(err.type, err.msg);
      });
    }
}
