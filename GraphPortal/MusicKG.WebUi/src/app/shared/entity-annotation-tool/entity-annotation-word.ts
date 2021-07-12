import { Component, Input, HostListener, Optional, ElementRef, HostBinding, ViewEncapsulation, Renderer2, ChangeDetectionStrategy, EventEmitter, Output } from "@angular/core";

@Component({
    selector: 'km-entity-word',
    template: `
    <span class="word-text">{{ text }}</span>
    <span class="word-box" *ngFor="let box of boxList"
        [style.width.px]="box?.width" [style.height.px]="box?.height"
        [style.zIndex]="box?.zIndex" [style.top.px]="box?.top"
        [style.left.px]="box?.left" [style.background]="box?.background"
        [style.borderTop]="box?.borderTop" [style.borderBottom]="box?.borderBottom"
        [style.borderLeft]="box?.borderLeft" [style.borderRight]="box?.borderRight"
        [style.opacity]="box?.opacity"
        [attr.title]="box?.title">   
    </span>
    <span class="box-selection" [ngStyle]="boxSelectionStyle"></span>
    `,
})
export class EntityWordComponent {
    box: any;
    index: number;
    text: string;
    boxSelectionStyle: any;
    boxList = [];
    entities = [];
    textSelection = [];
    wordMode = 'entity';
    maxLevel = 0;
    isCreated = false;
    highlightEntityList = [];

    @Output() wordEvent = new EventEmitter<any>();

    @Input()
    set word(value: any) {
        this.text = value.text;
        this.index = value.index;
    }

    @Input()
    set mode(mode: string) {
        this.wordMode = mode || 'entity'
    }

    @Input()
    set entityList(value: any) {
        value = value || [];
        this.entities = value.filter(v => this.index >= v.start && this.index < v.end);
        this.title = this.entities.map(v => v.title).join('&');
    }

    @Input()
    set highlightEntity(value: any) {
        this.highlightEntityList = value;
    }

    @Input()
    set selection(value: any) {
        this.textSelection = value;
    }

    @HostBinding('attr.title') title: any;

    @HostBinding('style.marginLeft.px') marginLeft: number;

    @HostBinding('style.marginRight.px') marginRight: number;

    @HostBinding('style.marginTop.px') marginTop: number;

    @HostBinding('style.marginBottom.px') marginBottom: number;

    @HostBinding('style.opacity') opacity: any;

    renderWord() {
        Promise.resolve().then(() => {
            if (this.wordMode == 'entity') {
                this.renderColorBox(this.entities);
                this.rederEntitySelction(this.textSelection);
            } else {
                this.renderBorderBox(this.entities);
                this.renderBorderSelction(this.textSelection);
            }
            this.renderHighligh();
        })
    }

    renderHighligh() {
        if (this.highlightEntityList && this.highlightEntityList.length > 0) {
            this.opacity = this.boxList.find(b => this.highlightEntityList.find(v => v.id == b.id)) ? 1 : 0.3;
            this.boxList = this.boxList.map(b => {
                if (this.highlightEntityList.find(v => v.id == b.id)) {
                    b.opacity = 1;
                } else {
                    b.opacity = 0.3;
                }
                return b;
            })
        } else {
            this.opacity = 1;
            this.boxList = this.boxList.map(b => {
                b.opacity = 1;
                return b;
            })
        }
    }

    renderColorBox(entities: any) {
        let boxList = [];
        let entityList = [];
        let endTotal = entities.filter(e => e.end == (this.index + 1)).length;
        let startTotal = entities.filter(e => e.start == this.index).length;
        if (entities && entities.length > 0) {
            entityList = entities.sort((a, b) => b.level - a.level);
            this.maxLevel = entityList[0].level;
            let rect = this.hostEl.nativeElement.getBoundingClientRect();
            for (let index = 0; index < entityList.length; index++) {
                let entity = entityList[index];
                let box: any = {
                    background: entity.color,
                    width: rect.width,
                    height: rect.height + entity.level * 4,
                    zIndex: 80,
                    top: `${entity.level * -2}`,
                    left: 0,
                    id: entity.id,
                    entity: entity.entityType,
                };
                for (let nextIndex = 0; nextIndex < entityList.length; nextIndex++) {
                    let next = entityList[nextIndex];
                    if (next.start == this.index && index <= nextIndex) {
                        box.left -= 2;
                        box.width += 2;
                    }
                    if (next.end == (this.index + 1) && index <= nextIndex) {
                        box.width += 2;
                    }
                }
                boxList.push(box);
            }
        }
        this.boxList = boxList;
        this.marginRight = endTotal * 2;
        this.marginLeft = startTotal * 2;
        this.marginTop = entityList[0] ? ((entityList[0].level + 1) * 2) : 2;
        this.marginBottom = entityList[0] ? ((entityList[0].level + 1) * 2) : 2;
        this.marginTop = (this.maxLevel + 1) * 2;
    }

    renderBorderBox(entities: any) {
        let boxList = [];
        let entityList = [];
        let endTotal = entities.filter(e => e.end == (this.index + 1)).length;
        let startTotal = entities.filter(e => e.start == this.index).length;
        let rect = this.hostEl.nativeElement.getBoundingClientRect();
        if (entities && entities.length > 0) {
            entityList = entities.sort((a, b) => b.level - a.level);
            this.maxLevel = entityList[0].level;
            for (let index = 0; index < entityList.length; index++) {
                let entity = entityList[index];
                let box: any = {
                    width: rect.width,
                    height: rect.height,
                    zIndex: 80,
                    top: 0,
                    left: 0,
                    id: entity.id,
                    entity: entity.entityType,
                    borderTop: '2px dotted #ccc',
                    borderBottom: '2px dotted #ccc',
                    borderLeft: this.index == entity.start ? '2px dotted #ccc' : null,
                    borderRight: this.index == (entity.end - 1) ? '2px dotted #ccc' : null
                };
                for (let nextIndex = 0; nextIndex < entityList.length; nextIndex++) {
                    let next = entityList[nextIndex];
                    if (next.start == this.index && index <= nextIndex) {
                        box.left -= 3;
                        box.width += 4;
                    }
                    if (next.end == (this.index + 1) && index <= nextIndex) {
                        box.width += 3;
                    }
                }
                boxList.push(box);
            }
        } else {
            this.maxLevel = 0;
        }
        this.boxList = boxList;
        this.marginRight = endTotal * 4;
        this.marginLeft = startTotal * 4;
        this.marginBottom = 2;
        this.marginTop = this.maxLevel * 50;
    }

    rederEntitySelction(selection: any) {
        let style: any;
        let rect = this.hostEl.nativeElement.getBoundingClientRect();
        if (selection && this.index >= selection.start && this.index < selection.end) {
            if (selection.entity) {
                style = {
                    height: rect.height + selection.level * 4 + 'px',
                    width: rect.width,
                    borderLeft: this.index == selection.start ? `2px solid #00B2EF` : null,
                    borderRight: this.index == (selection.end - 1) ? `2px solid #00B2EF` : null,
                    borderTop: `2px solid #00B2EF`,
                    borderBottom: `2px solid #00B2EF`,
                    top: `${selection.level * -2}px`,
                }
            } else {
                style = {
                    height: rect.height + 'px',
                    width: rect.width,
                    borderLeft: this.index == selection.start ? `2px solid #00B2EF` : null,
                    borderRight: this.index == (selection.end - 1) ? `2px solid #00B2EF` : null,
                    borderTop: `2px solid #00B2EF`,
                    borderBottom: `2px solid #00B2EF`
                }
            }
            let marginLeft = this.marginLeft;
            let marginRight = this.marginRight;
            if (marginLeft > 0) {
                style.width += marginLeft;
                style.left = marginLeft * -1 + 'px'
            }
            if (marginRight > 0) {
                style.width += marginRight;
            }
            style.width += 'px';
        }
        this.boxSelectionStyle = style;
    }

    renderBorderSelction(selection: any) {
        let style: any;
        let rect = this.hostEl.nativeElement.getBoundingClientRect();
        if (selection && this.index >= selection.start && this.index < selection.end) {
            if (selection.entity) {
                style = {
                    height: rect.height + 'px',
                    width: rect.width,
                    borderLeft: this.index == selection.start ? `2px solid #00B2EF` : null,
                    borderRight: this.index == (selection.end - 1) ? `2px solid #00B2EF` : null,
                    borderTop: `2px solid #00B2EF`,
                    borderBottom: `2px solid #00B2EF`,
                    top: `0px`,
                }
                if (this.index == selection.start || this.index == (selection.end - 1)) {
                    let box = this.boxList.find(b => b.id == selection.id);
                    style.width = box.width + 'px';
                    style.left = box.left + 'px';
                } else {
                    let marginLeft = this.marginLeft;
                    let marginRight = this.marginRight;
                    if (marginLeft > 0) {
                        style.width += marginLeft;
                        style.left = marginLeft * -1 + 'px'
                    }
                    if (marginRight > 0) {
                        style.width += marginRight;
                    }
                    style.width += 'px';
                }
            } else {
                style = {
                    height: rect.height + 'px',
                    width: rect.width,
                    borderLeft: this.index == selection.start ? `2px solid #00B2EF` : null,
                    borderRight: this.index == (selection.end - 1) ? `2px solid #00B2EF` : null,
                    borderTop: `2px solid #00B2EF`,
                    borderBottom: `2px solid #00B2EF`
                }
                if (this.index != selection.start || this.index != (selection.end - 1)) {
                    style.width += (this.marginLeft + this.marginRight);
                    style.left = this.marginLeft * -1 + 'px';
                }
                style.width += 'px';
            }
        }
        this.boxSelectionStyle = style;
    }

    constructor(
        public hostEl: ElementRef<HTMLElement>
    ) { }

    ngOnInit() {
    }

    ngAfterViewInit() {
        this.isCreated = true;
        this.renderWord();
    }

    ngOnChanges() {
        if (this.isCreated) {
            this.renderWord();
        }
    }

    @HostListener('mousedown', ['$event'])
    mousedown(event: MouseEvent) {
        this.wordEvent.emit('mousedown');
        event.stopPropagation();
    }

    @HostListener('mousemove', ['$event'])
    mousemove(event: MouseEvent) {
        this.wordEvent.emit('mousemove');
        event.stopPropagation();
    }

    @HostListener('mouseup', ['$event'])
    mouseup(event: MouseEvent) {
        this.wordEvent.emit('mouseup');
        event.stopPropagation();
    }

}