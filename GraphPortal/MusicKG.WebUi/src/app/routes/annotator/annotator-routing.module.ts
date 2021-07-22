import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AnnotatorTaskComponent } from './annotator-task/annotator-task.component';
import { AnnotatorDocumentComponent } from './annotator-document/annotator-document.component';
import { AnnotatorTextSimilarityComponent } from './annotator-text-similarity/annotator-text-similarity.component';
import { CanDeactivateGuard } from 'src/app/core/can-deactivate.guard';
import { AnnotatorEntityComponent } from './annotator-entity/annotator-entity.component';
import { AnnotatorTokenizationComponent } from './annotator-tokenization/annotator-tokenization.component';
import { AnnotatorTextparaphraseComponent } from './annotator-textparaphrase/annotator-textparaphrase.component';
import { AnnotatorEntitytypeComponent } from './annotator-entitytype/annotator-entitytype.component'
import { AnnotatorIntentComponent } from './annotator-intent/annotator-intent.component';
import { AnnotatorGraphComponent } from './annotator-graph/annotator-graph.component'
import { AnnotatorClassificationComponent } from './annotator-classification/annotator-classification.component';
import { AnnotatorReasonComponent } from './annotator-reason/annotator-reason.component';
import { AnnotatorConfigComponent } from './annotator-config/annotator-config.component';
import { AnnotatorAgainComponent } from './annotator-again/annotator-again.component';
import { ReasonAuthGuard } from 'src/app/core/reason-auth.service';

const routes: Routes = [
  { path: 'task', component: AnnotatorTaskComponent },
  { path: 'reason', component: AnnotatorReasonComponent, canActivate: [ReasonAuthGuard] },
  { path: 'config', component: AnnotatorConfigComponent },
  { path: 'again', component: AnnotatorAgainComponent },//新增3-18 yt
  { path: 'task/:task/doc/:doc/dialog/intent', component: AnnotatorIntentComponent, canDeactivate: [CanDeactivateGuard] },
  { path: 'task/:task/doc/:doc/dialog/entity', component: AnnotatorEntitytypeComponent, canDeactivate: [CanDeactivateGuard] },
  { path: 'task/:task', component: AnnotatorDocumentComponent },
  { path: 'task/:task/doc/:doc/textSimilarity', component: AnnotatorTextSimilarityComponent, canDeactivate: [CanDeactivateGuard] },
  { path: 'task/:task/doc/:doc/entity', component: AnnotatorEntityComponent, canDeactivate: [CanDeactivateGuard] },
  { path: 'task/:task/doc/:doc/tokenization', component: AnnotatorTokenizationComponent, canDeactivate: [CanDeactivateGuard] },
  { path: 'task/:task/doc/:doc/textparaphrase', component: AnnotatorTextparaphraseComponent, canDeactivate: [CanDeactivateGuard] },
  { path: 'task/:task/doc/:doc/graph', component: AnnotatorGraphComponent, canDeactivate: [CanDeactivateGuard] },
  { path: 'task/:task/doc/:doc/classification', component: AnnotatorClassificationComponent, canDeactivate: [CanDeactivateGuard] },
  { path: '', redirectTo: '/annotator/task' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AnnotatorRoutingModule { }
