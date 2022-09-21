import { Component, OnInit } from '@angular/core';
import { EmployeeParams } from 'src/app/_models/employee-params';
import { Pagination } from 'src/app/_models/pagination';
import { EmployeeModel } from '../employee.Model';
import { EmployeeService } from '../employee.service';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css']
})
export class EmployeeListComponent implements OnInit {
  employees: EmployeeModel[];
  pagination: Pagination;
  employeeParams: EmployeeParams;
  genderList = [{ value: 'male', display: 'Males' }, { value: 'female', display: 'Females' }];

  constructor(private employeeService: EmployeeService) {
    this.employeeParams = this.employeeService.getEmployeeParams();
  }

  ngOnInit(): void {
    this.loadEmployees();
  }

  loadEmployees() {
    this.employeeService.setEmployeeParams(this.employeeParams);
    this.employeeService.getEmployees(this.employeeParams).subscribe({
      next: (response) => {
        this.employees = response.result;
        this.pagination = response.pagination;
      }
    });
  }

  resetFilters() {
    this.employeeParams = this.employeeService.resetEmployeeParams();
    this.loadEmployees();
  }

  pageChanged(event: any) {
    this.employeeParams.pageNumber = event.page;
    this.employeeService.setEmployeeParams(this.employeeParams);
    this.loadEmployees();
  }

}
