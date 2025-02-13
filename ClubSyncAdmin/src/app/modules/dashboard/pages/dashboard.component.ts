import { Component } from '@angular/core';
import { AuthManagerService } from '#core/services/auth/auth-manager.service';
import { ButtonModule } from 'primeng/button';
@Component({
  selector: 'dashboard-module',
  standalone: true,
  templateUrl: "./dashboard.component.html",
  imports: [ButtonModule]

})
export class DashboardComponent {

  constructor(private authManager: AuthManagerService) {

  }
  ngOnInit() {

  }
  async execute() {
    console.log("execute")
    const t = await this.authManager.getToken()
    console.log(t)
  }

}
