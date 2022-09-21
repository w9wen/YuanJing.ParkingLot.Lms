import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { map, ReplaySubject, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import Swal from 'sweetalert2';
import { UserModel } from '../_models/user-model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;

  private currentUserSource = new ReplaySubject<UserModel>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(
    private http: HttpClient,
    private jwtHelperService: JwtHelperService,
    private router: Router) { }

  login(model: any) {
    return this.http.post(this.baseUrl + "account/login", model).pipe(
      map((response: UserModel) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

  logout() {
    this.router.navigate(["/"]);
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

  setCurrentUser(user: UserModel) {
    user.roles = [];
    const roles = this.jwtHelperService.decodeToken(user.token).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);

    console.log(`toke expired: ${this.jwtHelperService.isTokenExpired()}`)
    if (this.jwtHelperService.isTokenExpired()) {
      this.logout();
      Swal.fire(
        "w9wen",
        "登入逾時，請重新登入",
        "warning"
      );
    } else {
      this.autoLogout(this.jwtHelperService.getTokenExpirationDate());
    }
  }

  autoLogout(tokenExpirationDate: Date) {
    let tokenExpiration = new Date(tokenExpirationDate).getTime() - new Date().getTime();
    setTimeout(() => {
      console.log(`token expired!`);
      this.logout();
      Swal.fire(
        "w9wen",
        "登入逾時，請重新登入",
        "warning"
      );
    }, tokenExpiration);
  }

  // getDecodedToken(token: string) {
  //   return JSON.parse(atob(token.split('.')[1]));
  // }
}
