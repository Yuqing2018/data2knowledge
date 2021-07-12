import { Component, OnInit, Input, forwardRef, ElementRef, ViewChild, AfterViewInit, ContentChildren, ContentChild, HostListener } from '@angular/core';
import { FormGroup, ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { DictionaryService } from 'src/app/services/dictionary.service';
import { fromEvent, of, BehaviorSubject, Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, map } from 'rxjs/operators';
import { NzSelectTopControlComponent, NzSelectComponent } from 'ng-zorro-antd';

@Component({
  selector: 'km-dictionary-id-select',
  templateUrl: './dictionary-id-select.component.html',
  styleUrls: ['./dictionary-id-select.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DictionaryIdSelectComponent),
      multi: true
    }
  ]
})
export class DictionaryIdSelectComponent implements OnInit {

  selectedValue = '';

  isLoading = false;

  idLoading = false;

  searchChange$ = new BehaviorSubject('');

  @ViewChild(NzSelectComponent) selectComponent: NzSelectComponent;   

  options = [];

  @Input() disabled: boolean;

  @Input() autoFocus = true;

  _initData: string;

  @Input()
  set initData(data: any) {
    if (data) {
      this.idLoading = true;
      this.dictionaryService.getCategoryInfo(data).subscribe(res => {
        this.options.push(res); 
        this._initData = res;
        this.idLoading = false;
      });
    }
  }

  propagateChange = (_: any) => { };

  constructor(
    private dictionaryService: DictionaryService
  ) { }

  ngOnInit() {
    const optionList$: Observable<any> = this.searchChange$
      .asObservable()
      .pipe(debounceTime(500))
      .pipe(switchMap(value => {
        return value ? this.dictionaryService.getCategorySearch(value) : of(null)
      }));

    optionList$.subscribe(res => {
      this.options = (res && res.items) || [];
      this.isLoading = false;
    });
  }

  onSearch(value: string): void {
    this.isLoading = true;
    this.searchChange$.next(value);
  }

  valueChange() {
    this.propagateChange(this.selectedValue);
  }

  writeValue(value: any) {
    this.selectedValue = value;
  }

  registerOnChange(fn: any) {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any) {
  }

  @HostListener('mouseover') 
  handleMouseoverEvent() {
    if(this.autoFocus && this.selectComponent && !this.selectComponent.open) {
      this.selectComponent.nzOpen = true;
    }
  }

}
