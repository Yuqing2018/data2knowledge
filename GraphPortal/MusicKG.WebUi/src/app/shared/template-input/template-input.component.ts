import { Component, OnInit, forwardRef, Renderer2, ViewChildren, QueryList } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { DropService } from 'src/app/services/drop.service';

@Component({
  selector: 'km-template-input',
  templateUrl: './template-input.component.html',
  styleUrls: ['./template-input.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TemplateInputComponent),
      multi: true
    }
  ],
})
export class TemplateInputComponent implements OnInit {
  parts: any;

  @ViewChildren('partInput') partInputList: QueryList<any>;
  @ViewChildren('mirror') mirrorList: QueryList<any>;

  propagateChange = (_: any) => { };

  writeValue(value: any) {
    if (value) {
      this.parts = value;
    }
  }

  ngDoCheck() {
    this.updateWidth();
  }

  updateWidth() {
    if (this.partInputList && this.mirrorList) {
      this.partInputList.forEach((input, index) => {
        let mirror = this.mirrorList.toArray()[index].nativeElement;
        mirror.textContent = input.nativeElement.value;
        this.render.setStyle(
          input.nativeElement,
          'width',
          `${mirror.clientWidth + 2}px`
        );
      });
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

  emitValue() {
    this.propagateChange(this.parts);
  }

  changeValue(input: any, part: any, mirror: any) {
    part.Text = input.value;
  }

  onDragover(event: MouseEvent, el: any) {
    el.focus();
    event.preventDefault();
  }

  onDragleave(event: MouseEvent, el: any) {
    event.preventDefault();
    el.blur();
  }

  onDrop(event: any, el: any, part: any) {
    let entity = this.dropService.dropEntity;
    if (entity) {
      this.parts.splice(this.parts.indexOf(part), 1, { Text: part.Text.substring(0, el.selectionEnd) },
        { Text: '@' + entity.EntityType, EntityType: entity.EntityType, Alias: entity.EntityType.toLowerCase(), UserDefined: true, Color: entity.Color },
        { Text: part.Text.substring(el.selectionEnd, part.Text.length) });
      this.dropService.dropEntity = null;
    }
  }

}
