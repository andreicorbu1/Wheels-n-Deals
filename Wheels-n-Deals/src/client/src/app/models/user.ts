import { Role } from "./enums/role";

export interface User {
  id: string,
  firstName:string,
  lastName: string,
  email: string,
  phoneNumber: string,
  address: string,
  role: Role,
  dateCreated: string,
  dateModified: string
}
