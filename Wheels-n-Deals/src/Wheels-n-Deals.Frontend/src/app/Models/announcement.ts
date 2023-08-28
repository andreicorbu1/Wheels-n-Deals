import { User } from './user';
import { Vehicle } from './vehicle';
export interface Announcement {
  id: string,
  title: string,
  description: string,
  county: string,
  city: string,
  dateCreated: string,
  dateModified: string,
  images: string[],
  vehicle: Vehicle
  user: User
}
