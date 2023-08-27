import { Vehicle } from './../../models/vehicle';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AddAnnouncementDto } from 'src/app/models/Dto/add-announcement-dto';
import { AddVehicleDto } from 'src/app/models/Dto/add-vehicle-dto';
import { VehicleService } from 'src/app/services/vehicle.service';
import { ImageDto } from "./../../models/Dto/imageDto";
import { AnnouncementService } from 'src/app/services/announcement.service';
import { Announcement } from 'src/app/models/announcement';

@Component({
  selector: 'app-add-announcement',
  templateUrl: './add-announcement.component.html',
  styleUrls: ['./add-announcement.component.scss']
})
export class AddAnnouncementComponent {
  registerForm: FormGroup;
  errorMessage: string | null = null;
  fuelTypes: string[] = ['Petrol', 'Diesel', 'LPG', 'Hybrid', 'Electricity'];

  constructor(private fb: FormBuilder, private vehicleService: VehicleService, private router: Router, private announcementService: AnnouncementService) {
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
      gearbox: ['', Validators.required]
    });

    console.log(this.fuelTypes);
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      const registerData = this.registerForm.value;
      const vehicle: AddVehicleDto = {
        vinNumber: registerData.vin,
        make: registerData.make,
        model: registerData.model,
        year: registerData.year,
        mileage: registerData.mileage,
        technicalState: registerData.technicalState,
        priceCurrency: registerData.priceCurrency,
        price: registerData.priceValue,
        carBody: registerData.carBody,
        engineSize: registerData.engineSize,
        fuelType: registerData.fuelType,
        gearbox: registerData.gearbox,
        horsePower: registerData.horsePower
      };

      let imagesUrl: ImageDto[] = [];

      const images: string[] = registerData.images.split(',');

      images.forEach((image: string) => {
        const imageDto: ImageDto = {
          imageUrl: image
        };
        imagesUrl.push(imageDto);
      });

      const announcement: AddAnnouncementDto = {
        title: registerData.title,
        description: registerData.description,
        county: registerData.county,
        city: registerData.city,
        imagesUrl: imagesUrl,
        vinNumber: registerData.vin
      };

      this.vehicleService.addVehicle(vehicle).subscribe({
        next: (vehicle: Vehicle) => {
          console.log(vehicle);
          this.announcementService.addAnnouncement(announcement).subscribe((announcement: Announcement) => {
            console.log(announcement);
          });
          window.location.href = '/';
        },
        error: (error) => {
          console.error('Registration failed:', error.error);
          this.errorMessage = error.error.message;
        }
      });
    }
  }
}
