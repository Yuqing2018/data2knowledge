import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'statisticIsmanager'
})
export class StatisticIsmanagerPipe implements PipeTransform {

  transform(value: any, args?: any): any {
    return value.filter(val => val.isManager === args).map(val => val.annotatorName)
  }

}
