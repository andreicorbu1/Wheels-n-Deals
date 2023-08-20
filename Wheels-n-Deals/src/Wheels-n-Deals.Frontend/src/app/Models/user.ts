import { Role } from "./Enums/role";

export interface User {
  id: string,
  firstName: string,
  lastName: string,
  email: string,
  phoneNumber: string,
  address: string,
  role: Role,
  dateCreated: Date,
  dateModified: Date
}
