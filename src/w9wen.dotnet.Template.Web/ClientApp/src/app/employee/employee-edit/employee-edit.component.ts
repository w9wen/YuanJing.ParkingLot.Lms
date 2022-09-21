import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { UserModel } from 'src/app/_models/user-model';
import { AccountService } from 'src/app/_services/account.service';
import { CommonService } from 'src/app/_services/common.service';
import { EmployeeModel } from '../employee.Model';
import { EmployeeService } from '../employee.service';

@Component({
  selector: 'app-employee-edit',
  templateUrl: './employee-edit.component.html',
  styleUrls: ['./employee-edit.component.css']
})
export class EmployeeEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm;
  disabled = false;
  userItem: UserModel;
  employeeItem: EmployeeModel;
  roles: string[];

  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private employeeService: EmployeeService,
    private accountService: AccountService,
    private commonService: CommonService,
    private toastrService: ToastrService) {

    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: (response) => {
        this.userItem = response;
      },
    });

    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {
    this.route.data.subscribe({
      next: (data) => {
        this.employeeItem = data.employee;
      }
    });
    // this.loadEmployee();
    this.getRoles();
  }

  // loadEmployee() {
  //   this.employeeService.getEmployee(this.employeeItem.userName).subscribe({
  //     next: (response) => {
  //       this.employeeItem = response;
  //     },
  //   });
  // }

  updateEmployee() {
    this.employeeService.updateEmployee(this.employeeItem).subscribe({
      complete: () => {
        this.toastrService.success('Profile updated completed');
      },
      next: () => {
        this.toastrService.success('Profile updated successfully');
        this.router.navigateByUrl("employee/" + this.employeeItem.userName);
      },
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

}
