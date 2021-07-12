import { Component, OnInit, TemplateRef, HostListener, forwardRef, Input, Output, EventEmitter } from '@angular/core'
import { NzModalRef, NzModalService, NzMessageService, UploadFile } from 'ng-zorro-antd'
import { Observable } from 'rxjs'
import * as Papa from 'papaparse'
import { NG_VALUE_ACCESSOR } from '@angular/forms'
import { EntitytypeData, Menus } from '../../interfaces/entitytype'

@Component({
  selector: 'km-entitytype-annotation-tool',
  templateUrl: './entitytype-annotation-tool.component.html',
  styleUrls: ['./entitytype-annotation-tool.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => EntitytypeAnnotationToolComponent),
      multi: true
    }
  ]
})
export class EntitytypeAnnotationToolComponent implements OnInit {

  id: string
  menus: Menus[]
  currentNode: any
  docItems: Array<any> = []
  tplModal: NzModalRef
  itemIndex: number = 0
  nodes: EntitytypeData[]
  fileList: Array<UploadFile> = []

  @Output() private childOuter = new EventEmitter()

  constructor(
    private modalService: NzModalService,
    private message: NzMessageService
  ) { }

  ngOnInit() { }

  createTree(arr) {
    let top = [], sub = [], arrObj = {};
    arr.forEach(item => {
      if (!item.parentId) {
        item.level = 1
        top.push(item)
      } else {
        sub.push(item)
      }
      item.open = false
      item.selected = false
      item.disabled = false
      item.children = []
      arrObj[item.Id] = item
    })
    sub.forEach(function (item) {
      var parent = arrObj[item.parentId] || { children: [] }
      item.level = parent.level + 1
      parent.children.push(item)
    })
    return top
  }

  getNode(id: string) {
    this.itemIndex = this.nodes.findIndex(val => val.Id === id)
    this.id = id
    this.currentNode = this.nodes.find(val => val.Id === id)
    this.childOuter.emit(this.itemIndex)
  }

  createTplModal(tplTitle: TemplateRef<{}>, tplContent: TemplateRef<{}>, tplFooter: TemplateRef<{}>): void {
    this.tplModal = this.modalService.create({
      nzTitle: tplTitle,
      nzContent: tplContent,
      nzFooter: tplFooter,
      nzMaskClosable: false,
      nzClosable: false,
      nzWidth: "60%"
    });
  }

  beforeUpload = (file: any): Observable<boolean> | boolean => {
    const filepath = file.name;
    const fileend = filepath.substring(filepath.lastIndexOf('.'))
    if (fileend !== '.csv') {
      this.message.error('上传文件仅支持 CSV 格式')
      return false
    } else {
      return Observable.create(observer => {
        Papa.parse(file, {
          encoding: 'UTF8',
          complete: results => {
            let res = results.data
            if (res[res.length - 1] == '') {
              res.pop()
            }
            this.docItems.push(res.map(val => val.filter(el => el !== '')))
            this.fileList.push(file)
            observer.next('false')
            observer.complete()
            return
          }
        })
      })
    }
  }

  handleUpload() {
    let docs = Array.prototype.concat.apply([], this.docItems)
    let flag = false
    docs.map(val => {
      let [entity, ...alias] = val
      let nodes = this.currentNode.Entries.find(el => el.Value[0] === entity)
      if (nodes) {
        let data = this.flattenDeep(nodes.Synonyms)
        alias.map(val => data.push(val))
        nodes.Synonyms = Array.from(new Set(data)).map(el => [el])
        flag = true
      } else {
        this.currentNode.Entries.push({
          Value: [entity],
          Synonyms: alias.map(el => [el])
        })
      }
    })
    if (flag) {
      this.currentNode = []
      setTimeout(() => {
        this.getNode(this.id)
        this.handleCancel()
        this.updateMenus()
        this.message.success('上传成功')
      }, 0)
    } else {
      this.handleCancel()
      this.updateMenus()
      this.message.success('上传成功')
    }
  }

  handleCancel() {
    this.docItems = []
    this.fileList = []
    this.tplModal.destroy()
  }

  removeFile(index: number) {
    this.fileList.splice(index, 1)
    this.docItems.splice(index, 1)
  }

  updateMenus() {
    this.menus = this.createTree(this.nodes)
    this.menus[this.itemIndex].selected = true
  }

  deleteEntity(e: number) {
    this.currentNode.Entries.splice(e, 1)
    this.updateMenus()
  }

  alias(e: any) {
    let { type, data, index, aliasIdx } = e
    let node = this.currentNode.Entries[index].Synonyms
    let idx = node.findIndex(val => val[0] === data)
    if (type === 'add' && idx < 0) {
      node.push([data])
    } else if (type === 'change') {
      node[aliasIdx] = [data]
      node = Array.from(new Set(this.flattenDeep(node))).map(val => [val])
      this.currentNode.Entries[index].Synonyms = node
    } else if (type === 'delete') {
      node.splice(aliasIdx, 1)
    }
  }

  editEntity(e: any) {
    let { data, index } = e
    this.currentNode.Entries[index].Value = [data]
  }

  registerOnChange(fn: any): void {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any): void {
  }

  propagateChange = (_: any) => { };

  writeValue(value: any) {
    if (value) {
      this.format(value)
    }
  }

  format(data: EntitytypeData[]) {
    this.nodes = [].concat(JSON.parse(JSON.stringify(data)))
    this.menus = this.createTree(data)
    this.currentNode = this.nodes[this.itemIndex]
    this.id = this.nodes[this.itemIndex].Id
    this.menus[this.itemIndex].selected = true
  }

  addItem() {
    if (this.currentNode.Entries.length > 0 && this.currentNode.Entries[0].Value.length === 0) {
      this.message.error('同时只能添加一个实体')
      return
    }
    this.currentNode.Entries.unshift({ Value: [], Synonyms: [] })
    this.updateMenus()
  }

  // Compatible with IE
  flattenDeep(arr) {
    return arr.reduce((acc, val) => Array.isArray(val) ? acc.concat(this.flattenDeep(val)) : acc.concat(val), []);
  }

}
