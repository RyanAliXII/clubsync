import { HttpEvent, HttpHandlerFn, HttpRequest } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "../../../environments/environment.development";

export function baseUrlInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
    const baseUrl = environment.apiBaseUrlV1;
    const newRequest = req.clone({
        url: `${baseUrl}${req.url}`
      });
    return next(newRequest);
  }