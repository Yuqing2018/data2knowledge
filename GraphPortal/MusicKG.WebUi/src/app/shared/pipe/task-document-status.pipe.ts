import { Pipe, PipeTransform } from '@angular/core';
import { OptionService } from 'src/app/services/option.service';
import { Option } from 'src/app/interfaces/api.interface';

@Pipe({
  name: 'taskDocumentStatus'
})
export class TaskDocumentStatusPipe implements PipeTransform {
  docStatusList: Option[] = [];

  constructor(
    private optionService: OptionService
  ) {
    this.optionService.getTakDocumentStatus().subscribe(res => {
      this.docStatusList = res;   
    });
  }

  transform(value: any, args?: any): any {
    let docStatus = this.docStatusList.find(w => w.value == value);
    return docStatus ? docStatus.displayName : value;   
  }

}
