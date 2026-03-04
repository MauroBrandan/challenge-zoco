import axiosClient from "./axiosClient";
import type { AuthResponse } from "../types";

export const authApi = {
  login: async (email: string, password: string): Promise<AuthResponse> => {
    const { data } = await axiosClient.post<AuthResponse>("/auth/login", { email, password });
    return data;
  },

  logout: async (refreshToken: string): Promise<void> => {
    await axiosClient.post("/auth/logout", { refreshToken });
  },

  refreshToken: async (refreshToken: string): Promise<AuthResponse> => {
    const { data } = await axiosClient.post<AuthResponse>("/auth/refresh", { refreshToken });
    return data;
  },
};
