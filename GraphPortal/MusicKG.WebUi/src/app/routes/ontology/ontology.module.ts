import { NgModule } from '@angular/core';

import { OntologyRoutingModule } from './ontology-routing.module';
import { OntologyMainComponent } from './main/ontology-main.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { OntologyFormComponent } from './ontology-form/ontology-form.component';
import { RelationComponent } from './relation/relation.component';
import { SubHeaderComponent } from './sub-header/sub-header.component';
import { RelationFormComponent } from './relation-form/relation-form.component';
import { FigureComponent } from './figure/figure.component';

@NgModule({
  imports: [
    SharedModule,
    OntologyRoutingModule
  ],
  declarations: [OntologyMainComponent, OntologyFormComponent, RelationComponent, SubHeaderComponent, RelationFormComponent, FigureComponent],
  entryComponents: [OntologyFormComponent,RelationFormComponent]
})
export class OntologyModule { }
