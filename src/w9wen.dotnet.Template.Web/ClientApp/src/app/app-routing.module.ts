import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeeCreateComponent } from './employee/employee-create/employee-create.component';
import { EmployeeDetailResolver } from './employee/employee-detail.resolver';
import { EmployeeDetailComponent } from './employee/employee-detail/employee-detail.component';
import { EmployeeEditComponent } from './employee/employee-edit/employee-edit.component';
import { EmployeePanelComponent } from './employee/employee-panel/employee-panel.component';
import { HomeComponent } from './home/home.component';
import { ViewPermissionGuard } from './_guards/admin.guard';

const routes: Routes = [
  { path: "", component: HomeComponent },
  {
    path: "",
    children: [
      {
        path: "employees",
        component: EmployeePanelComponent,
        canActivate: [ViewPermissionGuard]
      },
      {
        path: "employee/edit/:username",
        component: EmployeeEditComponent,
        resolve: { employee: EmployeeDetailResolver }
      },
      {
        path: "employees/create",
        component: EmployeeCreateComponent
      },
      { path: "employee/:username", component: EmployeeDetailComponent, resolve: { employee: EmployeeDetailResolver } }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
