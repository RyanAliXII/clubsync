import { ApplicationConfig, inject, provideAppInitializer, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from "@angular/common/http"
import { providePrimeNG } from 'primeng/config';
import { routes } from './app.routes';
import { ClubSyncPreset } from './clubsync.preset';
import { AuthService } from '#core/services/auth/auth.service';
import { AuthStateService } from '#core/services/auth/auth-state.service';
import { AuthManagerService } from '#core/services/auth/auth-manager.service';
import { AuthGuard } from '#core/guards/auth.guard';
import { baseUrlInterceptor } from '#core/interceptors/base-url-interceptor';
import { environment } from '../environments/environment.development';
function initializeApp() {
    const authManager = inject(AuthManagerService);
    return authManager.signInWithRefreshToken();
}
export const appConfig: ApplicationConfig = {
    providers: [
        provideZoneChangeDetection({ eventCoalescing: true }),
        provideRouter(routes),
        provideAnimationsAsync(),
        provideHttpClient(withInterceptors([baseUrlInterceptor])),
        { provide: AuthService },
        { provide: AuthStateService },
        { provide: AuthGuard },

        provideAppInitializer(initializeApp),
        providePrimeNG({
            theme: {
                preset: ClubSyncPreset
            }
        })]
};
