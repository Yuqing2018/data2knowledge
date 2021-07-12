import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OntologyMainComponent } from './main/ontology-main.component';
import { RelationComponent } from './relation/relation.component';
import { FigureComponent } from './figure/figure.component';
import { CanDeactivateGuard } from 'src/app/core/can-deactivate.guard';


const routes: Routes = [
  { path: '', component: OntologyMainComponent , canDeactivate: [CanDeactivateGuard]},
  { path: 'relation', component: RelationComponent },
  { path: 'figure', component: FigureComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OntologyRoutingModule { }
