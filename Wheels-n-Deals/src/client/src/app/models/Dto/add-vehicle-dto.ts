import { Fuel } from "../enums/fuel";
import { Gearbox } from "../enums/gearbox";
import { State } from "../enums/state";

export interface AddVehicleDto {
  vinNumber: string,
  make: string,
  model: string,
  year: number,
  mileage: number,
  technicalState: State,
  priceCurrency: string,
  price: number,
  carBody: string,
  engineSize: number,
  fuelType: Fuel,
  gearbox: Gearbox,
  horsePower: number
}
