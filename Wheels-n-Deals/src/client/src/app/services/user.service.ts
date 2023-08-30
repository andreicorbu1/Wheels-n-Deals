import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { Observable } from 'rxjs';
import { RegisterDto } from '../models/Dto/registerDto';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private readonly baseUrl: string = 'http://localhost:8088/api/Users';
  readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + sessionStorage.getItem('token'),
    }),
  };

  constructor(private httpClient: HttpClient) {}

  getUserById(id: string): Observable<User> {
    const url = `${this.baseUrl}/${id}`;
    return this.httpClient.get<User>(url, this.httpOptions);
  }

  deleteUser(id: string): Observable<User> {
    return this.httpClient.delete<User>(
      `${this.baseUrl}?userId=${id}`,
      this.httpOptions
    );
  }

  updateUser(id: string, announcement: RegisterDto): Observable<User> {
    return this.httpClient.put<User>(
      `${this.baseUrl}?userId=${id}`,
      announcement,
      this.httpOptions
    );
  }
}
