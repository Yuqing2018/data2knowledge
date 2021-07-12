import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CircularTaskComponent } from './circular-task/circular-task.component';
import { CircularTrackComponent } from './circular-track/circular-track.component';
import { CircularInfoComponent } from './circular-info/circular-info.component';

const routes: Routes = [
  { path: 'task', component: CircularTaskComponent },
  { path: 'track', component: CircularTrackComponent },
  { path: 'track/info/:id', component: CircularInfoComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CircularRoutingModule { }
