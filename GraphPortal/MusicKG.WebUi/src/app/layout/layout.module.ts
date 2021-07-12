import { NgModule } from '@angular/core';
import { NgProgressModule } from '@ngx-progressbar/core';
import { NgProgressRouterModule } from '@ngx-progressbar/router';
import { MainComponent } from './main/main.component';
import { SharedModule } from '../shared/shared.module';
import { SidebarNavComponent } from './sidebar-nav/sidebar-nav.component';
import { HeaderComponent } from './header/header.component';
import { AdminMainComponent } from './admin-main/admin-main.component';
import { AnnotatorMainComponent } from './annotator-main/annotator-main.component';
import { PasswordComponent } from './password/password.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AnnotatorHeaderComponent } from './annotator-header/annotator-header.component';

@NgModule({
  imports: [
    SharedModule,
    NgProgressModule.forRoot(),
    NgProgressRouterModule.forRoot({
      delay: 600
    }),
    FormsModule,
    ReactiveFormsModule
  ],
  declarations: [MainComponent, SidebarNavComponent, HeaderComponent, AdminMainComponent, AnnotatorMainComponent, PasswordComponent, AnnotatorHeaderComponent],
  exports: [MainComponent],
  entryComponents: [PasswordComponent]
})
export class LayoutModule { }
