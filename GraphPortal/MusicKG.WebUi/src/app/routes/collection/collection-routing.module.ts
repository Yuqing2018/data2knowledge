import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CollectionListComponent } from './collection-list/collection-list.component';
import { CollectionConfigComponent } from './collection-config/collection-config.component';

const routes: Routes = [
  { path: '', component: CollectionListComponent },
  { path: 'config', component: CollectionConfigComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CollectionRoutingModule { }
