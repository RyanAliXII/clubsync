import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
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

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes),
    provideAnimationsAsync(),
    provideHttpClient(),
    {provide: AuthService},
    {provide: AuthStateService},
    {provide: AuthGuard},
    {provide: AuthManagerService},
    providePrimeNG({
        theme: {
            preset: ClubSyncPreset
        }
    })]
};
