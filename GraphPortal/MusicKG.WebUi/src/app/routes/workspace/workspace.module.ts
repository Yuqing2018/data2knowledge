import { NgModule } from '@angular/core';

import { WorkspaceRoutingModule } from './workspace-routing.module';
import { WorkspaceMainComponent } from './main/workspace-main.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { WorkspaceFormComponent } from './form/workspace-form.component';

@NgModule({
  declarations: [WorkspaceMainComponent, WorkspaceFormComponent],
  imports: [
    SharedModule,
    WorkspaceRoutingModule
  ],
  entryComponents: [WorkspaceFormComponent]
})
export class WorkspaceModule { }
