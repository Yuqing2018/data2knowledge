import { Component, Input, ElementRef, HostBinding, Output, EventEmitter } from "@angular/core";
import { AnnotationService } from './annotation.service';
import { SentenceRow } from './row';
import { SentenceData } from './interface';

@Component({
    selector: 'km-placeholder',
    template: `
      <span>
        {{ text }}
      </span>
      <span *ngIf="annotationService.mode =='entity' || annotationService.mode =='detail'" (click)="remove()" class="close">X</span>
    `,
})
export class PlaceholderComponent {
    text = '';
    id: string;
    type: string;
    level: number;

    @HostBinding('style.background') color: string;

    @HostBinding('style.opacity') opacity: number = 1;

    @HostBinding('style.marginTop.px') marginTop: number;

    @Output() removePlaceholder = new EventEmitter<any>();

    @Input()
    set placeholder(value: any) {
        if (value) {
            this.id = value.id;
            this.type = value.type;
            this.text = value.text;
            this.color = value.color;
        }
    }

    @Input()
    set sentenceRowList(value: SentenceData) {
        if (value && value.sentenceRowList.length > 0) {
            let row = value.sentenceRowList.find(row => !!row.placeholderList.find(p => p.id == this.id));
            let connectLineMaxLevel = value.connectLineList.filter(connect => connect.row == row.row && (connect.from.id == this.id || connect.to.id == this.id)).reduce((p, c) => {
                return p > c.level ? p : c.level;
            }, 0);
            this.marginTop = connectLineMaxLevel * 50;
        } else {
            this.marginTop = 0;
        }
    }

    constructor(
        public annotationService: AnnotationService,
        public hostEl: ElementRef<HTMLElement>
    ) { }

    ngOnInit() {
        this.annotationService.sentenceShowEntity.subscribe(value => {
            this.opacity = (!value || this.type == value.id) ? 1 : 0.3;
        });
    }

    remove() {
        this.removePlaceholder.emit(true);
    }
}


@Component({
    selector: 'km-add-placeholder',
    template: `
    <form nz-form>
        <nz-form-item style="margin-bottom: 0px;">
            <nz-form-label [nzSpan]="6">实体</nz-form-label>
            <nz-form-control [nzSpan]="14">
                <nz-select nzPlaceHolder="请选择实体类型" [(ngModel)]="placeholder" name="placeholder">
                    <nz-option nzLabel="歌手" nzValue="1"></nz-option>
                    <nz-option nzLabel="歌曲" nzValue="2"></nz-option>
                    <nz-option nzLabel="专辑" nzValue="3"></nz-option>
                </nz-select>
            </nz-form-control>
        </nz-form-item>
    </form>
    `
})
export class AddPlaceholderComponent {
    placeholder: any;
}