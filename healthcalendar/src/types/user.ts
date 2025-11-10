export type Role = 'Patient' | 'Worker' | 'Admin';

/**
 * Common fields for all user types.
 * These match your backend's JWT token claims.
 */
export interface BaseUser {
  sub: string;        // Name of the user
  email: string;      // Email of the user
  nameid: string;     // User ID
  role: Role;         // Role claim
  jti: string;        // Token ID
  iat: number;        // Issued-at timestamp
  exp?: number;       // Optional: expiration timestamp if backend includes it
}

/**
 * Patient tokens always include an extra field: WorkerId.
 */
export interface PatientUser extends BaseUser {
  role: 'Patient';
  WorkerId: string;   // Related worker's ID
}

/**
 * Worker and Admin tokens do not include WorkerId.
 */
export interface WorkerUser extends BaseUser {
  role: 'Worker' | 'Admin';
}

/**
 * Union type that covers all possible JWT users.
 */
export type JwtUser = PatientUser | WorkerUser;
