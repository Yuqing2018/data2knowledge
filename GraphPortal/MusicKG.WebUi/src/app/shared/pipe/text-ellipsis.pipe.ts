import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'textEllipsis'
})
export class TextEllipsisPipe implements PipeTransform {

  transform(value: string = '', args: any = []): any {
    let text = value;
    let ellipsisLength = args[0] || 13; 
    if (value && value.length > ellipsisLength) {  
      text = value.substring(0, ellipsisLength) + '...';
    }
    return text;
  }

}
