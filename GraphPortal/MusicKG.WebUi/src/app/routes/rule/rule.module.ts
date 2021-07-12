import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RuleRoutingModule } from './rule-routing.module';
import { ListComponent } from './list/list.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { RuleFormComponent } from './rule-form/rule-form.component';

@NgModule({
  imports: [
    CommonModule,
    RuleRoutingModule,
    SharedModule
  ],
  declarations: [ListComponent, RuleFormComponent],
  entryComponents: [RuleFormComponent]
})
export class RuleModule { }
