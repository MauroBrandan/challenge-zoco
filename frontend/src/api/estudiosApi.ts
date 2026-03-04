import axiosClient from './axiosClient'
import type { Estudio, CreateEstudioDTO } from '../types'

export const estudiosApi = {
	getByUserId: async (userId: number): Promise<Estudio[]> => {
		const { data } = await axiosClient.get<Estudio[]>(`/users/${userId}/estudios`)
		return data
	},

	create: async (userId: number, dto: CreateEstudioDTO): Promise<Estudio> => {
		const { data } = await axiosClient.post<Estudio>(
			`/users/${userId}/estudios`,
			dto
		)
		return data
	},

	update: async (
		userId: number,
		id: number,
		dto: CreateEstudioDTO
	): Promise<Estudio> => {
		const { data } = await axiosClient.put<Estudio>(
			`/users/${userId}/estudios/${id}`,
			dto
		)
		return data
	},

	delete: async (userId: number, id: number): Promise<void> => {
		await axiosClient.delete(`/users/${userId}/estudios/${id}`)
	}
}
