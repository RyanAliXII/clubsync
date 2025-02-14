import { Injectable } from "@angular/core";
import { HttpClient, HttpErrorResponse, HttpResponse } from "@angular/common/http";
import { catchError, map } from "rxjs/operators";
import { Observable, of } from "rxjs";
import { UserData } from "./auth-state.service";

export type Credentials = {
  email: string;
  password: string;
};

export type SignInApiResponse = {
  message: string;
  accessToken?: string;
  refreshToken?: string;
  user?: UserData;
};

export type SignInResult = {
  message: string;
  isSuccess: boolean;
  accessToken: string | null;
  refreshToken: string | null;
  user: UserData | null;
};

@Injectable({ providedIn: "root" })
export class AuthService {
  constructor(private http: HttpClient) { }
  signIn(credentials: Credentials): Observable<SignInResult> {
    return this.http
      .post<SignInApiResponse>("/sign-in", credentials, { observe: "response", withCredentials: true })
      .pipe(
        map((response: HttpResponse<SignInApiResponse>) => ({
          message: response.body?.message || "",
          isSuccess: true,
          accessToken: response.body?.accessToken || null,
          refreshToken: response.body?.refreshToken || null,
          user: response.body?.user
            ? {
              givenName: response.body.user.givenName,
              surname: response.body.user.surname,
              id: response.body.user.id,
              email: response.body.user.email
            }
            : null,
        })),
        catchError((error: HttpErrorResponse) =>
          of({
            message: error.error?.message ?? "",
            isSuccess: false,
            accessToken: null,
            refreshToken: null,
            user: null,
          })
        )
      );
  }
  signInWithRefreshToken(): Observable<SignInResult> {
    return this.http
      .post<SignInApiResponse>("/refresh-token", null, { observe: "response", withCredentials: true })
      .pipe(
        map((response: HttpResponse<SignInApiResponse>) => ({
          message: response.body?.message || "",
          isSuccess: true,
          accessToken: response.body?.accessToken || null,
          refreshToken: response.body?.refreshToken || null,
          user: response.body?.user
            ? {
              givenName: response.body.user.givenName,
              surname: response.body.user.surname,
              id: response.body.user.id,
              email: response.body.user.email
            }
            : null,
        })),
        catchError((error: HttpErrorResponse) =>
          of({
            message: error.error?.message ?? "",
            isSuccess: false,
            accessToken: null,
            refreshToken: null,
            user: null,
          })
        )
      );
  }
}
