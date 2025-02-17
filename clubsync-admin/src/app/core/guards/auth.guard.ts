import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { AuthManagerService } from '#core/services/auth/auth-manager.service';
import { map, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authManagerService: AuthManagerService, private router: Router) { }
  canActivate(_: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return this.authManagerService.isAuth().pipe(
      tap(isAuthenticated => {
        if (!isAuthenticated) {
          this.router.navigate(['/'], { queryParams: { returnUrl: state.url } });
        }
      }),
      map(isAuthenticated => isAuthenticated)
    );
  }
}