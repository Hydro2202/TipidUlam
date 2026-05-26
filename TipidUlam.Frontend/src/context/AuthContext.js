import React, { createContext, useCallback, useContext, useEffect, useMemo, useState } from 'react';
import { authService, getStoredToken, setStoredToken } from '../services/api';

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [initializing, setInitializing] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [authError, setAuthError] = useState(null);
  const [lastPassword, setLastPassword] = useState(null);

  const clearSession = useCallback(() => {
    setStoredToken(null);
    setUser(null);
  }, []);

  const establishSession = useCallback(async (authResponse) => {
    setStoredToken(authResponse.token);
    setUser(authResponse.user);
    setAuthError(null);
    return authResponse.user;
  }, []);

  const refreshSession = useCallback(async () => {
    setRefreshing(true);
    const token = getStoredToken();
    if (!token) {
      setUser(null);
      setRefreshing(false);
      return null;
    }

    try {
      const profile = await authService.me();
      setUser(profile);
      setAuthError(null);
      setRefreshing(false);
      return profile;
    } catch {
      clearSession();
      setRefreshing(false);
      return null;
    }
  }, [clearSession]);

  useEffect(() => {
    let active = true;

    const bootstrap = async () => {
      await refreshSession();
      // Show splash screen for 3 seconds before proceeding
      if (active) {
        setTimeout(() => {
          if (active) {
            setInitializing(false);
          }
        }, 3000);
      }
    };

    bootstrap();
    return () => {
      active = false;
    };
  }, [refreshSession]);

  const login = useCallback(async (email, password) => {
    setAuthError(null);
    setLastPassword(password);
    try {
      const response = await authService.login(email, password);
      return await establishSession(response);
    } catch (err) {
      setAuthError(err.message);
      throw err;
    }
  }, [establishSession]);

  const register = useCallback(async (username, email, password) => {
    setAuthError(null);
    setLastPassword(password);
    try {
      const response = await authService.register(username, email, password);
      return await establishSession(response);
    } catch (err) {
      setAuthError(err.message);
      throw err;
    }
  }, [establishSession]);

  const logout = useCallback(() => {
    clearSession();
    setAuthError(null);
    setLastPassword(null);
  }, [clearSession]);

  const value = useMemo(
    () => ({
      user,
      initializing,
      refreshing,
      authError,
      isAuthenticated: Boolean(user),
      login,
      register,
      logout,
      refreshSession,
      lastPassword,
      clearAuthError: () => setAuthError(null),
    }),
    [user, initializing, authError, login, register, logout, refreshing, refreshSession, lastPassword]
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return context;
};
