import { Component } from '@angular/core';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent {
  userIsLoggedIn: boolean = false;

  isAuthenticated(): boolean {
    const hasToken: boolean = sessionStorage.getItem('token') !== '' && sessionStorage.getItem('token') !== null;

    return hasToken
  }

  logout(): void {
    sessionStorage.removeItem('token');
  }
}
