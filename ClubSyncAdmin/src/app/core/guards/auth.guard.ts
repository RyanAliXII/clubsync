import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthManagerService } from '#core/services/auth/auth-manager.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authManagerService: AuthManagerService, private router: Router) { }
  canActivate(): boolean {
    if (this.authManagerService.isAuth()) {
      return true; // Allow access if the user is authenticated
    } else {
      this.router.navigate(['/']); // Redirect to login if not authenticated
      return false; // Prevent access to the route
    }
  }
}