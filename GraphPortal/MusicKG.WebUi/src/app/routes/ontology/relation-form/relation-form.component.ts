import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators,FormControl } from '@angular/forms';
import { OntologyEntityInfo } from 'src/app/interfaces/ontology.interface';

@Component({
  selector: 'km-relation-form',
  templateUrl: './relation-form.component.html',
  styleUrls: ['./relation-form.component.less']
})
export class RelationFormComponent implements OnInit {
  @Input() data: any;//编辑时传来的数据
  @Input() entityData: OntologyEntityInfo[];  //打开弹框时传来的数据 实体数据;
  disabled:boolean = false;

  ontologyForm: FormGroup;
  // attrTypeList:Array<any>=[{label:'lang',value:'lang'},{label:'String',value:'string'},{label:'Integer',value:'integer'},{label:'Date Time',value:'date_time'},{label:'Boolean',value:'boolean'}];
  // attrList:Array<any>=[{label:'AGE',value:'age'},{label:'DATE',value:'date'},{label:'PERSON',value:'person'}];

  constructor(
    private formBuilder: FormBuilder
  ) {   }

  checkName= (control: FormControl): { [ s: string ]: boolean } => {
    console.log(control.value)
    if (!control.value) {
      return { required: true };
    } else if (!/^[a-zA-Z0-9_]+$/.test(control.value)) {
      return { checkName: true , error: true };
    }
  };

  ngOnInit() {
    if(this.data){
      this.disabled = true;
    }
    this.ontologyForm = this.formBuilder.group({
      name: [this.data ? this.data.name : '',  [ this.checkName ] ],
      firstEntityId: [this.data ? this.data.firstEntityId : '', Validators.required],
      secondEntityId: [this.data ? this.data.secondEntityId : '', Validators.required],
      description: [this.data ? this.data.description : ''],
    });
  }
  hasError(control: string) {
    return this.ontologyForm.get(control).dirty && this.ontologyForm.get(control).errors;
  }
  

}
