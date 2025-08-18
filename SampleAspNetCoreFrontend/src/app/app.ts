import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { NavigationTop } from './ui/navigation-top/navigation-top';
import { NavigationContent } from './ui/navigation-content/navigation-content';
import { Footer } from './ui/footer/footer';
import { Header } from './ui/header/header';
@Component({
  selector: 'app-root',
  imports: [RouterOutlet,NavigationTop,NavigationContent,Header,Footer],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('SampleAspNetCoreFrontend');
}
