import { Component } from '@angular/core';
import { Announcement } from 'src/app/models/announcement';
import { AnnouncementService } from 'src/app/services/announcement.service';
import jwt_decode from 'jwt-decode';
import { JwtClaims } from 'src/app/models/jwt-claims';
import { UserService } from 'src/app/services/user.service';
import { User } from 'src/app/models/user';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent {
  announcements: Announcement[] = [];
  user: User;

  constructor(private announcementService: AnnouncementService, private userService: UserService) {}

  ngOnInit(): void {
    const jwt: JwtClaims = jwt_decode(localStorage.getItem('token'));
    console.log(jwt.nameid);
    this.announcementService.getAnnouncementsByUserId(jwt.nameid).subscribe((announcements) => (this.announcements = announcements));
    this.userService.getUserById(jwt.nameid).subscribe((user) => {
      this.user = user;
      console.log(this.user)
    });
  }
}
