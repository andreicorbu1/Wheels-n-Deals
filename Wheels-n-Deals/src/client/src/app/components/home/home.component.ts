import { Component } from '@angular/core';
import { Announcement } from 'src/app/models/announcement';
import { AnnouncementService } from 'src/app/services/announcement.service';
import { FormsModule } from '@angular/forms'; // Import FormsModule

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent {
  showFilterDropdown: boolean = false;
  title = 'Wheels-n-Deals';
  announcements: Announcement[] = [];
  selectedMake: string = '';
  selectedModel: string = '';
  minYear: number | null = null;
  maxYear: number | null = null;
  minMileage: number | null = null;
  maxMileage: number | null = null;
  minPrice: number | null = null;
  maxPrice: number | null = null;
  selectedCarBody: string = '';
  minEngineSize: number | null = null;
  maxEngineSize: number | null = null;
  selectedFuelType: string = '';
  selectedGearbox: string = '';
  minHorsePower: number | null = null;
  maxHorsePower: number | null = null;
  constructor(private announcementService: AnnouncementService) {}

  ngOnInit(): void {
    this.announcementService
      .getAnnouncements()
      .subscribe((announcements) => (this.announcements = announcements));
  }

  private gearboxMapping = {
    manual: 1,
    automatic: 2,
  };

  private fuelTypeMapping = {
    petrol: 1,
    diesel: 2,
    lpg: 3,
    hybrid: 4,
    electricity: 5,
  };
  applyFilters(): void {
    this.toggleFilterDropdown();
    const makes: string[] = this.selectedMake
      .split(/,\s*/)
      .filter((make) => !!make);
    const models: string[] = this.selectedModel
      .split(/,\s*/)
      .filter((model) => !!model);
    const carBodies: string[] = this.selectedCarBody
      .split(/,\s*/)
      .filter((carBody) => !!carBody);
    const gearboxes = this.selectedGearbox
      .split(/,\s*/)
      .filter((gearbox) => !!gearbox)
      .map(
        (gearbox) => this.gearboxMapping[gearbox.trim().toLowerCase()] || []
      );
    const fuelTypes = this.selectedFuelType
      .split(/,\s*/)
      .filter((fuel) => !!fuel)
      .map(
        (fuelType) => this.fuelTypeMapping[fuelType.trim().toLowerCase()] || []
      );

    const queryParams: string[] = [];

    if (makes.length > 0) {
      queryParams.push(
        ...makes.map((make) => `Make=${encodeURIComponent(make)}`)
      );
    }

    if (models.length > 0) {
      queryParams.push(
        ...models.map((model) => `Model=${encodeURIComponent(model)}`)
      );
    }

    if (carBodies.length > 0) {
      queryParams.push(
        ...carBodies.map((carBody) => `CarBody=${encodeURIComponent(carBody)}`)
      );
    }

    if (gearboxes.length > 0) {
      queryParams.push(...gearboxes.map((gearbox) => `Gearbox=${gearbox}`));
    }

    if (fuelTypes.length > 0) {
      queryParams.push(...fuelTypes.map((fuelType) => `FuelType=${fuelType}`));
    }

    if (this.minYear !== null) {
      queryParams.push(`MinYear=${this.minYear}`);
    }

    if (this.maxYear !== null) {
      queryParams.push(`MaxYear=${this.maxYear}`);
    }

    if (this.minMileage !== null) {
      queryParams.push(`MinMileage=${this.minMileage}`);
    }

    if (this.maxMileage !== null) {
      queryParams.push(`MaxMileage=${this.maxMileage}`);
    }

    if (this.minPrice !== null) {
      queryParams.push(`MinPrice=${this.minPrice}`);
    }

    if (this.maxPrice !== null) {
      queryParams.push(`MaxPrice=${this.maxPrice}`);
    }

    if (this.minEngineSize !== null) {
      queryParams.push(`MinEngineSize=${this.minEngineSize}`);
    }

    if (this.maxEngineSize !== null) {
      queryParams.push(`MaxEngineSize=${this.maxEngineSize}`);
    }

    if (this.minHorsePower !== null) {
      queryParams.push(`MinHorsePower=${this.minHorsePower}`);
    }

    if (this.maxHorsePower !== null) {
      queryParams.push(`MaxHorsePower=${this.maxHorsePower}`);
    }

    const queryString = queryParams.join('&');

    this.announcementService
      .getAnnouncementsWithFilters(queryString)
      .subscribe((announcements) => {
        this.announcements = announcements;
        console.log(announcements);
      });
  }

  toggleFilterDropdown() {
    this.showFilterDropdown = !this.showFilterDropdown;
  }
}
