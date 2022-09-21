import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { EmployeeParams } from '../_models/employee-params';
import { UserModel } from '../_models/user-model';
import { AccountService } from '../_services/account.service';
import { getPaginatedResult, getPaginationHeaders } from '../_services/paginationHelper';
import { EmployeeModel } from './employee.Model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  baseUrl = environment.apiUrl;
  user: UserModel;
  employeeParams: EmployeeParams;

  constructor(
    private http: HttpClient,
    private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: (user) => {
        this.user = user;
        this.employeeParams = new EmployeeParams(user);
      }
    });
  }

  getEmployeeParams() {
    return this.employeeParams;
  }

  setEmployeeParams(params: EmployeeParams) {
    this.employeeParams = params;
  }

  resetEmployeeParams() {
    this.employeeParams = new EmployeeParams(this.user);
    return this.employeeParams;
  }

  getEmployees(employeeParams: EmployeeParams) {
    let params = getPaginationHeaders(employeeParams.pageNumber, employeeParams.pageSize);

    params = params.append("gender", employeeParams.gender);
    params = params.append("orderBy", employeeParams.orderBy);

    return getPaginatedResult<EmployeeModel[]>(this.baseUrl + "Employee", params, this.http)
      .pipe(map(response => {
        return response;
      }))
  }

  getEmployee(userName: string) {
    return this.http.get<EmployeeModel>(this.baseUrl + "Employee/" + userName);
  }

  createEmployee(model: any) {
    return this.http.post(this.baseUrl + "Employee", model).pipe(
      map((employee: EmployeeModel) => {
        if (employee) {
          console.log("Created success");
        }
      })
    );
  }

  updateEmployee(employee: EmployeeModel) {
    return this.http.put(this.baseUrl + "Employee", employee).pipe(
      map(() => {
        // Store employee in list here.
      })
    );
  }
}
