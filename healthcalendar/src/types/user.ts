export type Role = 'Patient' | 'Worker' | 'Usermanager';

// Common fields for all user types
export interface BaseUser {
  sub: string;       
  name: string;   
  nameid: string; 
  role: Role;        
  jti: string; 
  iat: number;     
  exp?: number;       
}

export interface PatientUser extends BaseUser {
  role: 'Patient';
  WorkerId: string;
}

// Worker and UserManager tokens do not include WorkerId.

export interface WorkerUser extends BaseUser {
  role: 'Worker' | 'Usermanager';
}

// Union type that covers all JWT users
export type JwtUser = PatientUser | WorkerUser;