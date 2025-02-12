import { ApplicationConfig, inject, provideAppInitializer, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import {provideHttpClient} from "@angular/common/http"
import { providePrimeNG } from 'primeng/config';
import { routes } from './app.routes';
import { ClubSyncPreset } from './clubsync.preset';
import { AuthService } from './shared/services/auth/auth.service';
import { AuthStateService } from './shared/services/auth/auth-state.service';
import { AuthManagerService } from './shared/services/auth/auth-manager.service';
import { AuthGuard } from './guards/auth.guard';
import { lastValueFrom } from 'rxjs';

function initializeApp() {
    const authManager = inject(AuthManagerService);
    return authManager.signInWithRefreshToken();
}
export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes),
    provideAnimationsAsync(),
    provideHttpClient(),
    {provide: AuthService},
    {provide: AuthStateService},
    {provide: AuthGuard},
    provideAppInitializer(initializeApp),
    providePrimeNG({
        theme: {
            preset: ClubSyncPreset
        }
    })]
};
