import { createContext, useState, useEffect, useCallback, type ReactNode } from "react";
import { authApi } from "../api/authApi";
import { usersApi } from "../api/usersApi";
import type { User } from "../types";

export interface AuthContextType {
  user: User | null;
  accessToken: string | null;
  refreshToken: string | null;
  isAuthenticated: boolean;
  isAdmin: boolean;
  isLoading: boolean;
  login: (email: string, password: string) => Promise<void>;
  logout: () => Promise<void>;
  updateUser: (user: User) => void;
}

export const AuthContext = createContext<AuthContextType>(null!);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [accessToken, setAccessToken] = useState<string | null>(null);
  const [refreshToken, setRefreshToken] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const storedToken = sessionStorage.getItem("accessToken");
    const storedRefresh = sessionStorage.getItem("refreshToken");
    const storedUser = sessionStorage.getItem("user");

    if (storedToken && storedRefresh) {
      setAccessToken(storedToken);
      setRefreshToken(storedRefresh);
      if (storedUser) {
        setUser(JSON.parse(storedUser));
      }
      // Validate session by fetching profile
      usersApi
        .getProfile()
        .then((profile) => {
          const u: User = {
            id: profile.id,
            nombre: profile.nombre,
            apellido: profile.apellido,
            email: profile.email,
            rol: profile.rol,
            createdAt: profile.createdAt,
          };
          setUser(u);
          sessionStorage.setItem("user", JSON.stringify(u));
        })
        .catch(() => {
          sessionStorage.clear();
          setUser(null);
          setAccessToken(null);
          setRefreshToken(null);
        })
        .finally(() => setIsLoading(false));
    } else {
      setIsLoading(false);
    }
  }, []);

  const login = useCallback(async (email: string, password: string) => {
    const data = await authApi.login(email, password);
    sessionStorage.setItem("accessToken", data.accessToken);
    sessionStorage.setItem("refreshToken", data.refreshToken);
    sessionStorage.setItem("user", JSON.stringify(data.user));
    setAccessToken(data.accessToken);
    setRefreshToken(data.refreshToken);
    setUser(data.user);
  }, []);

  const logout = useCallback(async () => {
    const rt = sessionStorage.getItem("refreshToken");
    if (rt) {
      try {
        await authApi.logout(rt);
      } catch {
        // ignore logout API errors
      }
    }
    sessionStorage.clear();
    setUser(null);
    setAccessToken(null);
    setRefreshToken(null);
  }, []);

  const updateUser = useCallback((u: User) => {
    setUser(u);
    sessionStorage.setItem("user", JSON.stringify(u));
  }, []);

  return (
    <AuthContext.Provider
      value={{
        user,
        accessToken,
        refreshToken,
        isAuthenticated: !!user,
        isAdmin: user?.rol === "Admin",
        isLoading,
        login,
        logout,
        updateUser,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}
