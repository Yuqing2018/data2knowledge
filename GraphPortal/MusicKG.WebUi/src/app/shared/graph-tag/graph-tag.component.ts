import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core'

@Component({
  selector: 'km-graph-tag',
  templateUrl: './graph-tag.component.html',
  styleUrls: ['./graph-tag.component.less']
})
export class GraphTagComponent implements OnInit {

  @Input() tagList: any
  @Output() private tag = new EventEmitter()

  constructor() { }

  ngOnInit() {
  }

  selectTag(tagId) {
    this.tag.emit(tagId)
  }

}
