import { Injectable, EventEmitter } from '@angular/core';
import { WordComponent } from './word.component';
import { throttleTime, filter } from 'rxjs/operators';
import { Entity } from './interface';

@Injectable({
  providedIn: 'root'
})
export class AnnotationService {
  mode = 'entity'

  entityList = [];

  relationList = [];

  selection: any;  

  sentenceSelectionEvent = new EventEmitter();

  sentenceAddEntity = new EventEmitter();

  sentenceShowEntity = new EventEmitter();

  fontSizeChange = new EventEmitter();

  modeChange = new EventEmitter();

  constructor() { }

  getEntityById(id: string) {
     for(let entity of this.entityList) {
       if(id == entity.id) {
         return entity;
       }          
       for(let pro of entity.propertyList) {
         if(id == pro.id) {
           return pro
         }
       }
     }
  }

  getRelationById(id: string) {
    return this.relationList.find(relation => relation.id == id);
  }
}
