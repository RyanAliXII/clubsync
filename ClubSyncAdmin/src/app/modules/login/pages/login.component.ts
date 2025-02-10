import { Component } from '@angular/core';
import { InputTextModule } from 'primeng/inputtext';
import {ButtonModule} from "primeng/button"
@Component({
  selector: 'login-module',
  standalone: true,
  imports: [InputTextModule, ButtonModule],
  templateUrl: "./login.component.html",
})
export class LoginComponent{}
