import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly baseUrl: string = 'http://localhost:7250/api/Users/';
  readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token'),
    }),
  };

  constructor(private httpClient: HttpClient) {}

  getUserById(id: string): Observable<User> {
    const url = `${this.baseUrl}${id}`;
    return this.httpClient.get<User>(url, this.httpOptions);
  }
}
