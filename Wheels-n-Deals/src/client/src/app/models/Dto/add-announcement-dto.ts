import { ImageDto } from "./imageDto";

export interface AddAnnouncementDto {
  title: string,
  description: string,
  county: string,
  city: string,
  imagesUrl: ImageDto[],
  vinNumber: string,
}
