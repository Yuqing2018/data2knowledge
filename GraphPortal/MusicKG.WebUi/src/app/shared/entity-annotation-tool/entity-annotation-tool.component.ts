import { Component, OnInit, Input, ViewEncapsulation, HostListener, ViewChildren, QueryList, forwardRef } from '@angular/core';
import { getEntityList, getWordRange, createSentenceRowList } from './helper';
import { EntityWordComponent } from './entity-annotation-word';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'km-entity-annotation-tool',
  templateUrl: './entity-annotation-tool.component.html',
  styleUrls: ['./entity-annotation-tool.component.less'],
  encapsulation: ViewEncapsulation.None,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => EntityAnnotationToolComponent),
      multi: true
    }
  ]
})
export class EntityAnnotationToolComponent implements OnInit {
  private _entityList = [];
  private _data: any;
  private _highlightEntity = [];
  wordList = [];
  labelList = [];
  lineList = [];
  labelUserList = [];
  previousWord: any;
  selection: any;
  mode = 'entity';
  activeLabel: any;


  @ViewChildren(EntityWordComponent) wordComponentList: QueryList<EntityWordComponent>;

  propagateChange = (_: any) => { };

  emitValue(event: any) {
    this.propagateChange(event)
  }

  writeValue(value: any) {
    this.clearActived();
    if (value) {    
      this._data = value; 
      this.wordList = Array.from(value.text).map((text, index) => {
        return { text, index };
      });
      this.entityList = value.entityList;
      this.renderAnnotation();
    }
  }

  registerOnChange(fn: any) {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any) {
  }

  set entityList(value: any) {
    this._entityList = getEntityList(value);
    this.propagateChange({
      ...this._data,
      entityList: this._entityList.map(e => {
        return { start: e.start, end: e.end, entityType: e.entityType }
      })
    });
  }

  get entityList() {
    return this._entityList;
  }

  get removeLabelStyle() {
    if (this.activeLabel && this.labelList && this.labelList.length > 0) {
      let label = this.labelList.find(l => l.id == this.activeLabel && l.isEnd);
      return {
        background: label.color,
        id: label.id,
        left: label.left + label.width + 'px',
        top: label.top + 'px'
      }
    }
    return null;
  }

  set highlightEntity(value: any) {
    this._highlightEntity = value;
    if(value && value.length > 0) {
      this.lineList = this.lineList.map(l => {
        l.opacity =  value.find(v => v.id == l.id) ? 1 : 0.3;
        return l;
      });
      this.labelUserList = this.labelUserList.map(l => {
        l.opacity =  value.find(v => v.id == l.id) ? 1 : 0.3;
        return l;
      });
    }else {
      this.lineList = this.lineList.map(l => {
        l.opacity = 1;
        return l;
      });
      this.labelUserList = this.labelUserList.map(l => {   
        l.opacity = 1;
        return l;
      });
    }
  }

  get highlightEntity() {
    return this._highlightEntity;
  }

  constructor() { }

  ngOnInit() {
  }

  @HostListener('window:resize')
  resizeBy() {
    this.renderAnnotation();
  }

  renderAnnotation() {
    setTimeout(() => {
      let data = createSentenceRowList(this.wordComponentList.toArray(), this.entityList);
      this.labelList = data.labelList;
      this.lineList = data.lineList;
      this.labelUserList = data.labelUserList;
    }, 0);
  }

  addEntity(entity: any) {
    if (this.selection) {
      if (this.selection.entity) {
        this.entityList = this.entityList.map(e => {
          if (e.id == this.selection.id) {
            e.start = this.selection.start;
            e.end = this.selection.end;
            e.entityType = entity.name;
            e.color = entity.color; 
            e.title = entity.name;
          }
          return e;
        });
      } else {
        let id = this.entityList.reduce((result, value) => {
          return value.id > result ? value.id : result;
        }, 0);
        this.entityList = this.entityList.concat([{
          start: this.selection.start,
          end: this.selection.end,
          entityType: entity.name,
          color: entity.color,
          title: entity.name,
          id: ++id
        }]);
      }
      this.clearActived();
      this.renderAnnotation();
    }else {
      this.highlightEntity = this.entityList.filter(e => e.entityType == entity.id);
    }
  }

  changeMode(mode: string) {
    this.mode = mode;
    this.renderAnnotation();
  }

  removeEntity(entityId: any) {
    this.entityList = this.entityList.filter(e => e.id !== entityId);
    this.clearActived();
    this.renderAnnotation();
  }

  wordEvent(type: string, word: any) {
    if (type == 'mousedown') {
      this.previousWord = word;
      this.highlightEntity = null;
      this.handleMouseDown(word);
    }
    if (type == 'mouseup') {
      this.previousWord = null;
    }
    if (type == 'mousemove') {
      if (this.previousWord && this.previousWord.index !== word.index) {
        this.handleMousemove(word, this.selection);
      }
    }
  }

  labelModusedown(label: any, event: MouseEvent) {
    let entity = this.entityList.find(e => e.id == label.id);
    this.activeLabel = label.id;
    let range = {
      start: entity.start,
      end: entity.end,
      entity: entity.entityType,
      id: entity.id,
      level: entity.level,
    };
    this.selection = range;
    this.highlightEntity = null;
    event.stopPropagation();
  }

  handleMouseDown(word: any) {
    let range = getWordRange(word, this.entityList);
    if (this.selection) {
      if (this.selection.entity) {
        this.selection = range.entity ? range : this.selection;
        this.activeLabel = this.selection.id;  
      } else {
        let s = [{ start: range.start, end: range.end }].concat(this.selection);
        this.selection = {
          start: s.reduce((result, value) => {
            return value.start <= result ? value.start : result;
          }, range.start),
          end: s.reduce((result, value) => {
            return value.end >= result ? value.end : result;
          }, range.end),
        };
      }
    } else {
      this.selection = range; 
      this.activeLabel = range.id;    
    }
  }

  handleMousemove(word: any, selection: any) {
    let activeSelection: any = { ...selection };
    if (word.index > this.previousWord.index) {
      if (word.index < selection.end && word.index >= selection.start) {
        activeSelection.start = word.index;
        activeSelection.end = selection.end;
      } else {
        activeSelection.start = selection.start;
        activeSelection.end = word.index + 1;
      }
    } else {
      if (word.index <= selection.end && word.index >= selection.start) {
        activeSelection.start = selection.start;
        activeSelection.end = word.index + 1;
      } else {
        activeSelection.start = word.index;
        activeSelection.end = selection.end;
      }
    }
    this.previousWord = word;
    if (activeSelection.entity) {
      let entityList = this.entityList.map(e => {
        if (e.id == this.selection.id) {
          e.start = activeSelection.start;
          e.end = activeSelection.end;
        }
        return e;
      });
      this.entityList = entityList;
      if (activeSelection.entity) {
        activeSelection.level = this.entityList.find(e => e.id == activeSelection.id).level;
      }
      this.renderAnnotation();
    }
    this.selection = activeSelection;
  }

  @HostListener('mousedown', ['$event'])
  @HostListener('mouseup', ['$event'])
  clearActived() {
    this.selection = null;
    this.previousWord = null;
    this.activeLabel = null;
    this.highlightEntity = null;
  }

}
