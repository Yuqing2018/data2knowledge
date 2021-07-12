import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OntologyDialogueMainComponent } from './main/ontology-dialogue-main.component';

const routes: Routes = [
  { path: '', component: OntologyDialogueMainComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OntologyDialogueRoutingModule { }
