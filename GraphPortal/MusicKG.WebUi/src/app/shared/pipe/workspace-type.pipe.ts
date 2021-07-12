import { Pipe, PipeTransform } from '@angular/core';
import { OptionService } from 'src/app/services/option.service';
import { Option } from 'src/app/interfaces/api.interface';

@Pipe({
  name: 'workspaceType'
})
export class WorkspaceTypePipe implements PipeTransform {
  workspaceTypeList: Option[] = [];

  constructor(
    private optionService: OptionService
  ) {
    this.optionService.getWorkspaceType().subscribe(res => {
      this.workspaceTypeList = res;   
    });
  }

  transform(value: any, args?: any): any {
    let workspaceType = this.workspaceTypeList.find(w => w.value == value);
    return workspaceType ? workspaceType.displayName : value;
  }

}
