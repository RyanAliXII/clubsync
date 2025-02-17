import { Component, Input } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { RippleModule } from 'primeng/ripple';
import { AvatarModule } from "primeng/avatar"
import { MenuModule } from 'primeng/menu';
import { CommonModule } from '@angular/common';
import { UserData } from '#core/services/auth/auth-state.service';
import { Observable, of } from 'rxjs';
@Component({
    selector: 'sidebar',
    templateUrl: './sidebar.component.html',
    standalone: true,
    imports: [MenuModule, RippleModule, AvatarModule, CommonModule]
})
export class SidebarComponent {
    @Input()
    items: MenuItem[] = []
    @Input()
    user: Observable<UserData | null> = of(null);
}
