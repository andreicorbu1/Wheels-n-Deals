import { Component } from '@angular/core';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent {
  userIsLoggedIn: boolean = false;

  isAuthenticated(): boolean {
    return localStorage.getItem('token') !== '' && localStorage.getItem('token') !== null;
  }

  logout(): void {
    localStorage.removeItem('token');
  }
}
