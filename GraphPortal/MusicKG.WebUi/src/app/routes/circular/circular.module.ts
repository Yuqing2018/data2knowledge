import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CircularRoutingModule } from './circular-routing.module';
import { CircularTaskComponent } from './circular-task/circular-task.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { CircularTaskFormComponent } from './circular-task-form/circular-task-form.component';
import { YearsRangeComponent } from './years-range/years-range.component';
import { CircularTrackComponent } from './circular-track/circular-track.component';
import { CircularFilterComponent } from './circular-filter/circular-filter.component';
import { CircularInfoComponent } from './circular-info/circular-info.component';
import { CircularConfirmComponent } from './circular-confirm/circular-confirm.component';
import { FrequencyFormComponent } from './frequency-form/frequency-form.component';
import { LinkSettingComponent } from './link-setting/link-setting.component';
import { CircularInfoModalComponent } from './circular-info-modal/circular-info-modal.component';
import { CircularLevelFormComponent } from './circular-level-form/circular-level-form.component';

@NgModule({
  declarations: [CircularTaskComponent, CircularTaskFormComponent, YearsRangeComponent, CircularTrackComponent, CircularFilterComponent, CircularInfoComponent, CircularConfirmComponent, FrequencyFormComponent, LinkSettingComponent, CircularInfoModalComponent, CircularLevelFormComponent],
  imports: [
    SharedModule,
    CircularRoutingModule
  ],
  entryComponents: [
    CircularTaskFormComponent,
    CircularConfirmComponent,
    FrequencyFormComponent,
    LinkSettingComponent,
    CircularLevelFormComponent,
    CircularInfoModalComponent
  ]
})
export class CircularModule { }
