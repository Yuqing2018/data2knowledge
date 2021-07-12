import { Component, OnInit, Input, ViewEncapsulation, HostBinding, HostListener, ViewChildren, QueryList, Output, EventEmitter } from '@angular/core';
import { AnnotationService } from './annotation.service';
import { SentenceComponent } from './sentence.component';
import { Entity, Sentence } from './interface';

@Component({
  selector: 'km-text-annotation',
  template: `
   <div class="annotation-content">
     <km-sentence *ngFor="let sentence of sentenceListData" [sentence]="sentence" (valueChange)="sentenceValueChange(sentence,$event)"></km-sentence>
   </div>
  `,
  styleUrls: ['./annotation.less'],
  encapsulation: ViewEncapsulation.None
})
export class TextAnnotationComponent implements OnInit {
  sentenceListData = [];
  _annotationData: any;

  @Output() valueChange = new EventEmitter<any>();

  @Input() 
  set annotation(value: any) {
    if(value) {
      this._annotationData = value;
      this.sentenceListData = value.sentenceList.sentences;   
    }
  }

  @ViewChildren(SentenceComponent) sentenceList: QueryList<SentenceComponent>;

  constructor(
    private annotationService: AnnotationService
  ) { }

  ngOnInit() {
  }

  sentenceValueChange(sentence: Sentence, changeSentence: Sentence) {
   this._annotationData.sentenceList.sentences =  this.sentenceListData.map(s => {
       return s == sentence ? changeSentence : s;
   });
   this.valueChange.emit(this._annotationData);
  }

  @HostListener('mousedown')
  @HostListener('mouseup')
  mousedown() {
    this.annotationService.sentenceSelectionEvent.emit(false);
  }
  
}
