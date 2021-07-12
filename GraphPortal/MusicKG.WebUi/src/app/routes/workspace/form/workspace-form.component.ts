import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Workspace } from 'src/app/interfaces/workspace.interface';
import { OptionService } from 'src/app/services/option.service';
import { Option } from 'src/app/interfaces/api.interface';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'km-workspace-form',
  templateUrl: './workspace-form.component.html',
  styleUrls: ['./workspace-form.component.less']
})
export class WorkspaceFormComponent implements OnInit {
  workspaceForm: FormGroup;
  typeList: Option[] = [];
  languageList: Option[] = [];

  @Input()
  set workspace(workspace: Workspace) {
    this.workspaceForm = this.formBuilder.group({
      id: [workspace.id],
      name: [workspace.name, [Validators.required, Validators.maxLength(20)]],
      // language: [workspace.language, Validators.required],
      type: [workspace.type, Validators.required],
      description: [workspace.description, Validators.maxLength(128)],
    });
  }

  constructor(
    private formBuilder: FormBuilder,
    private optionService: OptionService
  ) { }

  ngOnInit() {
    forkJoin(
      this.optionService.getLanguage(),
      this.optionService.getWorkspaceType()
    ).subscribe(([languageList, typeList]) => {
      this.languageList = languageList;
      this.typeList = typeList;
    });
  }

  hasError(controlName: string, error: string) {
    let formControl = this.workspaceForm.get(controlName);
    return formControl.dirty && formControl.errors && formControl.errors[error];
  }


}
