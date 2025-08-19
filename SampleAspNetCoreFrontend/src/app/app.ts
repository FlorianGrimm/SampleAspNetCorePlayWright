import { Component, signal, inject } from '@angular/core';
import { RouterModule, Router, RouterOutlet, Routes, RouterLink, RouterLinkActive } from '@angular/router';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { AsyncPipe } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { Observable } from 'rxjs';
import { map, shareReplay, tap } from 'rxjs/operators';

import { AppRoutes } from './app.routes';

@Component({
  selector: 'app-root',
  imports: [
    RouterModule,
    RouterOutlet,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatListModule,
    MatIconModule,
    MatTabsModule,
    AsyncPipe,
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('SampleAspNetCoreFrontend');
  private breakpointObserver = inject(BreakpointObserver);
  private router = inject(Router);

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.XSmall)
    .pipe(
      tap(result => console.log("Breakpoints.Handset", result)),
      map(result => result.matches),
      shareReplay()
    );
  // TODO: how to define the Navigation
  routes: AppRoutes = (this.router.config as AppRoutes).filter(c => true === c.data?.ShowInNavigation);
}
