import { Injectable } from "@angular/core";
import { BehaviorSubject, map } from "rxjs";

type AuthState = {
  id: string | null;
  givenName: string | null;
  surname: string | null;
  email: string | null;
  accessToken: string | null;
}
interface UserData extends Omit<AuthState, "accessToken"> { }
@Injectable({ providedIn: "root" })
export class AuthStateService {
  private authStateSubject = new BehaviorSubject<AuthState>({
    id: null,
    givenName: null,
    surname: null,
    email: null,
    accessToken: null,
  });
  user$ = this.authStateSubject.pipe(map(({ accessToken, ...rest }) => (rest) as UserData));
  // Components can subscribe
  set(state: AuthState) {
    this.authStateSubject.next(state);
  }
  clear() {
    this.authStateSubject.next({ id: null, email: null, accessToken: null, givenName: null, surname: null });
  }
  getToken() {
    return this.authStateSubject.pipe(map(state => state.accessToken));
  }

}