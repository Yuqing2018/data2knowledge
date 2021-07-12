import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FaqListComponent } from './faq-list/faq-list.component';
import { FaqDetailComponent } from './faq-detail/faq-detail.component';

const routes: Routes = [
  { path: '', component: FaqListComponent },
  { path: ':id', component: FaqDetailComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FaqRoutingModule { }
