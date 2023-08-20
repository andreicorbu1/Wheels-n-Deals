import { Announcement } from '../../Models/announcement';
import { Component, Input } from '@angular/core';
import { AnnouncementService } from '../../Services/announcement.service';

@Component({
  selector: 'app-announcement',
  templateUrl: './announcement.component.html',
  styleUrls: ['./announcement.component.sass']
})
export class AnnouncementComponent {
  @Input()
  announcement: Announcement;
  constructor(private announcementService: AnnouncementService) { }
}
