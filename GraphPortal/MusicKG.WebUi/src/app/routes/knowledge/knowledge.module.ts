import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';

import { KnowledgeRoutingModule } from './knowledge-routing.module';
import { KnowledgeMainComponent } from './knowledge-main/knowledge-main.component';

@NgModule({
  declarations: [KnowledgeMainComponent],
  imports: [
    SharedModule,
    KnowledgeRoutingModule
  ]
})
export class KnowledgeModule { }
