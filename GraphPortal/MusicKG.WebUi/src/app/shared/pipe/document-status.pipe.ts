import { Pipe, PipeTransform } from '@angular/core';
import { Option } from 'src/app/interfaces/api.interface';
import { OptionService } from 'src/app/services/option.service';

@Pipe({
  name: 'documentStatus'
})
export class DocumentStatusPipe implements PipeTransform {
  docStatusList: Option[] = [];

  constructor(
    private optionService: OptionService
  ) {
    this.optionService.getDocumentStatus().subscribe(res => {
      this.docStatusList = res;   
    });
  }

  transform(value: any, args?: any): any {
    let docStatus = this.docStatusList.find(w => w.value == value);
    return docStatus ? docStatus.displayName : value;   
  }

}
