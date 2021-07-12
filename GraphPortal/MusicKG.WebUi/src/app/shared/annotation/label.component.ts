import { Component, HostBinding, Input, HostListener } from "@angular/core";

@Component({
    selector: 'km-label',
    template: `
     {{ text }}
    `
})
export class AnnoationLabel {
    @HostBinding('attr.title') 
    text = '';
    id: any;

    @HostBinding('class.isBegin') isBegin = false;

    @HostBinding('class.isEnd') isEnd = false;

    @HostBinding('style.width.px') width: number;

    @HostBinding('style.height.px') height: number;

    @HostBinding('style.left.px') left: number;

    @HostBinding('style.top.px') top: number;

    @HostBinding('style.background') color: string;
    
    @HostBinding('class.active') 
    _active = false;

    @Input()
    set label(value: any) {
        this.id = value.id; 
        this.text = value.title;
        this.isEnd = value.isEnd;
        this.isBegin = value.isBegin;
        this.top = value.top;
        this.left = value.left;
        this.height = value.height;
        this.width = value.width;
        this.color = value.color;
    }

    @Input() 
    set active(value: any) {
        this._active = value && value.indexOf(this.id) !== -1;
    }

    constructor(
    ) { }

    @HostListener('mouseup',['$event'])
    mouseup(event: MouseEvent) {
        event.stopPropagation();
    }
  

}