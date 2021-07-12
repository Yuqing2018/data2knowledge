import { Component, Input, HostListener, Optional, ElementRef, HostBinding, ViewEncapsulation, Renderer2, ChangeDetectionStrategy, EventEmitter, Output } from "@angular/core";
import { AnnotationService } from './annotation.service';
import { SentenceRow } from './row';
import { SentenceData } from './interface';

export interface Word {
    text: string;
    index: number;
    opacity?: number;
    title?: string;
    color?: string;
    selected?: boolean;
    isBegin?: boolean;
    isEnd?: boolean;
}

@Component({
    selector: 'km-word',
    template: `
    <span class="word-text">{{ text }}</span>
    <span class="word-box" *ngFor="let box of boxList"
        [style.width.px]="box?.width" [style.height.px]="box?.height"
        [style.zIndex]="box?.zIndex" [style.top.px]="box?.top"
        [style.left.px]="box?.left" [style.background]="box?.background"
        [style.borderTop]="box?.borderTop" [style.borderBottom]="box?.borderBottom"
        [style.borderLeft]="box?.borderLeft" [style.borderRight]="box?.borderRight"
        [attr.title]="box.title">
    </span>
    <span class="box-selection" [ngStyle]="boxSelectionStyle"></span>
    `
})
export class WordComponent {
    box: any;
    index: number;
    text: string;
    boxSelectionStyle: any;
    boxList = [];
    entities = [];
    selections = [];
    maxLevel = 0;

    @Output() wordEvent = new EventEmitter<any>();

    @Input()
    set word(value: any) {
        this.text = value.text;
        this.index = value.index;
    }

    @Input()
    set entityList(value: any) {
        value = value || [];
        this.entities = value.filter(v => this.index >= v.begin && this.index < v.end);
        this.title = this.entities.map(v => v.title).join('&');
        this.renderBox(this.entities);
    }

    @Input() 
    set sentenceRowList(value: SentenceData) {
        if(value && value.sentenceRowList.length > 0) {     
            let row = value.sentenceRowList.find(row => row.begin && row.end && this.index >= row.begin && this.index < row.end);
            let labelMaxLevel = row.labelList.filter(label => label.wordList.find(word => word.index == this.index)).reduce((p,c) => {
                return p > c.level ? p : c.level;
            },0);
            let connectLineMaxLevel = value.connectLineList.filter(connect => this.entities.find(e => e.id == connect.from.id || e.id == connect.to.id) && connect.row == row.row).reduce((p,c) => {
                return p > c.level ? p : c.level;
            },0);
            this.marginTop = Math.max(labelMaxLevel,connectLineMaxLevel) * 50;   
        } else {
            this.marginTop = (this.maxLevel + 1) * 2;   
        }
    }

    @HostBinding('attr.title') title: any;

    @HostBinding('style.marginLeft.px') marginLeft: number;

    @HostBinding('style.marginRight.px') marginRight: number;

    @HostBinding('style.marginTop.px') marginTop: number;

    @HostBinding('style.marginBottom.px') marginBottom: number;  

    @HostBinding('style.opacity') opacity: any;

    renderBox(entities: any) {
        if (this.annotationService.mode == 'entity') {
            this.renderColorBox(entities);
        }
        if (this.annotationService.mode == 'detail' || this.annotationService.mode == 'relation') {
            this.renderBorderBox(entities);
        }
    }

    renderColorBox(entities: any) {
        let boxList = [];
        let entityList = [];
        let endTotal = entities.filter(e => e.end == (this.index + 1)).length;
        let beginTotal = entities.filter(e => e.begin == this.index).length;
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
                    id: entity.type,
                    entity: entity.id
                };
                for (let nextIndex = 0; nextIndex < entityList.length; nextIndex++) {
                    let next = entityList[nextIndex];
                    if (next.begin == this.index && index <= nextIndex) {
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
        this.marginLeft = beginTotal * 2;  
        this.marginTop = entityList[0] ? ((entityList[0].level + 1) * 2) : 2;
        this.marginBottom = entityList[0] ? ((entityList[0].level + 1) * 2) : 2;  
    }

    renderBorderBox(entities: any) {
        let boxList = [];
        let entityList = [];
        let endTotal = entities.filter(e => e.end == (this.index + 1)).length;
        let beginTotal = entities.filter(e => e.begin == this.index).length;
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
                    id: entity.type,
                    entity: entity.id,
                    borderTop: '2px dotted #ccc',
                    borderBottom: '2px dotted #ccc',
                    borderLeft: this.index == entity.begin ? '2px dotted #ccc' : null,
                    borderRight: this.index == (entity.end - 1) ? '2px dotted #ccc' : null
                };
                for (let nextIndex = 0; nextIndex < entityList.length; nextIndex++) {
                    let next = entityList[nextIndex];
                    if (next.begin == this.index && index <= nextIndex) {
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
        this.marginLeft = beginTotal * 4;
        this.marginBottom = 2;  
    }

    @Input()
    set selectionList(value: any) {
        this.selections = value;
        this.renderSelection(this.selections);
    }

    constructor(
        public annotationService: AnnotationService,
        public hostEl: ElementRef<HTMLElement>,
    ) { }

    ngOnInit() {
        this.annotationService.sentenceShowEntity.subscribe(value => {
            for (let box of this.boxList) {
                box.opacity = (!value || box.id == value.id) ? 1 : 0.3
            }
            this.opacity = (!value || this.boxList.some(b => b.opacity == 1)) ? 1 : 0.3;
        });
    }

    ngAfterViewInit() {
        this.refresh();
        this.annotationService.modeChange.subscribe(value => {
            this.renderBox(this.entities);
        });
    }

    refresh() {
        Promise.resolve().then(() => {
            this.renderBox(this.entities);
            this.renderSelection(this.selections);
        })
    }

    renderSelection(selectionList: any) {
        if (this.annotationService.mode == 'entity') {
            this.rederEntitySelction(selectionList);
        }
        if (this.annotationService.mode == 'detail') {
            this.renderBorderSelction(selectionList);
        }
    }

    renderBorderSelction(selectionList: any) {
        let style: any;
        let rect = this.hostEl.nativeElement.getBoundingClientRect();
        for (let selection of selectionList) {
            if (this.index >= selection.begin && this.index < selection.end) {
                if (selection.entity) {
                    style = {
                        height: rect.height + 'px',
                        width: rect.width,
                        borderLeft: this.index == selection.begin ? `2px solid #00B2EF` : null,
                        borderRight: this.index == (selection.end - 1) ? `2px solid #00B2EF` : null,
                        borderTop: `2px solid #00B2EF`,
                        borderBottom: `2px solid #00B2EF`,
                        top: `0px`,
                    }
                    if (this.index == selection.begin || this.index == (selection.end - 1)) {
                        let box = this.boxList.find(b => b.entity == selection.entity);
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
                        borderLeft: this.index == selection.begin ? `2px solid #00B2EF` : null,
                        borderRight: this.index == (selection.end - 1) ? `2px solid #00B2EF` : null,
                        borderTop: `2px solid #00B2EF`,
                        borderBottom: `2px solid #00B2EF`
                    }
                    if (this.index != selection.begin || this.index != (selection.end - 1)) {
                        style.width += (this.marginLeft + this.marginRight);
                        style.left = this.marginLeft * -1 + 'px';       
                    }
                    style.width += 'px';
                }
            }
        }
        this.boxSelectionStyle = style;
    }

    rederEntitySelction(selectionList: any) {
        let style: any;
        let rect = this.hostEl.nativeElement.getBoundingClientRect();
        for (let selection of selectionList) {
            if (this.index >= selection.begin && this.index < selection.end) {
                if (selection.entity && this.annotationService.mode == 'entity') {
                    style = {
                        height: rect.height + selection.level * 4 + 'px',
                        width: rect.width,
                        borderLeft: this.index == selection.begin ? `2px solid #00B2EF` : null,
                        borderRight: this.index == (selection.end - 1) ? `2px solid #00B2EF` : null,
                        borderTop: `2px solid #00B2EF`,
                        borderBottom: `2px solid #00B2EF`,
                        top: `${selection.level * -2}px`,
                    }
                } else {
                    style = {
                        height: rect.height + 'px',
                        width: rect.width,
                        borderLeft: this.index == selection.begin ? `2px solid #00B2EF` : null,
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
        }
        this.boxSelectionStyle = style;
    }

    @HostListener('mousedown', ['$event'])
    mousedown(event: MouseEvent) {
        if (this.annotationService.mode !== 'relation') {
            this.wordEvent.emit('mousedown');
        }
        event.stopPropagation();
    }

    @HostListener('mousemove', ['$event'])
    mousemove(event: MouseEvent) {
        if (this.annotationService.mode !== 'relation') {
            this.wordEvent.emit('mousemove');
            event.stopPropagation();
        }
    }

    @HostListener('mouseup', ['$event'])
    mouseup(event: MouseEvent) {
        if (this.annotationService.mode !== 'relation') {
            this.wordEvent.emit('mouseup');
            event.stopPropagation();
        }
    }

}