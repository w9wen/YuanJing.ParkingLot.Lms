import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { map, Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root'
})
export class ViewPermissionGuard implements CanActivate {

  constructor(
    private accountService: AccountService,
    private toastrService: ToastrService) { }

  canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map(user => {
        if (user.roles.includes("SuperAdmin") || user.roles.includes("Admin") || user.roles.includes("Operator")) {
          return true;
        }
        this.toastrService.error("You cannot enter this area");
      })
    );
  }

}
