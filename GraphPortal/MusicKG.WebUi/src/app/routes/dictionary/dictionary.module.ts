import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DictionaryRoutingModule } from './dictionary-routing.module';
import { ListComponent } from './list/list.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { InputNameComponent } from './input-name/input-name.component';
import { DetailComponent } from './detail/detail.component';

@NgModule({
  declarations: [ListComponent, InputNameComponent, DetailComponent],
  imports: [
    CommonModule,
    DictionaryRoutingModule,
    SharedModule
  ]
})
export class DictionaryModule { }
