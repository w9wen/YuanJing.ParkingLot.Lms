import { Component, Input, OnInit } from '@angular/core';
import { EmployeeModel } from '../employee.Model';
import { EmployeeService } from '../employee.service';

@Component({
  selector: 'app-employee-card',
  templateUrl: './employee-card.component.html',
  styleUrls: ['./employee-card.component.css']
})
export class EmployeeCardComponent implements OnInit {
  @Input() employee: EmployeeModel;

  constructor(private employeeService: EmployeeService) { }

  ngOnInit(): void {
  }

}
