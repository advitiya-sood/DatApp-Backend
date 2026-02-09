import { createContext, useContext, useMemo, useState } from 'react';

const AuthContext = createContext(null);

function decodeToken(token) {
  if (!token) return null;
  try {
    const payloadPart = token.split('.')[1];
    const decoded = JSON.parse(atob(payloadPart));
    const id =
      decoded.nameid ||
      decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
    const username =
      decoded.unique_name ||
      decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];

    return {
      id: id ? Number(id) : null,
      username: username || null
    };
  } catch {
    return null;
  }
}

export function AuthProvider({ children }) {
  const [token, setToken] = useState(() => localStorage.getItem('datapp_token') || '');

  const login = (newToken) => {
    setToken(newToken);
    localStorage.setItem('datapp_token', newToken);
  };

  const logout = () => {
    setToken('');
    localStorage.removeItem('datapp_token');
  };

  const value = useMemo(() => {
    const user = decodeToken(token);
    return {
      token,
      isAuthenticated: Boolean(token),
      user,
      userId: user?.id ?? null,
      username: user?.username ?? null,
      login,
      logout
    };
  }, [token]);

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return ctx;
}

