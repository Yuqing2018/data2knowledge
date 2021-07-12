import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { KnowledgeMainComponent } from './knowledge-main/knowledge-main.component';

const routes: Routes = [
  { path: '', component: KnowledgeMainComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class KnowledgeRoutingModule { }
