import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { PrimeIcons } from "primeng/api"
import { AuthManagerService } from '#core/services/auth/auth-manager.service';
import { UserData } from '#core/services/auth/auth-state.service';
import { Observable, of } from 'rxjs';
import { SidebarComponent } from './sidebar.component';
@Component({
    selector: 'main-layout',
    templateUrl: './main-layout.component.html',
    standalone: true,
    imports: [RouterModule, SidebarComponent],
})
export class MainLayoutComponent {
    user$: Observable<UserData | null> = of(null)
    constructor(private authManagerService: AuthManagerService) {
        this.user$ = authManagerService.getUser();
    }
    menuItems: MenuItem[] = [
        {
            title: "dashboard",
            label: "Dashboard",
            icon: PrimeIcons.HOME,
            iconClass: PrimeIcons.HOME
        }
    ]
}
