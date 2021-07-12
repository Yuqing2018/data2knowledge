import { Component, OnInit, forwardRef, EventEmitter, Output, Input } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { NzMessageService } from 'ng-zorro-antd'

@Component({
  selector: 'km-entitytype',
  templateUrl: './entitytype.component.html',
  styleUrls: ['./entitytype.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => EntitytypeComponent),
      multi: true
    }
  ]
})
export class EntitytypeComponent implements OnInit {

  entries: any
  value: string
  @Input() index: any
  @Output() alias = new EventEmitter()
  @Output() entity = new EventEmitter()
  @Output() deleteEntity = new EventEmitter()

  constructor(
    private message: NzMessageService
  ) { }

  ngOnInit() {
  }

  registerOnChange(fn: any): void {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any): void {
  }

  propagateChange = (_: any) => { };

  writeValue(value: any) {
    if (value) {
      let Synonyms = value.Synonyms.map(el => {
        return {
          inputable: false,
          value: el[0]
        }
      })
      this.entries = {
        Value: { inputable: value.Value[0] ? false : true, value: value.Value[0] },
        Synonyms
      }
    }
  }

  editEntity(input: boolean, e?: any) {
    if (this.checkValue(this.entries.Value.value)) return
    if (!input) {
      this.autoFocus(e.currentTarget)
    } else {
      this.entity.emit({ data: this.entries.Value.value, index: this.index })
    }
    this.entries.Value.inputable = !input
  }

  delete(type: string, idx?: number) {
    if (type === 'entity') {
      this.deleteEntity.emit(this.index)
    } else {
      this.entries.Synonyms.splice(idx, 1)
      this.alias.emit({ type: 'delete', data: null, index: this.index, aliasIdx: idx })
    }
  }

  addAlias() {
    if (!this.value || this.value.trim().length === 0) {
      this.message.error('输入不能为空')
      return
    }
    this.value.trim().split(' ').map(val => {
      if (val !== '') {
        this.entries.Synonyms.push({ inputable: false, value: val })
        this.alias.emit({ type: 'add', data: val, index: this.index })
      }
    })
    this.value = null
  }

  editAlias(num: any, inputable: boolean, e?: any) {
    if (this.checkValue(this.entries.Synonyms[num].value)) return
    if (!inputable) {
      this.autoFocus(e.currentTarget)
    } else {
      this.alias.emit({ type: 'change', data: this.entries.Synonyms[num].value, index: this.index, aliasIdx: num })
    }
    this.entries.Synonyms[num].inputable = !inputable
  }

  autoFocus(targert: any) {
    let parent = targert.parentElement.parentElement
    setTimeout(() => {
      parent.lastElementChild.focus()
    }, 0)
  }

  checkValue(str: string): boolean {
    if (!str || str.trim().length === 0) {
      this.message.error('输入不能为空')
      return true
    }
    return false
  }
}
