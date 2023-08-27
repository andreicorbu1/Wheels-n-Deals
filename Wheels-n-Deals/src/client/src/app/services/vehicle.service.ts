import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AddVehicleDto } from '../models/Dto/add-vehicle-dto';
import { Vehicle } from '../models/vehicle';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {
  private readonly baseUrl: string = 'http://localhost:7250/api/Vehicles/';
  readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + sessionStorage.getItem('token'),
    }),
  };
  constructor(private httpClient: HttpClient) {}

  addVehicle(vehicle: AddVehicleDto) {
    return this.httpClient.post<Vehicle>(
      this.baseUrl+'add',
      vehicle,
      this.httpOptions
    );
  }
}
