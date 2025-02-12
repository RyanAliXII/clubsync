import { Component } from '@angular/core';
import { InputTextModule } from 'primeng/inputtext';

import { AuthStateService } from '../../../shared/services/auth/auth-state.service';

@Component({
  selector: 'dashboard-module',
  standalone: true,
  templateUrl: "./dashboard.component.html",
})
export class DashboardComponent{
 
  constructor( private authStateService: AuthStateService){

  }
  ngOnInit(){
    console.log(this.authStateService.user$);
  }
 
}
