import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TaskListComponent } from './list/task-list.component';
import { TaskAnnotationComponent } from './annotation/task-annotation.component';
import { TaskDetailComponent } from './detail/task-detail.component';
import { EntityAnnotationComponent } from './entity-annotation/entity-annotation.component';
import { TextSimilarityComponent } from './text-similarity/text-similarity.component';
import { CanDeactivateGuard } from 'src/app/core/can-deactivate.guard';
import { TokenizationComponent } from './tokenization/tokenization.component';
import { TextparaphraseComponent } from './textparaphrase/textparaphrase.component';
import { EntitytypeComponent } from './entitytype/entitytype.component'
import { IntentComponent } from './intent/intent.component';
import { GraphComponent } from './graph/graph.component';

const routes: Routes = [
  { path: '', component: TaskListComponent },
  { path: ':task/annotation/:doc', component: TaskAnnotationComponent },
  { path: ':task/doc/:doc/dialog/intent', component: IntentComponent, canDeactivate: [CanDeactivateGuard] },
  { path: ':task/doc/:doc/dialog/entity', component: EntitytypeComponent, canDeactivate: [CanDeactivateGuard] },
  { path: ':task/doc/:doc/entity', component: EntityAnnotationComponent, canDeactivate: [CanDeactivateGuard] },
  { path: ':task/doc/:doc/textSimilarity', component: TextSimilarityComponent, canDeactivate: [CanDeactivateGuard] },
  { path: ':task/doc/:doc/tokenization', component: TokenizationComponent, canDeactivate: [CanDeactivateGuard] },   
  { path: ':task/doc/:doc/textparaphrase', component: TextparaphraseComponent, canDeactivate: [CanDeactivateGuard] },
  { path: ':task/doc/:doc/graph', component: GraphComponent, canDeactivate: [CanDeactivateGuard] },
  { path: ':task', component: TaskDetailComponent  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TaskRoutingModule { }
