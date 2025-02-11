import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

export interface UserState {
  id: string | null;
  givenName: string | null;
  surname: string | null;
  email: string | null;
  accessToken: string | null;
  isAuth: boolean;
}

@Injectable({ providedIn: "root" })
export class AuthStateService {
  private userSubject = new BehaviorSubject<UserState>({
    id: null,
    givenName: null,
    surname: null,
    email: null,
    accessToken: null,
    isAuth: false,
  });

  user$ = this.userSubject.asObservable(); // Components can subscribe

  setUser(user: UserState) {
    this.userSubject.next(user);
  }
  clearUser() {
    this.userSubject.next({ id: null, email: null, accessToken: null, isAuth: false, givenName: null,surname: null });
  }
}