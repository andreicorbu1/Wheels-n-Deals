import { HttpClient, HttpHeaders } from '@angular/common/http';
import { RegisterDto } from '../models/Dto/registerDto';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly baseUrl: string = 'http://localhost:7250/api/Users/';
  readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + sessionStorage.getItem('token'),
    }),
  };
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();
  constructor(private httpClient: HttpClient) {}

  setAuthenticated(isAuthenticated: boolean): void {
    this.isAuthenticatedSubject.next(isAuthenticated);
  }

  register(registerDto: RegisterDto): Observable<any> {
    return this.httpClient.post<any>(
      this.baseUrl + 'register',
      registerDto,
      this.httpOptions
    );
  }

  login(loginData: { email: string; password: string }): Observable<any> {
    return this.httpClient.post<any>(this.baseUrl + 'login', loginData);
  }
}
