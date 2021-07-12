import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Task, TaskInfo } from 'src/app/interfaces/task.interface';

@Component({
  selector: 'km-task-update-form',
  templateUrl: './task-update-form.component.html',
  styleUrls: ['./task-update-form.component.less']
})
export class TaskUpdateFormComponent implements OnInit {
  taskForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder
  ) { }

  @Input()
  set task(task: TaskInfo) {
    this.taskForm = this.formBuilder.group({
      name: [task.name, [Validators.required, Validators.maxLength(20)]],
      expectedDueAt: [task.expectedDueAt, Validators.required],
    });
  }

  ngOnInit() {
  }

  hasError(controlName: string, error: string) {
    let formControl = this.taskForm.get(controlName);
    return formControl.dirty && formControl.errors && formControl.errors[error];
  }

}
