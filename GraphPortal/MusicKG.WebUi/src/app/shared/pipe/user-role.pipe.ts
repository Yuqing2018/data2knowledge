import { Pipe, PipeTransform } from '@angular/core';
import { OptionService } from 'src/app/services/option.service';
import { Option } from 'src/app/interfaces/api.interface';

@Pipe({
  name: 'userRole'
})
export class UserRolePipe implements PipeTransform {
  userRoleList: Option[] = [];

  constructor(
    private optionService: OptionService
  ) {
    this.optionService.getUserRole().subscribe(res => {
      this.userRoleList = res;
    });
  }

  transform(value: any, args?: any): any {
    let userRole = this.userRoleList.find(w => w.value == value);
    return userRole ? userRole.displayName : value;
  }

}
