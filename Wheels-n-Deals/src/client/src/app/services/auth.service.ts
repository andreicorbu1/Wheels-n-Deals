import { HttpClient, HttpHeaders } from '@angular/common/http';
import { RegisterDto } from './../models/registerDto.enum';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly baseUrl: string = 'http://localhost:7250/api/Users/';
  readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token'),
    }),
  };
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();
  constructor(private httpClient: HttpClient) {}

  setAuthenticated(isAuthenticated: boolean): void {
    this.isAuthenticatedSubject.next(isAuthenticated);
  }

  register(registerDto : RegisterDto): void {
    this.httpClient.post(this.baseUrl+'register', registerDto, this.httpOptions).subscribe(response => {
      console.log(response);
    });
  }

  login(loginData: { email: string; password: string }): Observable<any> {
    return this.httpClient.post<any>(this.baseUrl+'login', loginData);
  }
}
