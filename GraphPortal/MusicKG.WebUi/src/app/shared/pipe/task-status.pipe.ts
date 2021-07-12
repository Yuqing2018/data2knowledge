import { Pipe, PipeTransform } from '@angular/core';
import { Option } from 'src/app/interfaces/api.interface';
import { OptionService } from 'src/app/services/option.service';

@Pipe({
  name: 'taskStatus'
})
export class TaskStatusPipe implements PipeTransform {
  taskStatusList: Option[] = [];

  constructor(
    private optionService: OptionService
  ) {
    this.optionService.getTaskStatus().subscribe(res => {
      this.taskStatusList = res;   
    }); 
  }

  transform(value: any, args?: any): any {
    let taskStatus = this.taskStatusList.find(w => w.value == value);
    return taskStatus ? taskStatus.displayName : value;   
  }

}
