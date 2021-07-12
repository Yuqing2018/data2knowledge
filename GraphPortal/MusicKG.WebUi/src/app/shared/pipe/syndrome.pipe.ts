import { Pipe, PipeTransform } from '@angular/core';
import { DictionaryService } from 'src/app/services/dictionary.service';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';

@Pipe({
  name: 'syndrome'
})
export class SyndromePipe implements PipeTransform {

  constructor(
    private dictionaryService: DictionaryService
  ) {
  }

  transform(value: any, args?: any): any {
    return !!value ? this.dictionaryService.getCategoryInfo(value).pipe(map(v => v.name)) : of('');
  }

}
