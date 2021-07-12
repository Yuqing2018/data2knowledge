import { Component, HostBinding, Input, HostListener } from "@angular/core";

@Component({
    selector: 'km-entity-label',
    template: `
     {{ text }}
    `
})
export class EntityAnnoationLabel {
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

    @HostBinding('style.opacity') opacity: number;
    
    @HostBinding('class.active') 
    _active = false;

    @Input()
    set label(value: any) {
        this.id = value.id; 
        this.text = value.title;
        this.isEnd = value.isEnd;
        this.isBegin = value.isStart;
        this.top = value.top;
        this.left = value.left;
        this.height = value.height;
        this.width = value.width;
        this.color = value.color;
    }

    @Input() 
    set active(value: any) {
        this._active = value && value == this.id;
    }

    @Input()
    set highlightEntity(value: any) {
        value = value || [];
        if(value && value.length > 0) {
            this.opacity = value.find(v => v.id == this.id) ? 1 : 0.3;
        }else {
            this.opacity = 1;
        }
    }

    constructor(
    ) { }

    @HostListener('mouseup',['$event'])
    mouseup(event: MouseEvent) {
        event.stopPropagation();
    }
  

}