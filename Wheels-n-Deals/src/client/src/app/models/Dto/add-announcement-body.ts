import { AddAnnouncementDto } from "./add-announcement-dto";
import { AddVehicleDto } from "./add-vehicle-dto";

export interface AddAnnouncementBody {
  announcement: AddAnnouncementDto,
  vehicle: AddVehicleDto
}
