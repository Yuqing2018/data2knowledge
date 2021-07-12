import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { Observable } from 'rxjs';

@Component({
  selector: 'km-annotation-toolbar',
  templateUrl: './annotation-toolbar.component.html',
  styleUrls: ['./annotation-toolbar.component.less']
})
export class AnnotationToolbarComponent implements OnInit {
  @Input() pageIndex: number = 0;
  
  @Input() itemIndex: number = 0; 

  @Input() pageTotal: number = 0;
  @Input() itemTotal: number = 0;
  @Input() title: string;
  @Input() disabled: boolean = true;

  @Input() hasItem = true;

  @Output() pageIndexChange = new EventEmitter<number>();
  @Output() itemIndexChange = new EventEmitter<number>();
  @Output() save = new EventEmitter<string>();

  get notHasPagePrevious() {
    return this.pageIndex == 0;
  }

  get notHasPageNext() {
    return this.pageTotal && this.pageIndex == (this.pageTotal - 1);
  }

  get notHasItemPrevious() {
    return this.itemIndex == 0;
  }

  get notHasItemNext() {
    return this.itemIndex == (this.itemTotal - 1);
  }

  constructor() { }

  ngOnInit() {
  }

  pagePrevious() {
    if (this.pageIndex > 0) {
      this.pageIndexChange.emit(this.pageIndex - 1);
    }
  }

  pageNext() {
    if (this.pageIndex < (this.pageTotal - 1)) {
      this.pageIndexChange.emit(this.pageIndex + 1);
    }
  }

  itemPrevious() {
    if (this.itemIndex > 0) {
      this.itemIndexChange.emit(this.itemIndex - 1);
    }
  }

  itemNext() {
    if (this.itemIndex < (this.itemTotal - 1)) {
      this.itemIndexChange.emit(this.itemIndex + 1);
    }
  }

}
