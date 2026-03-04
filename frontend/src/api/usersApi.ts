import axiosClient from "./axiosClient";
import type { User, UserDetail, CreateUserDTO, UpdateUserDTO } from "../types";

export const usersApi = {
  getAll: async (): Promise<User[]> => {
    const { data } = await axiosClient.get<User[]>("/users");
    return data;
  },

  getById: async (id: number): Promise<UserDetail> => {
    const { data } = await axiosClient.get<UserDetail>(`/users/${id}`);
    return data;
  },

  create: async (dto: CreateUserDTO): Promise<User> => {
    const { data } = await axiosClient.post<User>("/users", dto);
    return data;
  },

  update: async (id: number, dto: UpdateUserDTO): Promise<User> => {
    const { data } = await axiosClient.put<User>(`/users/${id}`, dto);
    return data;
  },

  delete: async (id: number): Promise<void> => {
    await axiosClient.delete(`/users/${id}`);
  },

  getProfile: async (): Promise<UserDetail> => {
    const { data } = await axiosClient.get<UserDetail>("/profile");
    return data;
  },

  updateProfile: async (dto: UpdateUserDTO): Promise<UserDetail> => {
    const { data } = await axiosClient.put<UserDetail>("/profile", dto);
    return data;
  },
};
