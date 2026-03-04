import { z } from "zod";

export const loginSchema = z.object({
  email: z.string().min(1, "El email es requerido").email("Email inválido"),
  password: z.string().min(6, "La contraseña debe tener al menos 6 caracteres"),
});

export const createUserSchema = z.object({
  nombre: z.string().min(1, "El nombre es requerido").max(50, "Máximo 50 caracteres"),
  apellido: z.string().min(1, "El apellido es requerido").max(50, "Máximo 50 caracteres"),
  email: z.string().min(1, "El email es requerido").email("Email inválido").max(100, "Máximo 100 caracteres"),
  password: z.string().min(6, "La contraseña debe tener al menos 6 caracteres"),
  rol: z.enum(["User", "Admin"]),
});

export const updateUserSchema = z.object({
  nombre: z.string().min(1, "El nombre es requerido").max(50, "Máximo 50 caracteres"),
  apellido: z.string().min(1, "El apellido es requerido").max(50, "Máximo 50 caracteres"),
  email: z.string().min(1, "El email es requerido").email("Email inválido").max(100, "Máximo 100 caracteres"),
});

export const estudioSchema = z.object({
  institucion: z.string().min(1, "La institución es requerida").max(150, "Máximo 150 caracteres"),
  titulo: z.string().min(1, "El título es requerido").max(150, "Máximo 150 caracteres"),
  nivelEstudio: z.enum(["Primario", "Secundario", "Terciario", "Universitario", "Posgrado"], {
    message: "El nivel de estudio es requerido",
  }),
  fechaInicio: z.string().min(1, "La fecha de inicio es requerida"),
  fechaFin: z.string().nullable().optional(),
  enCurso: z.boolean().optional(),
}).refine(
  (data) => {
    if (data.fechaFin && data.fechaInicio) {
      return new Date(data.fechaFin) > new Date(data.fechaInicio);
    }
    return true;
  },
  { message: "La fecha de fin debe ser posterior a la de inicio", path: ["fechaFin"] }
);

export const direccionSchema = z.object({
  calle: z.string().min(1, "La calle es requerida").max(200, "Máximo 200 caracteres"),
  ciudad: z.string().min(1, "La ciudad es requerida").max(100, "Máximo 100 caracteres"),
  estado: z.string().min(1, "El estado es requerido").max(100, "Máximo 100 caracteres"),
  pais: z.string().min(1, "El país es requerido").max(100, "Máximo 100 caracteres"),
  codigoPostal: z.string().min(1, "El código postal es requerido").max(20, "Máximo 20 caracteres"),
});

export type LoginFormData = z.infer<typeof loginSchema>;
export type CreateUserFormData = z.infer<typeof createUserSchema>;
export type UpdateUserFormData = z.infer<typeof updateUserSchema>;
export type EstudioFormData = z.infer<typeof estudioSchema>;
export type DireccionFormData = z.infer<typeof direccionSchema>;
