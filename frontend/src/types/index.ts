export interface User {
	id: number
	nombre: string
	apellido: string
	email: string
	rol: 'User' | 'Admin'
	createdAt: string
}

export interface UserDetail extends User {
	estudios: Estudio[]
	direcciones: Direccion[]
}

export interface Estudio {
	id: number
	institucion: string
	titulo: string
	nivelEstudio:
		| 'Primario'
		| 'Secundario'
		| 'Terciario'
		| 'Universitario'
		| 'Posgrado'
	fechaInicio: string
	fechaFin: string | null
	userId: number
}

export interface Direccion {
	id: number
	calle: string
	ciudad: string
	estado: string
	pais: string
	codigoPostal: string
	userId: number
}

export interface AuthResponse {
	accessToken: string
	refreshToken: string
	user: User
}

export interface CreateUserDTO {
	nombre: string
	apellido: string
	email: string
	password: string
	rol: 'User' | 'Admin'
}

export interface UpdateUserDTO {
	nombre: string
	apellido: string
	email: string
}

export interface CreateEstudioDTO {
	institucion: string
	titulo: string
	nivelEstudio: string
	fechaInicio: string
	fechaFin: string | null
}

export interface CreateDireccionDTO {
	calle: string
	ciudad: string
	estado: string
	pais: string
	codigoPostal: string
}
