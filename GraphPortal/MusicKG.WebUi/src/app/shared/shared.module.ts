import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgZorroAntdModule } from 'ng-zorro-antd';
import { InputTrimModule } from 'ng2-trim-directive';
import { ColorPickerModule } from 'ngx-color-picker';
import { AnnotationModule } from './annotation/annotation.module';
import { EntityAnnotationToolComponent } from './entity-annotation-tool/entity-annotation-tool.component';
import { EntityWordComponent } from './entity-annotation-tool/entity-annotation-word';
import { EntityAnnoationLabel } from './entity-annotation-tool/entity-annotation-label';
import { WorkspaceTypePipe } from './pipe/workspace-type.pipe';
import { WorkspaceLanguagePipe } from './pipe/workspace-language.pipe';
import { TextEllipsisPipe } from './pipe/text-ellipsis.pipe';
import { UserRolePipe } from './pipe/user-role.pipe';
import { TaskStatusPipe } from './pipe/task-status.pipe';
import { DocumentStatusPipe } from './pipe/document-status.pipe';
import { TaskDocumentStatusPipe } from './pipe/task-document-status.pipe';
import { TaskFormComponent } from './task-form/task-form.component';
import { TokenizationAnnotationToolComponent, SpanComponent } from './tokenization-annotation-tool/tokenization-annotation-tool.component';
import { AnnotationToolbarComponent } from './annotation-toolbar/annotation-toolbar.component';
import { TextparaphraseAnnotationToolComponent } from './textparaphrase-annotation-tool/textparaphrase-annotation-tool.component';
import { PasswordComponent } from './password/password.component';
import { DictionarySelectComponent } from './dictionary-select/dictionary-select.component';
import { ScrollDispatchModule } from '@angular/cdk/scrolling';
import { DictionaryItemSelectComponent } from './dictionary-item-select/dictionary-item-select.component';
import { StatisticIsmanagerPipe } from './pipe/statistic-ismanager.pipe';
import { QuickJumperComponent } from './quick-jumper/quick-jumper.component';
import { SafePipe } from './pipe/safe.pipe';
import { EntitytypeAnnotationToolComponent } from './entitytype-annotation-tool/entitytype-annotation-tool.component';
import { EntitytypeComponent } from './entitytype/entitytype.component';
import { OntologyTreeComponent } from './ontology-tree/ontology-tree.component';
import { IntentAnnotationToolComponent } from './intent-annotation-tool/intent-annotation-tool.component';
import { ObserversModule } from '@angular/cdk/observers';
import { TemplateInputComponent } from './template-input/template-input.component';
import { GraphTagComponent } from './graph-tag/graph-tag.component';
import { GraphAnnotationToolComponent } from './graph-annotation-tool/graph-annotation-tool.component';
import { SyndromeItemSelectComponent } from './syndrome-item-select/syndrome-item-select.component';
import { DictionaryInputSelectComponent } from './dictionary-input-select/dictionary-input-select.component';
import { DictionaryIdSelectComponent } from './dictionary-id-select/dictionary-id-select.component';
import { SyndromePipe } from './pipe/syndrome.pipe';
import { YearsPipe } from './pipe/years.pipe';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgZorroAntdModule,
    ObserversModule
  ],
  declarations: [TaskFormComponent, EntityAnnotationToolComponent, EntityWordComponent, EntityAnnoationLabel, WorkspaceTypePipe,
    WorkspaceLanguagePipe, TextEllipsisPipe, TaskStatusPipe, DocumentStatusPipe, UserRolePipe, TaskDocumentStatusPipe, SafePipe,
    TokenizationAnnotationToolComponent, SpanComponent, AnnotationToolbarComponent, TextparaphraseAnnotationToolComponent, PasswordComponent, DictionarySelectComponent, DictionaryItemSelectComponent, StatisticIsmanagerPipe, QuickJumperComponent, EntitytypeAnnotationToolComponent, EntitytypeComponent, OntologyTreeComponent, IntentAnnotationToolComponent, TemplateInputComponent, GraphTagComponent, GraphAnnotationToolComponent, SyndromeItemSelectComponent, DictionaryInputSelectComponent, DictionaryIdSelectComponent, SyndromePipe, YearsPipe],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    NgZorroAntdModule,
    AnnotationModule,
    EntityAnnotationToolComponent,
    WorkspaceLanguagePipe,
    WorkspaceTypePipe,
    TextEllipsisPipe,
    TaskStatusPipe,
    DocumentStatusPipe,
    UserRolePipe,
    TaskDocumentStatusPipe,
    TaskFormComponent,
    InputTrimModule,
    ColorPickerModule,
    TokenizationAnnotationToolComponent,
    AnnotationToolbarComponent,
    TextparaphraseAnnotationToolComponent,
    PasswordComponent,
    DictionarySelectComponent,
    ScrollDispatchModule,
    DictionaryItemSelectComponent,
    StatisticIsmanagerPipe,
    QuickJumperComponent,
    SafePipe,
    EntitytypeAnnotationToolComponent,
    OntologyTreeComponent,
    IntentAnnotationToolComponent,
    GraphTagComponent,
    GraphAnnotationToolComponent,
    SyndromeItemSelectComponent,
    DictionaryInputSelectComponent,
    DictionaryIdSelectComponent,
    SyndromePipe,
    YearsPipe
  ],
  entryComponents: [TaskFormComponent]
})
export class SharedModule { }
