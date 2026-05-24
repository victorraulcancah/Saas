import { createContext, useContext, useState, useEffect, type ReactNode } from 'react';
import api from './api';
import type { LoginRequest, LoginResponse } from './types';

interface AuthState {
  token: string;
  email: string;
  role: string;
  tenantId: string | null;
}

interface AuthContextType extends AuthState {
  login: (data: LoginRequest) => Promise<void>;
  logout: () => void;
  isSuperAdmin: boolean;
}

const AuthContext = createContext<AuthContextType | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [auth, setAuth] = useState<AuthState>(() => {
    const stored = localStorage.getItem('auth');
    return stored ? JSON.parse(stored) : { token: '', email: '', role: '', tenantId: null };
  });

  useEffect(() => {
    if (auth.token) localStorage.setItem('auth', JSON.stringify(auth));
    else localStorage.removeItem('auth');
  }, [auth]);

  const login = async (data: LoginRequest) => {
    const res = await api.post<LoginResponse>('/auth/login', data);
    const { token, email, role, tenantId } = res.data;
    localStorage.setItem('token', token);
    setAuth({ token, email, role, tenantId });
  };

  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('auth');
    setAuth({ token: '', email: '', role: '', tenantId: null });
  };

  return (
    <AuthContext.Provider value={{ ...auth, login, logout, isSuperAdmin: auth.role === 'SuperAdmin' }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth must be inside AuthProvider');
  return ctx;
}
