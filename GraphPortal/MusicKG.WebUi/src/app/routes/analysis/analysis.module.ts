import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AnalysisRoutingModule } from './analysis-routing.module';
import { AnalysisListComponent } from './analysis-list/analysis-list.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { AnalysisInfoComponent } from './analysis-info/analysis-info.component';
import { AnalysisTableComponent } from './analysis-table/analysis-table.component';
import { NgxEchartsModule } from 'ngx-echarts';
import { AnalysisSelectComponent } from './analysis-select/analysis-select.component';
import { AnalysisLineChartComponent } from './analysis-line-chart/analysis-line-chart.component';
import { AnalysisCommonChartComponent } from './analysis-common-chart/analysis-common-chart.component';
import { ExportModalComponent } from './export-modal/export-modal.component';
import { AnalysisFilterComponent } from './analysis-filter/analysis-filter.component';
import { EditModalComponent } from './edit-modal/edit-modal.component';
import { AnalysisEditdatafromModalComponent } from './analysis-editdatafrom-modal/analysis-editdatafrom-modal.component';
import { AnalysisRelationModalComponent } from './analysis-relation-modal/analysis-relation-modal.component';
import { AnalysisManualaddModalComponent } from './analysis-manualadd-modal/analysis-manualadd-modal.component';
import { AnalysisEditcntrtimeModalComponent } from './analysis-editcntrtime-modal/analysis-editcntrtime-modal.component';
import { AnalysisHomeComponent } from './analysis-home/analysis-home.component';
import { AnalysisWarningChartComponent } from './analysis-warning-chart/analysis-warning-chart.component';
import { AnalysisChannelChartComponent } from './analysis-channel-chart/analysis-channel-chart.component';
import { AnalysisKeyriskTableComponent } from './analysis-keyrisk-table/analysis-keyrisk-table.component';

@NgModule({
  declarations: [AnalysisListComponent, AnalysisInfoComponent, AnalysisTableComponent, AnalysisSelectComponent, AnalysisLineChartComponent, AnalysisCommonChartComponent, ExportModalComponent, AnalysisFilterComponent, EditModalComponent, AnalysisEditdatafromModalComponent, AnalysisRelationModalComponent, AnalysisManualaddModalComponent, AnalysisEditcntrtimeModalComponent, AnalysisHomeComponent, AnalysisWarningChartComponent, AnalysisChannelChartComponent, AnalysisKeyriskTableComponent],
  imports: [
    SharedModule,
    AnalysisRoutingModule,
    NgxEchartsModule.forRoot({
      echarts: (window as any).echarts,
    }),
  ],
  entryComponents: [ExportModalComponent,EditModalComponent,AnalysisTableComponent,AnalysisEditdatafromModalComponent,AnalysisRelationModalComponent,AnalysisManualaddModalComponent,AnalysisEditcntrtimeModalComponent]
})
export class AnalysisModule { }
