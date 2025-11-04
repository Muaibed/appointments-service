import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, FormsModule],
  template: `
    <h1 class="title">Appointments Service</h1>
    <router-outlet></router-outlet>
  `,
  styles: [`.title { text-align: center; margin: 20px; font-family: sans-serif; }`]
})
export class AppComponent {}
