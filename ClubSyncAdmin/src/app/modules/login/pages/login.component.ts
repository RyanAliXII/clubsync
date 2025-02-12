import { Component } from '@angular/core';
import { InputTextModule } from 'primeng/inputtext';
import {ButtonModule} from "primeng/button"
import {FormsModule} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthManagerService } from '../../../shared/services/auth/auth-manager.service';
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
  constructor(private authManageService: AuthManagerService, private router: Router){}
 
  onSubmit(){
      this.authManageService.signIn(this.credentials).subscribe(result=>{
        if(result.isSuccess){
            this.router.navigate(["/dashboard"])
        }
      })
  }
}
