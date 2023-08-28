import { Gearbox } from './enums/gearbox';
import { Fuel } from './enums/fuel';
import { State } from './enums/state';

export interface Vehicle {
  id: string;
  vinNumber: string;
  make: string;
  model: string;
  year: number;
  mileage: number;
  priceInEuro: number;
  priceInRon: number;
  technicalState: State;
  carBody: string;
  fuelType: Fuel;
  engineSize: number;
  gearbox: Gearbox;
  horsePower: number;
}
