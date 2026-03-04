# Zoco Challenge — Gestión de Usuarios

Aplicación web Full Stack que permite gestionar usuarios, sus estudios y direcciones personales. Cuenta con autenticación JWT, control de acceso por roles (Admin/User), registro de sesiones, API REST en .NET y frontend responsivo en React.

> **Prueba Técnica Full Stack Developer (.NET + React)** — Zoco

---

## Links de Deploy

| Servicio | URL |
|---|---|
| Frontend (Vercel) | https://challenge-zoco-mb.vercel.app/ |
| Backend API (Azure) | https://challenge-zoco-api-bzfsaccxgfhzchdg.brazilsouth-01.azurewebsites.net/api |
| Swagger UI | https://challenge-zoco-api-bzfsaccxgfhzchdg.brazilsouth-01.azurewebsites.net/swagger |

---

## Stack Tecnológico

| Capa | Tecnologías |
|---|---|
| **Backend** | .NET 10, ASP.NET Core, Entity Framework Core, SQL Server, JWT, Swagger |
| **Frontend** | React 19, TypeScript, Vite, Tailwind CSS v4, React Router DOM, Axios, React Hook Form + Zod |
| **Infraestructura** | Azure App Service, Azure SQL Database, Vercel, GitHub Actions (CI/CD) |

---

## Requerimientos del Challenge Resueltos

### Parte 1 — Backend (.NET Core)

| Requerimiento | Estado | Detalle |
|---|---|---|
| .NET Core 6+ | Implementado | .NET 10 con ASP.NET Core |
| Entity Framework Core + SQL Server | Implementado | Code-First con migraciones automáticas |
| Autenticación JWT | Implementado | Access token + Refresh token con rotación |
| Swagger funcional | Implementado | Con soporte para Bearer token |
| Inyección de dependencias | Implementado | Repositorios, servicios y helpers registrados en DI |
| Login y logout con JWT | Implementado | Login, logout server-side, refresh token |
| CRUD Usuarios, Estudios, Direcciones | Implementado | Endpoints RESTful completos |
| Validación por rol (Admin/User) | Implementado | `[Authorize(Roles)]` + validación de ownership |
| Registro de sesión (SessionLogs) | Implementado | Tabla con UserId, FechaInicio, FechaFin |
| Middleware de autorización | Implementado | JWT Bearer + middleware global de errores |
| Validación de propiedad de recursos | Implementado | `AuthorizationHelper.IsOwnerOrAdmin()` |
| Código organizado por capas | Implementado | Controllers → Services → Repositories |
| Migraciones con EF Core | Implementado | Se aplican automáticamente al iniciar |
| Repositorio público en GitHub | Implementado | Este repositorio |
| Instrucciones para correr localmente | Implementado | Ver [backend/README.md](./backend/README.md) |

### Parte 2 — Frontend (React)

| Requerimiento | Estado | Detalle |
|---|---|---|
| React con Hooks | Implementado | React 19 con hooks y componentes funcionales |
| React Router DOM | Implementado | Rutas protegidas por autenticación y rol |
| Context API para autenticación | Implementado | `AuthContext` con login, logout, updateUser |
| Axios | Implementado | Interceptors para JWT y refresh automático |
| Tailwind CSS | Implementado | v4 con tema custom (colores de marca) |
| sessionStorage | Implementado | Tokens y datos de usuario en sessionStorage |
| Login con formulario validado | Implementado | React Hook Form + Zod |
| Dashboard protegido por rol | Implementado | Admin: gestión de usuarios / User: perfil |
| CRUD Estudios y Direcciones por usuario | Implementado | Con modales, validaciones y confirmación |
| Logout global | Implementado | Limpia tokens, contexto y redirige a login |
| Diseño responsivo | Implementado | Navbar mobile, tablas → cards, modales adaptativos |
| Deploy en Vercel | Implementado | Con rewrites para SPA routing |

### Extras implementados (no requeridos)

- Refresh token con rotación automática y expiración configurable
- Interceptor de Axios con cola de requests durante el refresh
- Middleware global de errores (stack trace solo en desarrollo)
- Protección contra eliminar al último administrador
- Validación de FechaFin > FechaInicio en estudios
- Endpoint `/api/profile` dedicado para el usuario autenticado
- CI/CD con GitHub Actions para deploy automático a Azure
- Seed data con usuarios de prueba listos para usar
- Dockerfile multi-stage para el backend

---

## Ejecución Local Rápida

### Backend
```bash
# 1. Levantar SQL Server con Docker
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong!Passw0rd" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest

# 2. Crear appsettings.Development.json (ver backend/README.md)

# 3. Ejecutar
cd backend
dotnet run
```
API: `http://localhost:5198` — Swagger: `http://localhost:5198/swagger`

### Frontend
```bash
# 1. Crear .env con VITE_API_URL=http://localhost:5198/api

# 2. Ejecutar
cd frontend
npm install
npm run dev
```
App: `http://localhost:5173`

### Credenciales de prueba

| Email | Contraseña | Rol |
|---|---|---|
| admin@example.com | Admin123! | Admin |
| mauro@example.com | User123! | User |
| juanpablo@example.com | User123! | User |

---

## Documentación Detallada

- [Backend — README](./backend/README.md): arquitectura, patrones, endpoints, base de datos, deploy
- [Frontend — README](./frontend/README.md): componentes, auth flow, rutas, validaciones, deploy
