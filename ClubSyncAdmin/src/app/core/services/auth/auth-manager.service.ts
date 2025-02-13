import { firstValueFrom, map, tap } from "rxjs";
import { AuthStateService } from "./auth-state.service";
import { AuthService, Credentials } from "./auth.service";
import { Injectable } from "@angular/core";
import { jwtDecode } from "jwt-decode"
@Injectable({ providedIn: "root" })
export class AuthManagerService {
  constructor(private authService: AuthService, private authStateService: AuthStateService) { }
  signIn(credentials: Credentials) {
    return this.authService.signIn(credentials).pipe(tap((result) => {
      if (!result.isSuccess || !result.user) {
        return;
      }
      this.authStateService.set({
        accessToken: result.accessToken,
        email: result.user.email,
        id: result.user.id,
        givenName: result.user.givenName,
        surname: result.user.surname,
      })
    }))
  }
  signInWithRefreshToken() {
    return this.authService.signInWithRefreshToken().pipe(tap((result) => {
      if (!result.isSuccess || !result.user) {
        return;
      }
      this.authStateService.set({
        accessToken: result.accessToken,
        email: result.user.email,
        id: result.user.id,
        givenName: result.user.givenName,
        surname: result.user.surname,
      })
    }))
  }
  isAuth() {
    return this.authStateService.getToken().pipe(map(token => !!token));
  }
  async getToken() {
    const token = await firstValueFrom(this.authStateService.getToken());
    if (!token) {
      return null;
    }
    const decodedToken = jwtDecode<{ exp: number }>(token);
    if (!decodedToken.exp) {
      return null;
    }
    const nowUtc = Math.floor(Date.now() / 1000);
    if (nowUtc >= decodedToken.exp) {
      const result = await firstValueFrom(this.authService.signInWithRefreshToken());
      if (result.isSuccess && result.user) {
        this.authStateService.set({
          accessToken: result.accessToken,
          email: result.user.email,
          id: result.user.id,
          givenName: result.user.givenName,
          surname: result.user.surname,
        })
        return result.accessToken;
      }
    }
    return token;
  }

}