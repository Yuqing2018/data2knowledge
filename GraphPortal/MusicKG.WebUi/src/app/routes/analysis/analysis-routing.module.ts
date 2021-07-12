import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AnalysisListComponent } from './analysis-list/analysis-list.component';
import { AnalysisInfoComponent } from './analysis-info/analysis-info.component';
import { AnalysisHomeComponent } from './analysis-home/analysis-home.component';

const routes: Routes = [
  { path: '', component: AnalysisListComponent },
  { path: 'info', component: AnalysisInfoComponent },
  { path: 'home', component: AnalysisHomeComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AnalysisRoutingModule { }
