import { Injectable } from "@angular/core";
import { HttpClient, HttpErrorResponse, HttpResponse } from "@angular/common/http";
import { catchError, map } from "rxjs/operators";
import { Observable, of } from "rxjs";

type Credentials = {
  email: string;
  password: string;
};
type UserData = {
  id: string;
  givenName: string;
  surname: string;
  email: string;
};
type SignInApiResponse = {
  message: string;
  accessToken?: string;
  refreshToken?: string;
  user?: UserData;
};

type SignInResult = {
  message: string;
  isSuccess: boolean;
  accessToken: string | null;
  refreshToken: string | null;
  user: UserData | null;
};

@Injectable({ providedIn: "root" })
export class AuthService {
  private url: string = "http://localhost:4003/api/admin/v1/sign-in";

  constructor(private http: HttpClient) {}

  signIn(credentials: Credentials): Observable<SignInResult> {
    return this.http
      .post<SignInApiResponse>(this.url, credentials, { observe: "response", withCredentials: true })
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
