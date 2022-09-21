import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { SharedModule } from './_modules/shared.module';
import { ConfirmDialogComponent } from './_modals/confirm-dialog/confirm-dialog.component';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { EmployeeEditComponent } from './employee/employee-edit/employee-edit.component';
import { EmployeeCreateComponent } from './employee/employee-create/employee-create.component';
import { TextInputComponent } from './_forms/text-input/text-input.component';
import { DateInputComponent } from './_forms/date-input/date-input.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { EmployeePanelComponent } from './employee/employee-panel/employee-panel.component';
import { EmployeeListComponent } from './employee/employee-list/employee-list.component';
import { EmployeeCardComponent } from './employee/employee-card/employee-card.component';
import { EmployeeDetailComponent } from './employee/employee-detail/employee-detail.component';
import { HasRolesDirective } from './_directives/has-roles.directive';


@NgModule({
  declarations: [
    AppComponent,
    ConfirmDialogComponent,
    NavComponent,
    HomeComponent,
    EmployeeEditComponent,
    EmployeeCreateComponent,
    TextInputComponent,
    DateInputComponent,
    EmployeePanelComponent,
    EmployeeListComponent,
    EmployeeCardComponent,
    EmployeeDetailComponent,
    HasRolesDirective
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    BrowserAnimationsModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    BsDropdownModule.forRoot(),
    BsDatepickerModule.forRoot()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
