import { Component, ChangeDetectionStrategy } from '@angular/core';
import { ThemeService } from './services/Theme/theme.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class AppComponent {
  title = 'SAIH-Frontend';

  constructor(private themeService: ThemeService) { }
}
