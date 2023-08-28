import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import {
  HttpClient,
  HttpHeaderResponse,
  HttpHeaders,
} from '@angular/common/http';
import { Announcement } from '../Models/announcement';
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
    let json = this.httpClient.get<Announcement[]>(this.baseUrl+'getall', this.httpOptions).subscribe(response => {
      console.log(response);
    }, error => {
      console.error(error);
    });
    return this.httpClient.get<Announcement[]>(this.baseUrl+'getall', this.httpOptions);
  }

  getAnnouncementById(id: string): Observable<Announcement> {
    return this.httpClient.get<Announcement>(
      `${this.baseUrl}/${id}`,
      this.httpOptions
    );
  }
}
