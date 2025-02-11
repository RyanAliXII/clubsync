import { Component } from '@angular/core';
import { InputTextModule } from 'primeng/inputtext';
import {ButtonModule} from "primeng/button"
import { AuthService } from '../services/auth.service';
import {FormsModule} from '@angular/forms';
import { AuthStateService } from '../../../shared/services/auth-state.service';
import { Router } from '@angular/router';
@Component({
  selector: 'login-module',
  standalone: true,
  imports: [InputTextModule, ButtonModule, FormsModule],
  templateUrl: "./login.component.html",
})
export class LoginComponent{
  credentials = {
    email: "",
    password: ""
  }
  constructor(private authService: AuthService, private authStateService: AuthStateService, private router: Router){}
  onSubmit(){
    this.authService.signIn(this.credentials).subscribe(result=>{
      console.log(result)
        if(!result.isSuccess){
          return;
        }
        if(!result.user){
          return
        }
     
        this.authStateService.setUser({
          accessToken: result.accessToken,
          email: result.user.email,
          id: result.user.id,
          givenName: result.user.givenName,
          surname: result.user.surname,
          isAuth:true,
        })
        this.router.navigate(['/dashboard']);

    })
  }
}
