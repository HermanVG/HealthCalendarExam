
import type { LoginDto, RegisterPatientDto } from '../types/auth';
import type { JwtUser } from '../types/user';
import { jwtDecode } from 'jwt-decode';

// use VITE_API_URL if set, else default to localhost:5080
const API_URL = (import.meta.env.VITE_API_URL as string | undefined) ?? 'http://localhost:5080';

export const login = async (credentials: LoginDto): Promise<{ token: string }> => {
    const response = await fetch(`${API_URL}/api/Auth/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' },
        body: JSON.stringify(credentials),
    });
    if (!response.ok) {
        throw new Error('Login failed');
    }
    const data: any = await response.json();
    // Support both "Token" (PascalCase from backend) and "token" (camelCase)
    const token: string | undefined = data?.token ?? data?.Token;
    if (!token) {
        throw new Error('Login failed');
    }
    return { token };
};

// Separate endpoints; forms enforce roles via AuthContext.

export const registerPatient = async (userData: RegisterPatientDto): Promise<any> => {
    const response = await fetch(`${API_URL}/api/Auth/registerPatient`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' },
        body: JSON.stringify(userData),
    });
    if (!response.ok) {
        try {
            const errorData = await response.json();
            if (Array.isArray(errorData)) {
                const messages = errorData.map((e: any) => e?.description || e?.Description || String(e)).join(', ');
                throw new Error(messages || 'Registration failed');
            }
        } catch {}
        throw new Error('Registration failed');
    }
    return response.json();
};

// Note: Logout is handled in AuthContext by clearing localStorage (hc_token)

// Decode helper separated so context can reuse without re-implementing logic.
export function decodeUser(token: string): JwtUser {
    try {
        const raw: any = jwtDecode(token);
        // Normalize common ASP.NET Core claim types to our frontend shape
        const role = raw.role ?? raw["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
        const nameid = raw.nameid ?? raw["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
        const name = raw.name; // JwtRegisteredClaimNames.Name -> "name"
        const jti = raw.jti;
        const iat = typeof raw.iat === 'string' ? parseInt(raw.iat, 10) : raw.iat;
        const normalized: any = {
            sub: raw.sub,
            name,
            nameid,
            role,
            jti,
            iat,
            exp: raw.exp,
        };
        // Include WorkerId if present (Patient tokens)
        if (typeof raw.WorkerId !== 'undefined') normalized.WorkerId = String(raw.WorkerId);
        return normalized as JwtUser;
    } catch (e) {
        throw new Error('Failed to decode token');
    }
}