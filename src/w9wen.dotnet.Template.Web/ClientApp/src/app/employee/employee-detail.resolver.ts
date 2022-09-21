import { Injectable } from '@angular/core';
import {
  Router, Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { EmployeeModel } from './employee.Model';
import { EmployeeService } from './employee.service';

@Injectable({
  providedIn: 'root'
})
export class EmployeeDetailResolver implements Resolve<EmployeeModel> {

  constructor(private employeeService: EmployeeService) { }

  resolve(route: ActivatedRouteSnapshot): Observable<EmployeeModel> {
    return this.employeeService.getEmployee(route.paramMap.get("username"));
  }
}
