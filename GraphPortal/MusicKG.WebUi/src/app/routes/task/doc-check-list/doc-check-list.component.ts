import { Component, OnInit, Input, forwardRef } from '@angular/core';
import { PreMarkService } from 'src/app/services/pre-mark.service';
import { DocumentData } from 'src/app/interfaces/pre-mark.interface';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

export interface Tag {
  text: string;
  num: number;
  value: string;
  checked?: boolean;
}

@Component({
  selector: 'km-doc-check-list',
  templateUrl: './doc-check-list.component.html',
  styleUrls: ['./doc-check-list.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DocCheckListComponent),
      multi: true
    }
  ]
})
export class DocCheckListComponent implements OnInit, ControlValueAccessor {
  docList: DocumentData[];
  tagList: Tag[];
  type = 'tag';
  status = 'notUse';

  propagateChange = (_: any) => { };

  @Input() workspaceId: string;

  constructor(
    private preMarkService: PreMarkService
  ) { }

  ngOnInit() {
    this.preMarkService.getCreateTaskDocumentList(this.workspaceId).subscribe(res => {
      this.docList = res.items;
      let tagSet = new Set();
      this.docList.forEach(d => {
        if (d.tags && d.tags.length > 0) {
          d.tags.forEach(t => {
            tagSet.add(t);
          })
        } else {
          tagSet.add('无标签');
        }
      });
      this.tagList = Array.from(tagSet).map(t => {
        return { text: t, num: this.docList.filter(d => {
          if(t == '无标签') {
            return d.tags && d.tags.length == 0;
          }
          return d.tags && d.tags.some(g => g == t);
        }).length, value: t };
      });
    });
  }

  checkChange() {
    let value: any;
    if (this.type == 'tag') {
      let checkTag = this.tagList.filter(t => t.checked).map(t => t.value);
      value = this.docList.filter(d => { 
        return d.tags && d.tags.some(t => checkTag.indexOf(t) !== -1) || (d.tags.length == 0 && checkTag.indexOf('无标签') !== -1);  
      }).map(d => d.id);
    } else if (this.type == 'doc') {
      value = this.docList.filter((d: any) => d.checked).map(d => d.id);
    } else {
      value = this.docList.filter((d: any) => {
        if (this.status == 'notUse') {
          return d.status !== 'Assigned';
        }
        return d.status == 'Assigned';
      }).map(d => d.id);
    }
    this.propagateChange(value);
  }

  writeValue(value: any) {
  }

  registerOnChange(fn: any) {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any) {
  }

}
