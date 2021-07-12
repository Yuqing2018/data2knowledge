import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AnnotatorRoutingModule } from './annotator-routing.module';
import { AnnotatorTaskComponent } from './annotator-task/annotator-task.component';
import { AnnotatorDocumentComponent } from './annotator-document/annotator-document.component';
import { AnnotatorTextSimilarityComponent } from './annotator-text-similarity/annotator-text-similarity.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { AnnotatorEntityComponent } from './annotator-entity/annotator-entity.component';
import { AnnotatorTokenizationComponent } from './annotator-tokenization/annotator-tokenization.component';
import { AnnotatorTextparaphraseComponent } from './annotator-textparaphrase/annotator-textparaphrase.component';
import { AnnotatorEntitytypeComponent } from './annotator-entitytype/annotator-entitytype.component';
import { AnnotatorIntentComponent } from './annotator-intent/annotator-intent.component';
import { AnnotatorGraphComponent } from './annotator-graph/annotator-graph.component';
import { AnnotatorClassificationComponent } from './annotator-classification/annotator-classification.component';
import { AnnotatorReasonComponent } from './annotator-reason/annotator-reason.component';
import { AnnotatorConfigComponent } from './annotator-config/annotator-config.component';
import { AnnotatorUploadComponent } from './annotator-upload/annotator-upload.component';
import { AnnotatorAgainComponent } from './annotator-again/annotator-again.component';
import { BatchModalComponent } from './batch-modal/batch-modal.component';

@NgModule({
  declarations: [AnnotatorTaskComponent, AnnotatorDocumentComponent, AnnotatorTextSimilarityComponent, AnnotatorEntityComponent, AnnotatorTokenizationComponent, AnnotatorTextparaphraseComponent, AnnotatorEntitytypeComponent, AnnotatorIntentComponent, AnnotatorGraphComponent, AnnotatorClassificationComponent, AnnotatorReasonComponent, AnnotatorConfigComponent, AnnotatorUploadComponent, AnnotatorAgainComponent, BatchModalComponent],
  imports: [
    SharedModule,
    AnnotatorRoutingModule
  ],
  entryComponents: [AnnotatorUploadComponent, BatchModalComponent]
})
export class AnnotatorModule { }
