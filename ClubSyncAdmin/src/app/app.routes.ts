import { Routes } from '@angular/router';
import { LoginComponent } from '#modules/login/pages/login.component';
import { DashboardComponent } from '#modules/dashboard/pages/dashboard.component';
import { AuthGuard } from '#core/guards/auth.guard';
export const routes: Routes = [
    {
        path: "", component: LoginComponent

    }
    , {
        path: "dashboard", component: DashboardComponent, canActivate: [AuthGuard]
    }
];
