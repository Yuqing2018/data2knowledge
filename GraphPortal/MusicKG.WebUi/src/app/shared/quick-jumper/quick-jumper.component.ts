import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

@Component({
  selector: 'km-quick-jumper',
  templateUrl: './quick-jumper.component.html',
  styleUrls: ['./quick-jumper.component.less']
})
export class QuickJumperComponent implements OnInit {

  @Output() indexChange = new EventEmitter();
  @Input() item: boolean = true

  constructor() { }

  ngOnInit() {
  }

  handleKeyDown(input: HTMLInputElement) {
    const itemIndex = +input.value;
    if (typeof itemIndex === 'number') {
      this.indexChange.emit(itemIndex - 1);
    }
    input.value = '';
  }

}
