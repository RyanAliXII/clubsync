import { Routes } from '@angular/router';
import { LoginComponent } from '#modules/login/pages/login.component';
import { DashboardComponent } from '#modules/dashboard/pages/dashboard.component';
import { AuthGuard } from '#core/guards/auth.guard';
import { MainLayoutComponent } from '#shared/layouts/main/main-layout.component';
export const routes: Routes = [
    {
        path: "", component: LoginComponent

    }
    , {
        path: "", component: MainLayoutComponent, canActivate: [AuthGuard],
        children: [
            {
                path: "dashboard", component: DashboardComponent
            }
        ]
    }
];
