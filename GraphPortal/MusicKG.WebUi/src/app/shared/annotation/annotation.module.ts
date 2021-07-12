import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SentenceComponent } from './sentence.component';
import { TextAnnotationComponent } from './text-annotation.component';
import { WordComponent } from './word.component';
import { AnnotationService } from './annotation.service';
import { NgZorroAntdModule } from 'ng-zorro-antd';
import { AnnoationLabel } from './label.component';
import { PlaceholderComponent, AddPlaceholderComponent } from './placeholders.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    NgZorroAntdModule
  ],
  declarations: [SentenceComponent, WordComponent, TextAnnotationComponent, AnnoationLabel, PlaceholderComponent, AddPlaceholderComponent],
  entryComponents: [AddPlaceholderComponent],
  exports: [
    TextAnnotationComponent  
  ]
})
export class AnnotationModule { }
