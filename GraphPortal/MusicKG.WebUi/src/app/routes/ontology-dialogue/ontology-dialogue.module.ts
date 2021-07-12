import { NgModule } from '@angular/core';

import { OntologyDialogueRoutingModule } from './ontology-dialogue-routing.module';
import { OntologyDialogueMainComponent } from './main/ontology-dialogue-main.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [OntologyDialogueMainComponent],
  imports: [
    SharedModule,
    OntologyDialogueRoutingModule
  ]
})
export class OntologyDialogueModule { }
