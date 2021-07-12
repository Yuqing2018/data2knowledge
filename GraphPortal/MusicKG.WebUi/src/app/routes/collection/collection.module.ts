import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { CollectionRoutingModule } from './collection-routing.module';
import { CollectionListComponent } from './collection-list/collection-list.component';
import { CollectionFormComponent } from './collection-form/collection-form.component';
import { CollectionConfigComponent } from './collection-config/collection-config.component';
import { ConfigFormComponent } from './config-form/config-form.component';

@NgModule({
  declarations: [CollectionListComponent, CollectionFormComponent, CollectionConfigComponent, ConfigFormComponent],
  imports: [
    SharedModule,
    CollectionRoutingModule
  ],
  entryComponents: [CollectionFormComponent,ConfigFormComponent]
})
export class CollectionModule { }
