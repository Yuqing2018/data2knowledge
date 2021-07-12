import { Pipe, PipeTransform } from '@angular/core';
import { OptionService } from 'src/app/services/option.service';
import { Option } from 'src/app/interfaces/api.interface';

@Pipe({
  name: 'workspaceLanguage'
})
export class WorkspaceLanguagePipe implements PipeTransform {
  languageList: Option[] = [];

  constructor(
    private optionService: OptionService
  ) {
    this.optionService.getLanguage().subscribe(res => {
      this.languageList = res;  
    });
  }

  transform(value: any, args?: any): any {
    let language = this.languageList.find(w => w.value == value);
    return language ? language.displayName : value;
  }

}
