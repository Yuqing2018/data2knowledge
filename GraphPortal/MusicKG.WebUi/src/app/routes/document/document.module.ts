import { NgModule } from '@angular/core';

import { DocumentRoutingModule } from './document-routing.module';
import { DocumentListComponent } from './list/document-list.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  imports: [
    SharedModule,
    DocumentRoutingModule
  ],
  declarations: [DocumentListComponent]
})
export class DocumentModule { }
