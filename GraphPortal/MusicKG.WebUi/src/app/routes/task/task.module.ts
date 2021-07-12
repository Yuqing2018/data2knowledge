import { NgModule } from '@angular/core';

import { TaskRoutingModule } from './task-routing.module';
import { TaskListComponent } from './list/task-list.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { TaskAnnotationComponent } from './annotation/task-annotation.component';
import { TaskDetailComponent } from './detail/task-detail.component';
import { EntityAnnotationComponent } from './entity-annotation/entity-annotation.component';
import { TextSimilarityComponent } from './text-similarity/text-similarity.component';
import { DocCheckListComponent } from './doc-check-list/doc-check-list.component';
import { TaskUpdateFormComponent } from './task-update-form/task-update-form.component';
import { TokenizationComponent } from './tokenization/tokenization.component';
import { TextparaphraseComponent } from './textparaphrase/textparaphrase.component';
import { EntitytypeComponent } from './entitytype/entitytype.component';
import { IntentComponent } from './intent/intent.component';
import { GraphComponent } from './graph/graph.component';

@NgModule({
  imports: [
    SharedModule,
    TaskRoutingModule
  ],
  declarations: [TaskListComponent, TaskAnnotationComponent, TaskDetailComponent, EntityAnnotationComponent, TextSimilarityComponent, DocCheckListComponent, TaskUpdateFormComponent, TokenizationComponent, TextparaphraseComponent, EntitytypeComponent, IntentComponent, GraphComponent],
  entryComponents: [TaskUpdateFormComponent]
})
export class TaskModule { }
