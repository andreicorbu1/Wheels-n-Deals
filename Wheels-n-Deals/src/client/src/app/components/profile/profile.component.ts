import { Component } from '@angular/core';
import { Announcement } from 'src/app/models/announcement';
import { AnnouncementService } from 'src/app/services/announcement.service';
import jwt_decode from 'jwt-decode';
import { JwtClaims } from 'src/app/models/jwt-claims';
import { UserService } from 'src/app/services/user.service';
import { User } from 'src/app/models/user';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent {
  announcements: Announcement[] = [];
  user: User;
  isAdmin: boolean = false;
  canEdit: boolean = true;

  constructor(
    private announcementService: AnnouncementService,
    private userService: UserService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    let id: string;

    this.route.params.subscribe((params) => {
      if (params['id']) {
        id = params['id'];
        this.canEdit = false;
      } else {
        const jwt: JwtClaims = jwt_decode(sessionStorage.getItem('token'));
        console.log(jwt.nameid);
        this.userService.getUserById(jwt.nameid).subscribe((user) => {
          this.isAdmin = user.role === 'Admin';
        });
        id = jwt.nameid;
        this.canEdit = true;
      }
    });

    this.announcementService
      .getAnnouncementsByUserId(id)
      .subscribe((announcements) => (this.announcements = announcements));
    this.userService.getUserById(id).subscribe((user) => {
      this.user = user;
      console.log(this.user);
    });
  }

  deleteUser(): void {
    this.userService.deleteUser(this.user.id).subscribe({
      next: (response) => {
        console.log(response);
        sessionStorage.removeItem('token');
        window.location.href = '/';
      },
      error: (error) => {
        console.error('Delete failed:', error.error);
      },
    });
  }
}
