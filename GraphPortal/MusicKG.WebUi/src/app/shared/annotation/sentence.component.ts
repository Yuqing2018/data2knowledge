import { Component, OnInit, Input, HostListener, ElementRef, ViewChildren, QueryList, ViewChild, EventEmitter, Output, Host } from '@angular/core';
import { Word, WordComponent } from './word.component';
import { AnnotationService } from './annotation.service';
import { filter } from 'rxjs/operators';
import { parseEntity, getWordRange, calControlPoint } from './helper';
import { PlaceholderComponent, AddPlaceholderComponent } from './placeholders.component';
import { Sentence, Entity, SentenceData, Placeholder, Label } from './interface';
import { createSentenceRowList } from './row';
import { NzModalService } from 'ng-zorro-antd';

@Component({
  selector: 'km-sentence',
  template: `
  <km-word *ngFor="let word of wordList" [word]="word" [entityList]="entityList" [sentenceRowList]="sentenceRowList" [selectionList]="selectionList" (wordEvent)="wordEvent($event,word)"></km-word>
  <km-placeholder #placeholderComponent *ngFor="let placeholder of placeholderList" (mousedown)="linkDown($event,placeholderComponent)" [placeholder]="placeholder" [sentenceRowList]="sentenceRowList" (removePlaceholder)="removePlaceholder(placeholder)"></km-placeholder>
  <div class="add-placeholder" *ngIf="annotationService.mode =='entity' || annotationService.mode =='detail'" [style.opacity]="opacity" title="新增实体" (click)="addPlaceholder()">+</div>
  <km-label *ngFor="let label of labelList" (mousedown)="labelModusedown(label,$event)" [label]="label" [active]="activeLabel"></km-label>
  <span class="realtion-text" *ngFor="let relation of relationTextList" [style.left.px]="relation.left" [style.top.px]="relation.top">{{ relation.text }}</span>
  <span class="label-line" *ngFor="let line of lineList" [style.height.px]="line?.height" [style.top.px]="line?.top" [style.left.px]="line?.left"></span>
  <div *ngFor="let remove of removeList" (mousedown)="removeEntity(remove)" [ngStyle]="remove" class="label-close">X</div>
  <svg class="relation-svg" *ngIf="annotationService.mode == 'relation'">
        <defs>
            <marker id="markEnd" markerWidth="5" fill="#ccc" markerHeight="5" refX="2.5" refY="2.5" viewBox="0 0 5 5" orient="auto"><polyline id="SvgjsPolyline1180" points="0,0 5,2.5 0,5 0.2,2.5"></polyline></marker>
        </defs>
        <path marker-end="url(#markEnd)" *ngFor="let line of relationLineList" [attr.d]="line.path" stroke="#ccc" stroke-width="2" fill="transparent"/>
  </svg>
  <svg class="link-svg" *ngIf="annotationService.mode == 'relation'">
      <defs>
        <marker id="markEnd2" markerWidth="5" fill="#1890ff" markerHeight="5" refX="2.5" refY="2.5" viewBox="0 0 5 5" orient="auto"><polyline id="SvgjsPolyline1180" points="0,0 5,2.5 0,5 0.2,2.5"></polyline></marker>
      </defs>
      <path marker-end="url(#markEnd2)" [attr.d]="linkPath" fill="none" stroke-dasharray="2,2" stroke="#ccc" stroke-width="2"/>
  </svg>
  `
})
export class SentenceComponent implements OnInit {
  id: string;
  wordList: any;
  selectionList = [];
  tokenList = [];
  labelList = [];
  anchorRange: any;
  previousWord: any;
  sentenceText: string;
  sentenceBegin: number;
  lineList = [];
  activeLabel: any;
  removeList = [];
  _placeholderList = [];
  relationList = [];
  relationLineList = [];
  _sentence: any;
  _entityList: Entity[];
  relationTextList = [];
  sentenceRowList: SentenceData;
  fromLink: any;
  fromRelation: any;
  linkPath: string = '';

  @Output() valueChange = new EventEmitter<any>();

  @ViewChildren(WordComponent) wordComponentList: QueryList<WordComponent>;

  @ViewChildren(PlaceholderComponent) placeholderComponentList: QueryList<PlaceholderComponent>;

  @Input()
  set sentence(value: Sentence) {
    let wordList = [];
    this._sentence = value;
    this.id = value.id;
    this.sentenceBegin = value.begin;
    this.sentenceText = value.text;
    this.entityList = value.entities && value.entities.map(e => {
      let entity = this.annotationService.getEntityById(e.type);
      e.color = entity.color;
      e.title = entity.name; 
      return e;
    });; 
    this.placeholderList = value.placeholders && value.placeholders.map(p => {
      let entity = this.annotationService.getEntityById(p.type);
      p.color = entity.color;
      p.text = entity.name; 
      return p;
    });
    this.tokenList = value.tokens;
    let textArray = Array.from(value.text);
    for (let index = 0; index < textArray.length; index++) {
      let word: any = { index: index + value.begin, text: textArray[index] };
      wordList.push(word);
    }
    this.wordList = wordList;
  }

  constructor(
    private hostEl: ElementRef<HTMLElement>,
    public annotationService: AnnotationService,
    private modalService: NzModalService
  ) { }

  set entityList(entities: Entity[]) {
    this._entityList = parseEntity(entities);
    this._sentence = Object.assign(this._sentence,{ 
      entities: this._entityList.map(e => {  
        return { id: e.id, type: e.type, text: e.text, begin: e.begin, end: e.end };
      })       
    })
    this.valueChange.emit(this._sentence);
  }

  get entityList() {  
    return this._entityList;
  }

  set placeholderList(placeholderList: Placeholder[]) {
    this._placeholderList = placeholderList || [];   
    this._sentence = Object.assign(this._sentence,{
      placeholders: this._placeholderList.map(e => {  
        return { id: e.id, type: e.type };
      })       
    });
    this.valueChange.emit(this._sentence);
  }

  get placeholderList() {
    return this._placeholderList;
  }

  renderSentence() {
    setTimeout(() => {
      let sentence = Object.assign({},this._sentence);
      sentence.entities = sentence.entities.map(e => {
        let entity = this.annotationService.getEntityById(e.type);
        e.color = entity.color;
        e.title = entity.name; 
        return e;
      })
      sentence.placeholders = sentence.placeholders.map(p => {
        let entity = this.annotationService.getEntityById(p.type);
        p.color = entity.color;
        p.text = entity.name; 
        return p;
      });
      sentence.relations = sentence.relations.map(r => {
        let relation = this.annotationService.getRelationById(r.type);
        r.text = relation.name; 
        return r;
      });
      let data: any;
      if(this.annotationService.mode == 'detail') {
        sentence = { ...sentence, relations: []}
      }
      if(this.annotationService.mode == 'entity') {
        sentence = { ...sentence, entities: [], relations: []}
      }   
      data = createSentenceRowList(sentence, this.wordComponentList.toArray(), this.placeholderComponentList.toArray(), this.hostEl.nativeElement);
      this.sentenceRowList = data;
      this.relationLineList = data.connectLineList;
      this.labelList = data.labelList;
      this.lineList = data.lineList;
      this.relationTextList = data.relationTextList; 
      this.renderLabelRemove();
      console.log(data); 
    }, 0);
  }

  ngOnInit() {
    this.annotationService.sentenceSelectionEvent.pipe(
      filter(event => event != this.id)
    ).subscribe(() => {
      this.clearSelection();
    });
    this.annotationService.sentenceAddEntity.pipe(
      filter(() => this.selectionList && this.selectionList.length > 0)
    ).subscribe((value: any) => {
      this.addEntity(value);
    });
    this.annotationService.modeChange.subscribe(value => {
      this.renderSentence();
      this.removeList = [];
    })
  }

  ngAfterViewInit() {
  }

  addPlaceholder() {
    this.modalService.create({
      nzTitle: '新增实体',
      nzContent: AddPlaceholderComponent,
      nzOnOk: (ref: any) => {
        if(ref.placeholder) {
          let id = this.placeholderList.reduce((result, value) => {
            let index = +value.id.split(`${this.id}-place`)[1];
            return index > result ? index : result;
          }, 0);
          let entity = this.annotationService.getEntityById(ref.placeholder)
          this.placeholderList = [].concat(this.placeholderList, [{
              id:  `${this.id}-place${id + 1}`,
              type: ref.placeholder,
              color: entity.color,
              text: entity.name
          }]);
          return true;
        }
        return false;
      }
    });
  }

  removePlaceholder(placeholder: Placeholder) {
      this.placeholderList = this.placeholderList.filter(p => p != placeholder);
  }

  addEntity(value: any) {
    let id = this.entityList.reduce((result, value) => {
      let index = +value.id.split(`${this.id}-e`)[1];
      return index > result ? index : result;
    }, 0);
    if (this.selectionList[0].entity) {
      this.entityList = this.entityList.map(e => {
        if (e.id == this.selectionList[0].entity) {
          e.type = value.id;
          e.color = value.color;
        }
        return e;
      });
    } else {
      let entity = {
        begin: this.selectionList[0].begin,
        end: this.selectionList[0].end,
        color: value.color,
        text: this.sentenceText.substring((this.selectionList[0].begin - this.sentenceBegin),(this.selectionList[0].end - this.sentenceBegin)),
        title: value.name,
        type: value.id,
        id: `${this.id}-e${id + 1}`
      };
      this.entityList = [].concat(this.entityList, [entity]);
    }
    this.clearSelection();
    this.renderSentence();
  }

  clearSelection() {
    this.anchorRange = null;
    this.previousWord = null;
    this.annotationService.selection = null;
    this.activeLabel = null;
    this.selectionList = [];
    this.renderLabelRemove();
  }

  wordEvent(type: string, word: any) {
    if (type == 'mousedown') {
      this.previousWord = word;
      this.handleMouseDown(word);
      this.annotationService.sentenceSelectionEvent.emit(this.id);
    }
    if (type == 'mouseup') {
      this.previousWord = null;
    }
    if (type == 'mousemove') {
      if (this.previousWord && this.previousWord.index !== word.index) {
        this.handleMousemove(word, {
          begin: this.selectionList[0].begin,
          end: this.selectionList[0].end,
          entity: this.selectionList[0].entity,
          level: this.selectionList[0].level,
        });
        this.renderLabelRemove();
      } else {
        return;
      }
    }
    this.annotationService.selection = (this.selectionList && this.selectionList.length > 0) ? this.selectionList : this.annotationService.selection;
  }

  handleMousemove(word: any, selection: any) {
    let activeSelection: any = { ...selection };
    if (word.index > this.previousWord.index) {
      if (word.index < selection.end && word.index >= selection.begin) {
        activeSelection.begin = word.index;
        activeSelection.end = selection.end;
      } else {
        activeSelection.begin = selection.begin;
        activeSelection.end = word.index + 1;
      }
    } else {
      if (word.index <= selection.end && word.index >= selection.begin) {
        activeSelection.begin = selection.begin;
        activeSelection.end = word.index + 1;
      } else {
        activeSelection.begin = word.index;
        activeSelection.end = selection.end;
      }
    }
    this.previousWord = word;
    if (this.anchorRange.entity) {
      let entityList = this.entityList.map(e => {
        if (e.id == this.anchorRange.entity) {
          e.begin = activeSelection.begin;
          e.end = activeSelection.end;
          e.text = this.sentenceText.substring((e.begin - this.sentenceBegin),(e.end - this.sentenceBegin));
        }
        return e;
      });
      this.entityList = parseEntity(entityList);
      if (activeSelection.entity) {
        activeSelection.level = this.entityList.find(e => e.id == activeSelection.entity).level;
      }
      this.renderSentence();
    }
    this.selectionList = [activeSelection];
  }

  handleMouseDown(word: any) {
    let range = getWordRange(word, this.entityList, this.tokenList);
    this.anchorRange = this.anchorRange || range;
    if (this.anchorRange.entity && range.entity) {
      this.activeLabel = [range.entity];
      this.renderLabelRemove();
      this.anchorRange = range;
      this.selectionList = [range];
      return;
    }
    if (!this.anchorRange.entity) {
      let token = this.tokenList.find(t => word.index >= t.begin && word.index < t.end);
      let s = [{ begin: token.begin, end: token.end }].concat(this.selectionList);
      this.selectionList = [{
        begin: s.reduce((result, value) => {
          return value.begin <= result ? value.begin : result;
        }, token.begin),
        end: s.reduce((result, value) => {
          return value.end >= result ? value.end : result;
        }, token.end),
      }];
    }
  }

  @HostListener('window:resize')
  resizeBy() {
    this.renderSentence();
  }

  labelModusedown(label: any, event: MouseEvent) {
    this.activeLabel = [label.id];
    let entity = this.entityList.find(e => e.id == label.id);
    let range = {
      begin: entity.begin,
      end: entity.end,
      entity: entity.id,
      level: entity.level,
    };
    this.anchorRange = range;
    this.selectionList = [range];
    this.annotationService.sentenceSelectionEvent.emit(this.id);
    this.linkDown(event,label);
    this.renderLabelRemove();
    event.stopPropagation();
  }

  linkDown(event: MouseEvent,source: any) {
    if(this.annotationService.mode == 'relation') {
      this.fromLink = event;
      this.fromRelation = source;
      let rect = this.hostEl.nativeElement.getBoundingClientRect();
      this.linkPath = `
        M ${event.x - rect.left} ${event.y - rect.top} 
      `;
    }
  }

  renderLink(to: any) {
    let rect = this.hostEl.nativeElement.getBoundingClientRect();
    let x = to.x - rect.left + 1;
    let y = to.y - rect.top - 1
    let toLabel = this.labelList.find(label => {
      return x >= label.left && x <= (label.left + label.width) && y >= label.top && y<= (label.top + label.height) && label.id !== this.fromRelation.id;
    });
    let toPlacehoder = this.placeholderComponentList.map((p: any) => {
      let rect = p.hostEl.nativeElement.getBoundingClientRect();
      let hostRect = this.hostEl.nativeElement.getBoundingClientRect();
      p.left = rect.left - hostRect.left;
      p.width = rect.width;
      p.top = rect.top - hostRect.top
      return p;
    }).find(placeholder => {
      return x >= placeholder.left && x <= (placeholder.left + placeholder.width) && y >= placeholder.top && y<= (placeholder.top + placeholder.height);
    });
    if(toLabel || toPlacehoder) {
      let toSource = toLabel || toPlacehoder;
      if(toLabel) {
        toSource = this.labelList.find(t => t.id == toLabel.id && t.isBegin);   
      }    
      let fromX = this.fromRelation.left + this.fromRelation.width / 2;
      let fromY = this.fromRelation.top;
      x = toSource.left + toSource.width / 2;
      y = toSource.top;  
      let upY = Math.min(fromY,y) - 10;      
      this.linkPath = `
        M ${fromX - 10} ${fromY}
        L ${fromX > x ? (fromX - 20) : fromX} ${upY}
        L ${fromX > x ? (x + 10) : (x - 10)} ${upY}
        L ${x} ${y - 2.5}
      `;
      this.fromLink = null;
      this.fromRelation = null;
    }else {
      this.linkPath += `
        L ${x} ${y} 
      ` ;
    }
  }

  @HostListener('mousemove',['$event'])
  mousemove(event: MouseEvent) {
      if(this.fromLink) {
        this.renderLink(event);
      }
  }

  @HostListener('mouseup',['$event'])
  mouseup(event: MouseEvent) {
    this.fromLink = null;
    this.fromRelation = null;
    this.linkPath = '';
  }

  renderLabelRemove() {
    setTimeout(() => {
      let remove = [];
      if (this.annotationService.mode != 'relation') {
        let activeLabel = this.labelList.filter(l => this.activeLabel && this.activeLabel.indexOf(l.id) !== -1 && l.isEnd);
        for (let label of activeLabel) {
          remove.push({
            background: label.color,
            id: label.id,
            left: label.left + label.width + 'px',
            top: label.top + 'px'
          });
        }
      }
      this.removeList = remove; 
    }, 0);
  }

  removeEntity(remove: any) {
    this.entityList = this.entityList.filter(e => e.id !== remove.id);
    this.clearSelection();
    this.renderSentence();
  }
}


