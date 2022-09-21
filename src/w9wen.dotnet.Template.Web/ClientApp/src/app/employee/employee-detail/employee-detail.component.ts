import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { take } from 'rxjs';
import { UserModel } from 'src/app/_models/user-model';
import { AccountService } from 'src/app/_services/account.service';
import { EmployeeModel } from '../employee.Model';

@Component({
  selector: 'app-employee-detail',
  templateUrl: './employee-detail.component.html',
  styleUrls: ['./employee-detail.component.css']
})
export class EmployeeDetailComponent implements OnInit {
  @ViewChild("employeeTabs", { static: true }) employeeTabs: TabsetComponent;
  employee: EmployeeModel;
  activeTab: TabDirective;
  user: UserModel;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: (user) => {
        this.user = user;
      }
    });
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {
    this.route.data.subscribe({
      next: (data) => {
        this.employee = data.employee;
      }
    });

    this.route.queryParams.subscribe({
      next: (params) => {
        params.tab ? this.selectTab(params.tab) : this.selectTab(0);
      }
    });
  }

  selectTab(tabId: number) {
    this.employeeTabs.tabs[tabId].active = true;
  }

  onTabActivated(data: TabDirective) {
  }

}
