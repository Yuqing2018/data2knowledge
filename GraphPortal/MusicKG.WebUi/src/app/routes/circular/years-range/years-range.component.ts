import { Component, OnInit, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { differenceInCalendarYears, addYears, getYear } from 'date-fns';

@Component({
  selector: 'km-years-range',
  templateUrl: './years-range.component.html',
  styleUrls: ['./years-range.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => YearsRangeComponent),
      multi: true
    }
  ]
})
export class YearsRangeComponent implements OnInit {

  startYear = '2015';

  endYear = '2021';

  propagateChange = (_: any) => { };

  disableStartdDate = (current: Date) => {
    return differenceInCalendarYears(
      new Date(this.endYear),
      current
    ) <= -1;
  }

  disabledEndDate = (current: Date) => {
    return differenceInCalendarYears(
      current,
      new Date(this.startYear)
    ) <= -1;
  }

  constructor(
  ) { }

  ngOnInit() {
  }

  valueChange() {
    const value = Array.from({
      length: differenceInCalendarYears(
        new Date(this.endYear),
        new Date(this.startYear)
      )+1 || 1
    }).map((v, i) => getYear(addYears(new Date(this.startYear),i)) + '');
    this.propagateChange(value);
  }

  writeValue(value: string[]) {
    if (value && value.length >= 1) {
      this.startYear = value[0];
      this.endYear = value[value.length-1]
    }
  }

  registerOnChange(fn: any) {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any) {
  }

}
