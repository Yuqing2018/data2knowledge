import { Component, OnInit, Input, forwardRef, ElementRef, ViewChild, HostListener } from '@angular/core';
import { FormGroup, ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { DictionaryService } from 'src/app/services/dictionary.service';
import { fromEvent, of, BehaviorSubject, Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, map } from 'rxjs/operators';
import { NzSelectTopControlComponent, NzSelectComponent } from 'ng-zorro-antd';

@Component({
  selector: 'km-dictionary-input-select',
  templateUrl: './dictionary-input-select.component.html',
  styleUrls: ['./dictionary-input-select.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DictionaryInputSelectComponent),
      multi: true
    }
  ]
})
export class DictionaryInputSelectComponent implements OnInit {

  selectedValue = '';

  isLoading = false;

  searchChange$ = new BehaviorSubject('');

  @Input() disabled: boolean;

  @Input() autoFocus = true;

  options = [];

  _initData: string;

  @Input()
  set initData(data: any) {
    if (data) {
      setTimeout(() => {
        this.options = [...this.options, data];
      }, 600);
    }
    this._initData = data;
  }

  propagateChange = (_: any) => { };

  @ViewChild(NzSelectComponent) selectComponent: NzSelectComponent;   

  constructor(
    private dictionaryService: DictionaryService
  ) { }

  ngOnInit() {
    const optionList$: Observable<any> = this.searchChange$
      .asObservable()
      .pipe(debounceTime(500))
      .pipe(switchMap(value => {
        return value ? this.dictionaryService.getSearchPartName(value) : of(null)
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
    this.selectedValue = value || null;
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
