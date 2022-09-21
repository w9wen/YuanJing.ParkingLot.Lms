import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Project, ProjectListResponse } from '../_models/project-list-response.model';
import { ProjectResponse } from '../_models/project-response.model';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  createProject(name: string) {
    var projectName = { "name": name };
    return this.http.post(this.baseUrl + "Projects", projectName).pipe(
      map((response: Project) => {
        return response;
      })
    )
  }

  getProjects() {
    return this.http.get<ProjectListResponse>(this.baseUrl + "Projects");
  }

  getProject(id: Number) {
    return this.http.get<ProjectResponse>(this.baseUrl + "Projects/" + id);
  }

  updateProject(project: Project) {
    return this.http.put(this.baseUrl + 'Projects', project).pipe(
      map(() => {
        console.log("update success");
      })
    )
  }

  deleteProject(id: number) {
    return this.http.delete(this.baseUrl + 'Projects/' + id);
  }

}
