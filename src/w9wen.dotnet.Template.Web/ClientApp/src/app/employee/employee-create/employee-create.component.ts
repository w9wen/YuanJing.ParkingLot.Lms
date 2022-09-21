import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/_services/common.service';
import { EmployeeService } from '../employee.service';

@Component({
  selector: 'app-employee-create',
  templateUrl: './employee-create.component.html',
  styleUrls: ['./employee-create.component.css']
})
export class EmployeeCreateComponent implements OnInit {
  @Output() cancelCreate = new EventEmitter();
  createForm: FormGroup;
  maxDate: Date;
  validationErrors: string[] = [];
  roles: string[];

  constructor(
    private employeeService: EmployeeService,
    private formBuilder: FormBuilder,
    private router: Router,
    private commonService: CommonService) { }

  ngOnInit(): void {
    this.getRoles();
    this.initializeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializeForm() {
    this.createForm = this.formBuilder.group({
      gender: ["male"],
      userName: ["", Validators.required],
      phoneNumber: [""],
      email: [""],
      dateOfBirth: [""],
      city: [""],
      country: [""],
      roles: [[]],
    });
  }

  createEmployee() {
    console.log(this.createForm);
    this.employeeService.createEmployee(this.createForm.value).subscribe({
      next: (response) => {
        console.log(response);
        this.router.navigateByUrl("/employees");
      },
      error: (errors) => {
        this.validationErrors = errors;
      }
    });
  }

  getRoles() {
    return this.commonService.getRoles().subscribe({
      next: (roles) => {
        // console.log(roles);
        this.roles = roles;
      },
    });
  }

  cancel() {
    this.cancelCreate.emit(false);
  }
}
