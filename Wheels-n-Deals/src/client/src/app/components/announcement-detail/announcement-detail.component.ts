import { User } from 'src/app/models/user';
import { Component, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Announcement } from 'src/app/models/announcement';
import { JwtClaims } from 'src/app/models/jwt-claims';
import { AnnouncementService } from 'src/app/services/announcement.service';
import jwt_decode from 'jwt-decode';
import { Role } from 'src/app/models/enums/role';
import { UserService } from 'src/app/services/user.service';
import { VehicleService } from 'src/app/services/vehicle.service';

@Component({
  selector: 'app-announcement-detail',
  templateUrl: './announcement-detail.component.html',
  styleUrls: ['./announcement-detail.component.scss'],
})
export class AnnouncementDetailComponent {
  announcement: Announcement = null;
  currentImageIndex: number = 0;
  currentImage: string | undefined;
  canBeRenewed: boolean = false;
  isUserAdmin: boolean = false;
  isUserOwner: boolean = false;
  constructor(
    private route: ActivatedRoute,
    private announcementService: AnnouncementService,
    private userService: UserService,
    private vehicleService: VehicleService
  ) {}
  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      const oneDayInMilliseconds = 24 * 60 * 60 * 1000;
      const announcementId = params.get('id');
      if (announcementId) {
        this.announcementService
          .getAnnouncementById(announcementId)
          .subscribe((announcement) => {
            this.announcement = announcement;
            this.updateCurrentImage();
            const strToken: string = sessionStorage.getItem('token');
            if (
              strToken === null ||
              strToken === undefined ||
              strToken === ''
            ) {
              this.isUserAdmin = this.isUserOwner = false;
              return;
            }
            const dateModified = new Date(this.announcement.dateModified);
            this.canBeRenewed =
              Date.now() - dateModified.getTime() >= oneDayInMilliseconds;
            const token: JwtClaims = jwt_decode(
              sessionStorage.getItem('token')
            );
            this.isUserOwner = token.nameid === this.announcement.user.id;
            if (!this.isUserOwner) {
              this.userService
                .getUserById(token.nameid)
                .subscribe((user: User) => {
                  this.isUserAdmin = user.role.toString() === 'Admin';
                });
            }
          });
      }
    });
  }

  updateCurrentImage(): void {
    if (this.announcement.images.length > 0) {
      this.currentImage = this.announcement.images[this.currentImageIndex];
    } else {
      this.currentImage = undefined;
    }
  }

  renewAnnouncement(): void {
    this.announcementService.renewAnnouncement(this.announcement.id).subscribe({
      next: (response) => {
        window.location.reload();
      },
      error: (error) => {
        console.log(error);
      },
    });
  }

  nextImage(): void {
    this.currentImageIndex =
      (this.currentImageIndex + 1) % this.announcement.images.length;
    this.updateCurrentImage();
  }

  prevImage(): void {
    this.currentImageIndex =
      (this.currentImageIndex - 1 + this.announcement.images.length) %
      this.announcement.images.length;
    this.updateCurrentImage();
  }

  deleteAnnouncement(): void {
    this.announcementService.deleteAnnouncementById(this.announcement.id);
    this.vehicleService
      .deleteVehicle(this.announcement.vehicle.vinNumber)
      .subscribe({
        next: (response) => {
          window.location.href = '/';
        },
        error: (error) => {
          console.log(error);
        },
      });
  }

  editAnnouncement(): void {
    window.location.href = '/announcement/edit/' + this.announcement.id;
  }
}
