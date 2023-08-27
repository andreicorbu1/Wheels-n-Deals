import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import {
  HttpClient,
  HttpHeaderResponse,
  HttpHeaders,
} from '@angular/common/http';
import { Announcement } from '../models/announcement';
import { AddAnnouncementDto } from '../models/Dto/add-announcement-dto';

@Injectable({
  providedIn: 'root',
})
export class AnnouncementService {
  private readonly baseUrl: string = 'http://localhost:7250/api/Announcement/';
  readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + sessionStorage.getItem('token'),
    }),
  };
  constructor(private httpClient: HttpClient) {}

  getAnnouncements(): Observable<Announcement[]> {
    console.log('Called');
    return this.httpClient.get<Announcement[]>(
      this.baseUrl + 'getall',
      this.httpOptions
    );
  }

  getAnnouncementsWithFilters(filters: string): Observable<Announcement[]> {
    return this.httpClient.get<Announcement[]>(
      this.baseUrl + 'getall?' + filters,
      this.httpOptions
    );
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

  addAnnouncement(announcement: AddAnnouncementDto): Observable<Announcement> {
    return this.httpClient.post<Announcement>(
      this.baseUrl + 'add',
      announcement,
      this.httpOptions
    );
  }

  updateAnnouncement(
    id: string,
    announcement: AddAnnouncementDto
  ): Observable<Announcement> {
    return this.httpClient.put<Announcement>(
      `${this.baseUrl}${id}`,
      announcement,
      this.httpOptions
    );
  }
}
