import { Announcement } from '../../models/announcement';
import { Component, Input } from '@angular/core';
import { AnnouncementService } from '../../services/announcement.service';

@Component({
  selector: 'app-announcement',
  templateUrl: './announcement.component.html',
  styleUrls: ['./announcement.component.scss'],
})
export class AnnouncementComponent {
  @Input()
  announcement: Announcement;
  constructor(private announcementService: AnnouncementService) {}
}
