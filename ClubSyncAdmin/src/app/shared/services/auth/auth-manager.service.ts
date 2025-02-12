import { map, of, tap } from "rxjs";
import { AuthStateService } from "./auth-state.service";
import { AuthService, Credentials } from "./auth.service";
import { Injectable } from "@angular/core";
@Injectable({ providedIn: "root" })
export class AuthManagerService {
    constructor(private authService: AuthService, private authStateService: AuthStateService){}
    signIn(credentials: Credentials){
        return this.authService.signIn(credentials).pipe(tap((result)=>{
            if(!result.isSuccess || !result.user){
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
    signInWithRefreshToken(){
        return this.authService.signInWithRefreshToken().pipe(tap((result)=>{
            if(!result.isSuccess || !result.user){
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
    isAuth(){
        const token = this.authStateService.getToken();
        return !!token;
    }
    
    
}