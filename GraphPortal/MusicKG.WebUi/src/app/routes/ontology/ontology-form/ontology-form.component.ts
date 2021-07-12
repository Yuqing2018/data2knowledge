import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'km-ontology-form',
  templateUrl: './ontology-form.component.html',
  styleUrls: ['./ontology-form.component.less']
})
export class OntologyFormComponent implements OnInit {
  ontologyForm: FormGroup;
  @Input() labelType: string

  constructor(
    private formBuilder: FormBuilder
  ) {
    
   }

  ngOnInit() {
    this.ontologyForm = this.formBuilder.group({
      name: ['', [Validators.required,Validators.maxLength(100)]],
      description: ['',Validators.maxLength(128)],
    });
  }
 
  hasError(controlName: string, error: string) {
    let formControl = this.ontologyForm.get(controlName);
    return formControl.dirty && formControl.errors && formControl.errors[error];
  }
}
