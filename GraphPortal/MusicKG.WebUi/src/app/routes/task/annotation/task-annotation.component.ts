import { Component, OnInit, ViewChild } from '@angular/core';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { data } from './data';
import { AnnotationService } from 'src/app/shared/annotation/annotation.service';

@Component({
  selector: 'km-task-annotation',
  templateUrl: './task-annotation.component.html',
  styleUrls: ['./task-annotation.component.less'],
  animations: [
    trigger('expandState', [
      state('inactive', style({
        opacity: '0',
        height: 0,
        display: 'none'
      })),
      state('active', style({
        opacity: '1',
        height: '*'
      })),
      transition('inactive => active', animate('150ms ease-in')),
      transition('active => inactive', animate('150ms ease-out'))
    ])
  ]
})
export class TaskAnnotationComponent implements OnInit {
  annotation: any;
  status = '1';

  entityList = [
     {
       id: '1',name: '歌手', color: '#FFF92C', isExpand: true, propertyList: [
         { id: '1-1', name: '年龄', color: '#FFF92C' },
         { id: '1-2', name: '国籍', color: '#FFF92C' },
         { id: '1-3', name: '身高', color: '#FFF92C' },
       ]
     },
     {
       id: '2',name: '歌曲', color: '#E71D32', isExpand: true, propertyList: [
         { id: '2-1', name: '名称', color: '#E71D32' },
         { id: '2-2', name: '发表时间', color: '#E71D32' },
         { id: '2-3', name: '音乐风格', color: '#E71D32' },
         { id: '2-4', name: '获得奖项', color: '#E71D32' }
       ]
     },
     {
       id: '3',name: '专辑', color: '#008571', isExpand: true, propertyList: [
         { id: '3-1', name: '名称', color: '#008571' },
         { id: '3-2', name: '曲目数量', color: '#008571' }
       ]
     },
  ];

  relationList = [
    { id: '1', name: '演唱', fromEntityId: '1', toEntityId: '2'},
    { id: '2', name: '属于', fromEntityId: '2', toEntityId: '3'},
  ];

  constructor(
    public annotationService: AnnotationService
  ) { }

  ngOnInit() {
    setTimeout(() => {
      this.annotation = data; 
      this.annotationService.entityList = this.entityList;
      this.annotationService.relationList = this.relationList;
    }, 200);
  }

  addEntity(entity: any) {
    console.log(entity);
    this.annotationService.sentenceAddEntity.emit(entity);
  }

  valueChange(value: any) {
    console.log(value);
  }

  viewChange(mode: string) {
    this.annotationService.mode = mode;
    this.annotationService.modeChange.emit(mode);
  }

  tabChange(tab: any) {
    let mode = ['entity','relation','corefChain'];
    this.viewChange(mode[tab.index]);
  }

}
