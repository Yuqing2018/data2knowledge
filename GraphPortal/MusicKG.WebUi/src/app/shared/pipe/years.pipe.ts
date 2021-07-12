import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'years'
})
export class YearsPipe implements PipeTransform {

  transform(value: any, args?: any): any {
    if(value && Array.isArray(value)) {
      return `${value[0]}-${value[value.length - 1]}`
    }
    return value;
  }

}
