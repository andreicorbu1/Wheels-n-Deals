import { Vehicle } from './../../models/vehicle';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AddAnnouncementDto } from 'src/app/models/Dto/add-announcement-dto';
import { AddVehicleDto } from 'src/app/models/Dto/add-vehicle-dto';
import { VehicleService } from 'src/app/services/vehicle.service';
import { ImageDto } from './../../models/Dto/imageDto';
import { AnnouncementService } from 'src/app/services/announcement.service';
import { Announcement } from 'src/app/models/announcement';

@Component({
  selector: 'app-add-announcement',
  templateUrl: './add-announcement.component.html',
  styleUrls: ['./add-announcement.component.scss'],
})
export class AddAnnouncementComponent {
  registerForm: FormGroup;
  editMode: boolean = false;
  errorMessage: string | null = null;
  fuelTypes: string[] = ['Petrol', 'Diesel', 'LPG', 'Hybrid', 'Electricity'];
  constructor(
    private fb: FormBuilder,
    private vehicleService: VehicleService,
    private route: ActivatedRoute,
    private announcementService: AnnouncementService
  ) {
    this.registerForm = this.fb.group({
      title: ['', Validators.required],
      images: ['', Validators.required],
      description: ['', Validators.required],
      priceValue: ['', Validators.required],
      priceCurrency: ['', Validators.required],
      city: ['', Validators.required],
      county: ['', Validators.required],
      vin: ['', Validators.required],
      make: ['', Validators.required],
      model: ['', Validators.required],
      year: ['', Validators.required],
      engineSize: ['', Validators.required],
      horsePower: ['', Validators.required],
      fuelType: ['', Validators.required],
      carBody: ['', Validators.required],
      mileage: ['', Validators.required],
      technicalState: ['', Validators.required],
      gearbox: ['', Validators.required],
    });

    this.route.params.subscribe((params) => {
      if (params['id']) {
        this.editMode = true;
        this.loadDataForEdit(params['id']);
      }
    });
  }

  loadDataForEdit(id: string) {
    this.announcementService
      .getAnnouncementById(id)
      .subscribe((announcement: Announcement) => {
        this.registerForm.patchValue({
          title: announcement.title,
          images: announcement.images.map((image: string) => image).join(','),
          description: announcement.description,
          priceValue: announcement.vehicle.priceInEuro,
          priceCurrency: 'EURO',
          city: announcement.city,
          county: announcement.county,
          vin: announcement.vehicle.vinNumber,
          make: announcement.vehicle.make,
          model: announcement.vehicle.model,
          year: announcement.vehicle.year,
          engineSize: announcement.vehicle.engineSize,
          horsePower: announcement.vehicle.horsePower,
          fuelType: announcement.vehicle.fuelType,
          carBody: announcement.vehicle.carBody,
          mileage: announcement.vehicle.mileage,
          technicalState: announcement.vehicle.technicalState,
          gearbox: announcement.vehicle.gearbox,
        });
      });
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      const registerData = this.registerForm.value;
      let price: number;
      if (registerData.priceCurrency === 'EURO') {
        price = registerData.priceValue;
      } else {
        price = registerData.priceValue / 5;
      }
      const vehicle: AddVehicleDto = {
        vinNumber: registerData.vin,
        make: registerData.make,
        model: registerData.model,
        year: registerData.year,
        mileage: registerData.mileage,
        technicalState: registerData.technicalState,
        priceInEuro: price,
        carBody: registerData.carBody,
        engineSize: registerData.engineSize,
        fuelType: registerData.fuelType,
        gearbox: registerData.gearbox,
        horsePower: registerData.horsePower,
      };

      let imagesUrl: ImageDto[] = [];

      const images: string[] = registerData.images.split(',');

      images.forEach((image: string) => {
        image = image.trim();
        const imageDto: ImageDto = {
          imageUrl: image,
        };
        imagesUrl.push(imageDto);
      });

      const announcement: AddAnnouncementDto = {
        title: registerData.title,
        description: registerData.description,
        county: registerData.county,
        city: registerData.city,
        imagesUrl: imagesUrl,
        vinNumber: registerData.vin,
      };
      if (this.editMode === false) {
        this.vehicleService.addVehicle(vehicle).subscribe({
          next: (vehicle: Vehicle) => {
            console.log(vehicle);
            console.log(announcement);
            this.announcementService.addAnnouncement(announcement).subscribe({
              next: (announcement: Announcement) => {
                console.log(announcement);
                window.location.href = '/';
              },
              error: (error) => {
                console.error('Add announcement failed:', error);
                this.errorMessage = error;
              },
            });
          },
          error: (error) => {
            console.error('Add vehicle failed:', error);
            this.errorMessage = error;
          },
        });
      } else {
        this.announcementService
          .getAnnouncementById(this.route.snapshot.params['id'])
          .subscribe((announcementDb: Announcement) => {
            this.vehicleService
              .updateVehicle(announcementDb.vehicle.id, vehicle)
              .subscribe({
                next: (vehicle: Vehicle) => {
                  console.log(vehicle);
                  this.announcementService
                    .updateAnnouncement(
                      this.route.snapshot.params['id'],
                      announcement
                    )
                    .subscribe((announcement: Announcement) => {
                      console.log(announcement);
                    });
                  window.location.href = '/';
                },
                error: (error) => {
                  console.error('Update announcement failed:', error);
                  this.errorMessage = error.error.message;
                },
              });
          });
      }
    }
  }
}
