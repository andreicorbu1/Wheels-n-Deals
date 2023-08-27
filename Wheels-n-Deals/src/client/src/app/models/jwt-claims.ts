import { Role } from './enums/role';

export interface JwtClaims {
  aud: string;
  email: string;
  exp: number;
  iat: number;
  iss: string;
  nameid: string;
  nbf: number;
  role: Role;
}
