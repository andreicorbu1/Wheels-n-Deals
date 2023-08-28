import { Gearbox } from './Enums/gearbox';
import { Fuel } from "./Enums/fuel";
import { State } from "./Enums/state";

export interface Vehicle {
  id: string,
  vinNumber: string,
  make: string,
  model: string,
  year: number,
  mileage: number,
  priceInEuro: number,
  priceInRon: number,
  technicalState: State,
  carBody: string,
  fuel: Fuel,
  engineSize: number,
  gearbox: Gearbox,
  horsePower: number
}
