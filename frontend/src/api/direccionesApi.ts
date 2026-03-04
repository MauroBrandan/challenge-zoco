import axiosClient from './axiosClient'
import type { Direccion, CreateDireccionDTO } from '../types'

export const direccionesApi = {
	getByUserId: async (userId: number): Promise<Direccion[]> => {
		const { data } = await axiosClient.get<Direccion[]>(
			`/users/${userId}/direcciones`
		)
		return data
	},

	create: async (userId: number, dto: CreateDireccionDTO): Promise<Direccion> => {
		const { data } = await axiosClient.post<Direccion>(
			`/users/${userId}/direcciones`,
			dto
		)
		return data
	},

	update: async (
		userId: number,
		id: number,
		dto: CreateDireccionDTO
	): Promise<Direccion> => {
		const { data } = await axiosClient.put<Direccion>(
			`/users/${userId}/direcciones/${id}`,
			dto
		)
		return data
	},

	delete: async (userId: number, id: number): Promise<void> => {
		await axiosClient.delete(`/users/${userId}/direcciones/${id}`)
	}
}
