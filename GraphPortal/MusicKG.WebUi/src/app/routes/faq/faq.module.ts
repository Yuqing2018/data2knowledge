import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FaqRoutingModule } from './faq-routing.module';
import { FaqListComponent } from './faq-list/faq-list.component';
import { FaqDetailComponent } from './faq-detail/faq-detail.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { KnowledgeTemplateSelectComponent } from './knowledge-template-select/knowledge-template-select.component';

@NgModule({
  declarations: [FaqListComponent, FaqDetailComponent, KnowledgeTemplateSelectComponent],
  imports: [
    SharedModule,
    FaqRoutingModule
  ]
})
export class FaqModule { }
