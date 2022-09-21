import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { Project, ProjectListResponse } from './_models/project-list-response.model';
import { ProjectResponse } from './_models/project-response.model';
import { UserModel } from './_models/user-model';
import { AccountService } from './_services/account.service';
import { ConfirmService } from './_services/confirm.service';
import { ProjectService } from './_services/project.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'W9WEN App';
  projectListResponse: ProjectListResponse;
  projectResponse: ProjectResponse;

  constructor(
    private projectService: ProjectService,
    private confirmService: ConfirmService,
    private toastrService: ToastrService,
    private spinnerService: NgxSpinnerService,
    private accountService: AccountService) { }

  ngOnInit(): void {
    this.getProjects();
    // this.spinnerService.show();

    setTimeout(() => {
      this.spinnerService.hide();
    }, 5000);

    this.setCurrentUser();

  }
  setCurrentUser() {
    const user: UserModel = JSON.parse(localStorage.getItem("user"));
    if (user) {
      this.accountService.setCurrentUser(user);
    }
  }

  getProjects() {
    this.projectService.getProjects().subscribe({
      complete: () => { },
      error: () => { },
      next: (response) => {
        this.projectListResponse = response;
      },
    })
  }

  getProject() {
    this.projectService.getProject(5).subscribe({
      complete: () => { },
      error: () => { },
      next: (response) => {
        this.projectResponse = response;
      },
    });
  }

  createProject() {
    var name = "From Code";
    this.projectService.createProject(name).subscribe({
      complete: () => { },
      error: () => { },
      next: (response) => {
        // console.log("create success : " + response);
        Swal.fire("Create success: " + response);
      },
    });
  }

  updateProject() {
    var project: Project = { id: 3, name: "Update From Code" };
    this.projectService.updateProject(project).subscribe({
      complete: () => { },
      error: () => { },
      next: (response) => {
        // console.log("update success");
        this.toastrService.info("Update Success");
      },
    });
  }

  deleteProject(id: number) {
    this.confirmService.confirm('Confirm delete project', 'This cannot be undone').subscribe(result => {
      if (result) {
        this.projectService.deleteProject(id).subscribe({
          complete: () => { },
          error: () => { },
          next: (response) => {
            console.log(response);
          },
        });
      }

      // if (result) {
      //   this.messageService.deleteMessage(id).subscribe(() => {
      //     this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
      //   })
      // }
    })

  }
}
