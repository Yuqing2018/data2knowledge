import { Component, OnInit, forwardRef, HostListener, ViewChildren, ElementRef, QueryList, Renderer2 } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import {
  trigger,
  state,
  style,
  animate,
  transition,
} from '@angular/animations';
import { uuid } from 'src/app/core/utils';
import { DropService } from 'src/app/services/drop.service';

@Component({
  selector: 'km-intent-annotation-tool',
  templateUrl: './intent-annotation-tool.component.html',
  styleUrls: ['./intent-annotation-tool.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => IntentAnnotationToolComponent),
      multi: true
    }
  ],
  animations: [
    trigger('openClose', [
      state('open', style({
        opacity: 1,

      })),
      state('closed', style({
        opacity: 0,
        height: '0px'
      })),
      transition('open => closed', [
        animate('.2s')
      ]),
      transition('closed => open', [
        animate('.2s')
      ]),
    ]),
  ]
})
export class IntentAnnotationToolComponent implements OnInit {
  searchValue: string;
  trainingPhraseType = true;
  addValue: string;
  trainingPhraseList: any;
  trainingSearchPhraseList: any;
  activePhrase: any;
  intentSource: any;
  dropEntityList = [];

  selectedPart: any;
  selectedText: any;

  propagateChange = (_: any) => { };

  writeValue(value: any) {
    if (value) {
      this.intentSource = value;
      this.trainingPhraseList = this.intentSource.TrainingPhrases;
      this.trainingSearchPhraseList = [].concat(this.trainingPhraseList);
    }
  }

  registerOnChange(fn: any) {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any) {
  }

  constructor(
    private render: Renderer2,
    private dropService: DropService
  ) { }

  ngOnInit() {
  }

  onDragover(event: MouseEvent, el: any, phrase: any) {
    if (phrase && phrase.TrainingPhraseType == 'Example') {
      el.blur();
    } else {
      if (el.hasAttribute('contenteditable')) {
        let range = window.getSelection();
        range.selectAllChildren(el);
        range.collapseToEnd();
      }
      el.focus();
    }
    event.preventDefault();
  }

  onDragleave(event: MouseEvent, el: any, phrase: any) {
    event.preventDefault();
    el.blur();
  }

  projectContentChanged(mirror: any, input: any) {
    this.render.setStyle(
      input,
      'width',
      `${mirror.clientWidth}px`
    );
  }

  onDrop(event: any, el: any) {
    if (!this.trainingPhraseType) {
      let entity = this.dropService.dropEntity;
      this.dropEntityList.push(entity);
      this.addValue = this.addValue.substring(0, el.selectionEnd) + '@' + entity.EntityType + ' ' + this.addValue.substring(el.selectionEnd, this.addValue.length) + ' ';
    }
  }

  handleEntity(entity: any) {
    if (this.selectedPart) {
      Object.assign(this.selectedPart, {
        EntityType: entity.EntityType, Alias: entity.EntityType.toLowerCase(), UserDefined: true, Color: entity.Color,
        Text: this.activePhrase.TrainingPhraseType == 'Template' ? `@${entity.EntityType}` : this.selectedPart.Text
      });
    }
    if (this.selectedText) {
      let { start, end, phrase, part } = this.selectedText;
      let parts = [];
      let indexArr = Array.from(new Set([0, start, end, part.Text.length])).sort((a, b) => a - b);
      for (let i = 0; i < indexArr.length - 1; i++) {
        let partStart = indexArr[i];
        let partEnd = indexArr[i + 1];
        if (partStart == start && partEnd == end) {
          parts.push({ Text: part.Text.substring(partStart, partEnd), EntityType: entity.EntityType, Alias: entity.EntityType.toLowerCase(), UserDefined: true, Color: entity.Color });
        } else {
          parts.push({
            Text: part.Text.substring(partStart, partEnd),
            EntityType: null,
            Alias: null,
            UserDefined: false
          });
        }
      }
      phrase.Parts.splice(phrase.Parts.indexOf(part), 1, ...parts);
    }
    this.emitValue();
  }

  onUpdateAlias(event: any, part: any) {
    Object.assign(part, {
      Alias: event.target.textContent
    });
    this.emitValue();
  }

  onChangeType() {
    this.addValue = '';
    this.trainingPhraseType = !this.trainingPhraseType;
  }

  onSelectedEntity(event: MouseEvent, part: any) {
    this.selectedPart = part;
    event.stopPropagation();
  }

  onAddValue() {
    if (this.addValue) {
      let parts = this.trainingPhraseType ? [{
        Text: this.addValue,
        EntityType: null,
        Alias: null,
        UserDefined: false
      }] : this.parseParts();
      this.trainingPhraseList.unshift({
        Id: uuid(),
        Parts: parts,
        TrainingPhraseType: this.trainingPhraseType ? 'Example' : 'Template'
      });
      this.emitValue();
    }
    this.onSearch();
    this.addValue = '';
    this.dropEntityList = [];
  }

  parseParts() {
    let str = this.addValue;
    if (str.indexOf("@") == -1) {
      return [{
        Text: this.addValue,
        EntityType: null,
        Alias: null,
        UserDefined: false
      }]
    }
    let arr = str.split("@");
    let arr2 = [];
    arr.forEach((element, index) => {
      if (index == 0) {
        return;
      }
      arr[index] = "@" + element;
    });
    arr.forEach((element, index) => {
      if (index == 0) {
        arr2.push(element);
        return;
      }
      let spaceIndex = element.indexOf(" ");
      arr2.push(element.slice(0, spaceIndex));
      arr2.push(element.slice(spaceIndex + 1, element.length));
    });
    let arr3 = [];
    arr2.forEach((element, index) => {
      if (/^@/.test(element)) {
        let entity = this.dropEntityList.find(e => e.EntityType == element.replace(/@/, ""));
        arr3.push({
          Text: element,
          EntityType: element.replace(/@/, ""),
          Alias: element.replace(/@/, "").toLowerCase(),
          Color: entity ? entity.Color : '',
          UserDefined: false
        })
      } else {
        arr3.push({
          Text: element.trimStart(),
          EntityType: null,
          Alias: null,
          UserDefined: false
        })
      }
    })

    return arr3;
  }

  highlight(event: MouseEvent, phrase: any, part: any) {
    let selectObject = document.getSelection();
    this.selectedText = {
      start: Math.min(selectObject.anchorOffset, selectObject.focusOffset),
      end: Math.max(selectObject.anchorOffset, selectObject.focusOffset),
      text: selectObject.toString(),
      part,
      phrase
    }
    event.stopPropagation();
  }

  onSearch() {
    this.trainingSearchPhraseList = this.trainingPhraseList.filter(v => {
      let text = v.Parts.reduce((t, p) => {
        return t + p.Text;
      }, '');
      return !this.searchValue || text.indexOf(this.searchValue) !== -1;
    });
  }

  onRemovePhrase(index: number) {
    this.trainingPhraseList.splice(index, 1);
    this.emitValue();
    this.onSearch();
  }

  onRemovePart(part: any, phrase: any) {
    if (phrase.TrainingPhraseType == 'Example') {
      Object.assign(part, {
        EntityType: null, Alias: null, UserDefined: false, Color: null
      });
    } else {
      phrase.Parts.splice(phrase.Parts.indexOf(part), 1);
    }
    this.emitValue();
  }

  emitValue() {
    this.trainingPhraseList = this.trainingPhraseList.map(t => {
      t.Parts = t.Parts.reduce((res, cur) => {
        if (res && res.length > 0) {
          if (res[res.length - 1].EntityType || cur.EntityType) {
            res.push(cur);
          } else {
            res[res.length - 1] = {
              Text: res[res.length - 1].Text + cur.Text,
              EntityType: null,
              Alias: null,
              UserDefined: false
            }
          }
          return res;
        }
        return [cur];
      }, []);
      return t;
    })
    this.trainingPhraseList = this.trainingPhraseList.filter(t => {
      t.Parts = t.Parts.filter(cur => {
        return cur.Text !== '';
      });
      return t;
    })
    this.intentSource.TrainingPhrases = this.trainingPhraseList;
    this.propagateChange(this.intentSource);
  }

  @HostListener('window:mouseup')
  handleWindowMouseup() {
    this.selectedPart = null;
    this.selectedText = null;
  }

}
