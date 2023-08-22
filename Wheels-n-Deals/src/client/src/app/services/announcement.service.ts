import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import {
  HttpClient,
  HttpHeaderResponse,
  HttpHeaders,
} from '@angular/common/http';
import { Announcement } from '../models/announcement';

@Injectable({
  providedIn: 'root'
})
export class AnnouncementService {
  private readonly baseUrl: string = 'http://localhost:7250/api/Announcement/';
  readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + localStorage.getItem('token'),
    }),
  };
  constructor(private httpClient: HttpClient) {}

  getAnnouncements(): Observable<Announcement[]> {
    console.log('Called');
    return this.httpClient.get<Announcement[]>(this.baseUrl+'getall', this.httpOptions);
  }

  getAnnouncementsByUserId(id: string): Observable<Announcement[]> {
    return this.httpClient.get<Announcement[]>(
      `${this.baseUrl}userid=${id}`,
      this.httpOptions
    );
  }

  getAnnouncementById(id: string): Observable<Announcement> {
    return this.httpClient.get<Announcement>(
      `${this.baseUrl}${id}`,
      this.httpOptions
    );
  }

  deleteAnnouncementById(id: string): Observable<Announcement> {
    return this.httpClient.delete<Announcement>(
      `${this.baseUrl}${id}`,
      this.httpOptions
    );
  }
}
