import { LoginDto, RegisterPatientDto } from '../types/auth';

const API_URL = import.meta.env.VITE_API_URL;

export const login = async (credentials: LoginDto): Promise<{ token: string }> => {
  const response = await fetch(`${API_URL}/api/auth/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(credentials),
  });

  if (!response.ok) {
    if (response.status === 401) throw new Error('Invalid email or password.');
    throw new Error('Login failed.');
  }

  // Handle both JSON or plain string token responses
  const text = await response.text();
  try {
    return JSON.parse(text);
  } catch {
    return { token: text };
  }
};

export const registerPatient = async (userData: RegisterPatientDto): Promise<any> => {
  const response = await fetch(`${API_URL}/api/auth/registerPatient`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(userData),
  });

  if (!response.ok) {
    const message = await response.text();
    throw new Error(message || 'Registration failed.');
  }

  return response.text();
};

export const logout = (): void => {
  localStorage.removeItem('token');
};
