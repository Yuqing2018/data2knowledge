import { Component, OnInit, forwardRef, Input, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

export interface TokenizationModel {
  Id: string;
  Text: string;
  SpanItems: SpanModel[];
}

export interface SpanModel {
  Start: number;
  End: number;
  Text?: string;
  Color?: string;
}

export interface WordModel {
  Text: string;
  Index: number;
}

@Component({
  selector: 'km-tokenization-annotation-tool',
  templateUrl: './tokenization-annotation-tool.component.html',
  styleUrls: ['./tokenization-annotation-tool.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TokenizationAnnotationToolComponent),
      multi: true
    }
  ]
})
export class TokenizationAnnotationToolComponent implements OnInit {
  data: TokenizationModel;
  private _spanList: SpanModel[];

  @Input() showConflict = false;

  set spanList(value: SpanModel[]) {
    let spanData = [];
    let spanPointSet = new Set([0, this.data.Text.length]);
    value.filter(v => (v.Start < this.data.Text.length) && (v.End <= this.data.Text.length)).forEach(v => {   
      spanPointSet.add(v.Start).add(v.End);
    });
    let spanPointList = Array.from(spanPointSet).sort((a, b) => a - b);
    for (let index = 0; index < (spanPointList.length - 1); index++) {
      let exist = value.find(v => v.Start == spanPointList[index] && v.End == spanPointList[index + 1]);
      spanData.push({
        Start: spanPointList[index],
        End: spanPointList[index + 1],
        Text: this.data.Text.substring(spanPointList[index], spanPointList[index + 1]),
        Color: exist ? exist.Color : '#f5222d'
      });
    }
    this._spanList = spanData;
  }

  get spanList() {
    return this._spanList;
  }

  constructor() { }

  ngOnInit() {
  }

  propagateChange = (_: any) => { };

  registerOnChange(fn: any) {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any) {
  }

  writeValue(value: TokenizationModel) {
    if (value) {
      this.data = value;
      this.spanList = value.SpanItems;
      this.emit();
    }
  }

  emit() {
    this.data.SpanItems = this.spanList;
    this.propagateChange(this.data);   
  }

  connectSpan(index: number) {
    let current = this.spanList[index];
    let next = this.spanList[index + 1];
    let spanData = this.spanList.filter((v, i) => (i !== index) && (i !== (index + 1)));
    this.spanList = [...spanData, {
      Start: current.Start,
      End: next.End
    }];
    this.emit();
  }

  splitSpan(span: SpanModel, index: number) {
    let spanData = this.spanList.filter(v => v.Start !== span.Start && v.End !== span.End);
    if (index == (span.End - 1)) {
      spanData = [...this.spanList, {
        Start: span.Start,
        End: (span.End - 1)
      }, {
        Start: (span.End - 1),
        End: span.End
      }];
    } else {
      spanData = [...this.spanList, {
        Start: span.Start,
        End: index + 1
      }, {
        Start: index + 1,
        End: span.End
      }];
    }
    this.spanList = spanData;
    this.emit();
  }

}

@Component({
  selector: 'km-span',
  template: `
    <span *ngFor="let item of wordList" (click)="splitSpan.emit(item.Index)">
      {{ item.Text }}
    </span>
  `
})
export class SpanComponent {
  wordList: WordModel[];

  @Input()
  set data(value: SpanModel) {
    if (value) {
      this.wordList = value.Text.split('').map((t, i) => {
        return {
          Text: t,
          Index: value.Start + i
        }
      }) 
    }
  }

  @Output() splitSpan = new EventEmitter();

}
