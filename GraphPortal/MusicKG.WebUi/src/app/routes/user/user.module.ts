import { NgModule } from '@angular/core';

import { UserRoutingModule } from './user-routing.module';
import { UserListComponent } from './list/user-list.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { UserFormComponent } from './user-form/user-form.component';
import { UserPasswordComponent } from './user-password/user-password.component';
import { UserEditComponent } from './user-edit/user-edit.component';

@NgModule({
  imports: [
    SharedModule,
    UserRoutingModule
  ],
  declarations: [UserListComponent, UserFormComponent, UserPasswordComponent, UserEditComponent],
  entryComponents: [UserFormComponent, UserPasswordComponent, UserEditComponent]
})
export class UserModule { }
