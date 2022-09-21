import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CommonService {
  baseUrl = environment.apiUrl + "Common/";

  constructor(private http: HttpClient) { }

  getRoles() {
    return this.http.get<string[]>(this.baseUrl + "roles");
  }
}
