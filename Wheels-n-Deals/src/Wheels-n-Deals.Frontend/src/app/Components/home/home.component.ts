import { Component } from '@angular/core';
import { Announcement } from 'src/app/Models/announcement';
import { AnnouncementService } from 'src/app/Services/announcement.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.sass']
})
export class HomeComponent {
  title = 'Wheels-n-Deals';
  announcements: Announcement[] = [];

  constructor(private announcementService: AnnouncementService) {}

  ngOnInit(): void {
    this.announcementService.getAnnouncements().subscribe((announcements) => (this.announcements = announcements));
    console.log(this.announcements);
  }
}
