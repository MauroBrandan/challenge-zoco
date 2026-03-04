# User Management API — Backend

API REST para gestión de usuarios, estudios y direcciones personales. Construida con ASP.NET Core y Entity Framework Core, con autenticación JWT y control de acceso basado en roles.

> **Deploy en producción:** La API está desplegada en **Azure App Service** con CI/CD automático via GitHub Actions.

---

## Tecnologías Utilizadas

| Tecnología               | Uso                                       |
| ------------------------ | ----------------------------------------- |
| .NET 10 / ASP.NET Core   | Framework web y runtime                   |
| Entity Framework Core 10 | ORM y migraciones (Code-First)            |
| SQL Server               | Base de datos relacional                  |
| JWT (Bearer Tokens)      | Autenticación con Access + Refresh tokens |
| BCrypt                   | Hashing seguro de contraseñas             |
| Swagger / OpenAPI        | Documentación interactiva de la API       |
| GitHub Actions           | CI/CD para deploy automático a Azure      |

---

## Arquitectura y Patrones de Diseño

El proyecto sigue una **arquitectura en capas** con separación clara de responsabilidades:

```
backend/
├── Controllers/              # Capa de presentación (endpoints HTTP)
│   ├── AuthController.cs         # Login, logout, refresh token
│   ├── UsersController.cs        # CRUD de usuarios (Admin)
│   ├── ProfileController.cs      # Perfil del usuario autenticado
│   ├── EstudiosController.cs     # CRUD de estudios (nested bajo /users/{id})
│   └── DireccionesController.cs  # CRUD de direcciones (nested bajo /users/{id})
│
├── Services/                 # Capa de lógica de negocio
│   ├── Interfaces/               # Contratos (IUserService, IAuthService, etc.)
│   ├── AuthService.cs            # Autenticación, tokens, sesiones
│   ├── UserService.cs            # Lógica de usuarios + validaciones
│   ├── EstudioService.cs         # Lógica de estudios + validación de fechas
│   └── DireccionService.cs       # Lógica de direcciones
│
├── Repositories/             # Capa de acceso a datos
│   ├── Interfaces/               # Contratos (IUserRepository, etc.)
│   ├── UserRepository.cs         # Queries de usuarios con eager loading
│   ├── SessionRepository.cs      # Gestión de sesiones activas
│   ├── EstudioRepository.cs      # Queries de estudios
│   └── DireccionRepository.cs    # Queries de direcciones
│
├── Models/
│   ├── Entities/             # Entidades del dominio (User, Estudio, Direccion, SessionLog)
│   └── DTOs/                 # Data Transfer Objects (request/response)
│
├── Data/
│   ├── ApplicationDbContext.cs   # DbContext con configuración Fluent API
│   └── SeedData.cs               # Datos iniciales para desarrollo
│
├── Helpers/
│   ├── JwtHelper.cs              # Generación y validación de tokens JWT
│   └── AuthorizationHelper.cs    # Utilidades de autorización (IsOwnerOrAdmin)
│
├── Middleware/
│   └── ExceptionMiddleware.cs    # Manejo global de excepciones
│
├── Migrations/               # Migraciones de EF Core (Code-First)
├── Program.cs                # Punto de entrada, DI y pipeline de middleware
├── Dockerfile                # Imagen de producción multi-stage
├── appsettings.json          # Configuración base
└── appsettings.Production.json   # Configuración de producción (valores via env vars)
```

### Patrones implementados

- **Repository Pattern**: Abstrae el acceso a datos detrás de interfaces (`IUserRepository`, `ISessionRepository`, etc.). Los controllers nunca acceden al `DbContext` directamente.
- **Service Layer Pattern**: Toda la lógica de negocio vive en los Services. Los controllers solo orquestan request → service → response.
- **DTO Pattern**: Se usan DTOs específicos para request y response, evitando exponer las entidades directamente (ej: `PasswordHash` nunca se envía al cliente).
- **Dependency Injection**: Todos los servicios, repositorios y helpers se registran en el contenedor de DI de ASP.NET Core (`Program.cs`). Se inyectan via constructor.

---

## Funcionalidades

### Autenticación y Sesiones

- **Login** con email + contraseña (BCrypt para verificación)
- **JWT Access Token** (configurable, default 60 min) con claims: userId, email, rol, nombre
- **Refresh Token** (opaco, rotación automática): cada refresh invalida el token anterior y emite uno nuevo
- **Logout** server-side: invalida la sesión activa en base de datos
- **SessionLogs**: tabla que registra cada sesión (`UserId`, `FechaInicio`, `FechaFin`), permitiendo auditoría de acceso
- **Expiración de Refresh Token**: se valida contra `RefreshTokenExpirationDays` del config

### Control de Acceso por Roles

- **Admin**: acceso completo a todos los endpoints, puede gestionar cualquier usuario y sus datos
- **User**: solo puede ver y editar su propio perfil, estudios y direcciones
- Implementado con `[Authorize]` y `[Authorize(Roles = "Admin")]` en controllers
- Validación de propiedad de recursos via `AuthorizationHelper.IsOwnerOrAdmin()`
- Protección contra eliminar al último administrador del sistema

### CRUD Completo

- **Usuarios**: crear, listar, ver detalle (con estudios y direcciones), actualizar, eliminar
- **Estudios**: CRUD anidado bajo `/api/users/{userId}/estudios` con validación de fechas
- **Direcciones**: CRUD anidado bajo `/api/users/{userId}/direcciones`
- **Perfil**: endpoint dedicado `/api/profile` para el usuario autenticado

### Validaciones

- Data Annotations en DTOs (`[Required]`, `[MaxLength]`, `[EmailAddress]`, `[MinLength]`)
- Email único (con soporte para excluir al propio usuario en updates)
- FechaFin > FechaInicio en estudios
- No se puede eliminar al único Admin del sistema

### Middleware Global de Errores

- `ExceptionMiddleware` captura excepciones no manejadas
- En desarrollo: devuelve message + stack trace
- En producción: devuelve mensaje genérico (sin exponer detalles internos)

---

## Endpoints de la API

### Auth (`/api/auth`)

| Método | Ruta                | Auth | Descripción                             |
| ------ | ------------------- | ---- | --------------------------------------- |
| POST   | `/api/auth/login`   | No   | Autenticar con email + password         |
| POST   | `/api/auth/logout`  | JWT  | Cerrar sesión (invalida refresh token)  |
| POST   | `/api/auth/refresh` | No   | Obtener nuevos tokens con refresh token |

### Users (`/api/users`) — Solo Admin

| Método | Ruta              | Auth        | Descripción                        |
| ------ | ----------------- | ----------- | ---------------------------------- |
| GET    | `/api/users`      | Admin       | Listar todos los usuarios          |
| GET    | `/api/users/{id}` | Owner/Admin | Detalle con estudios y direcciones |
| POST   | `/api/users`      | Admin       | Crear usuario                      |
| PUT    | `/api/users/{id}` | Owner/Admin | Actualizar nombre y email          |
| DELETE | `/api/users/{id}` | Admin       | Eliminar usuario (cascade)         |

### Profile (`/api/profile`) — Usuario autenticado

| Método | Ruta           | Auth | Descripción                          |
| ------ | -------------- | ---- | ------------------------------------ |
| GET    | `/api/profile` | JWT  | Mi perfil con estudios y direcciones |
| PUT    | `/api/profile` | JWT  | Actualizar mi nombre y email         |

### Estudios (`/api/users/{userId}/estudios`)

| Método | Ruta                                | Auth        | Descripción                 |
| ------ | ----------------------------------- | ----------- | --------------------------- |
| GET    | `/api/users/{userId}/estudios`      | Owner/Admin | Listar estudios del usuario |
| GET    | `/api/users/{userId}/estudios/{id}` | Owner/Admin | Detalle de un estudio       |
| POST   | `/api/users/{userId}/estudios`      | Owner/Admin | Crear estudio               |
| PUT    | `/api/users/{userId}/estudios/{id}` | Owner/Admin | Actualizar estudio          |
| DELETE | `/api/users/{userId}/estudios/{id}` | Owner/Admin | Eliminar estudio            |

### Direcciones (`/api/users/{userId}/direcciones`)

| Método | Ruta                                   | Auth        | Descripción                    |
| ------ | -------------------------------------- | ----------- | ------------------------------ |
| GET    | `/api/users/{userId}/direcciones`      | Owner/Admin | Listar direcciones del usuario |
| GET    | `/api/users/{userId}/direcciones/{id}` | Owner/Admin | Detalle de una dirección       |
| POST   | `/api/users/{userId}/direcciones`      | Owner/Admin | Crear dirección                |
| PUT    | `/api/users/{userId}/direcciones/{id}` | Owner/Admin | Actualizar dirección           |
| DELETE | `/api/users/{userId}/direcciones/{id}` | Owner/Admin | Eliminar dirección             |

---

## Ejecución Local

### Requisitos previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local o Docker)

### 1. Levantar SQL Server con Docker

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=[UNA_CONTRASEÑA_SEGURA]" \
  -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

### 2. Crear `appsettings.Development.json`

Este archivo está en `.gitignore` por seguridad. Crealo en la raíz del backend:

```json
{
	"ConnectionStrings": {
		"DefaultConnection": "Server=localhost,1433;Database=UserManagementDB;User Id=sa;Password=[UNA_CONTRASEÑA_SEGURA];TrustServerCertificate=true"
	},
	"JwtSettings": {
		"SecretKey": "", // Secret key arbitraria para la encriptacion con JWT
		"Issuer": "UserManagementAPI",
		"Audience": "UserManagementClient",
		"AccessTokenExpirationMinutes": 60,
		"RefreshTokenExpirationDays": 7
	}
}
```

### 3. Ejecutar

```bash
cd backend
dotnet run
```

La API estará disponible en `http://localhost:5198`.

- **Swagger UI**: `http://localhost:5198/swagger`
- Las **migraciones se aplican automáticamente** al iniciar (`Database.Migrate()`)
- Los **datos iniciales se cargan automáticamente** si la base está vacía (Seed Data)

### Usuarios pre-cargados (Seed Data)

| Email                 | Contraseña | Rol   |
| --------------------- | ---------- | ----- |
| admin@example.com     | Admin123!  | Admin |
| mauro@example.com     | User123!   | User  |
| juanpablo@example.com | User123!   | User  |

Los usuarios User ya tienen estudios y direcciones cargados para poder probar la funcionalidad completa desde el primer momento.

---

## Deploy en Azure

La API está desplegada en **Azure App Service** con deploy continuo:

- **CI/CD**: GitHub Actions (`.github/workflows/`) — cada push a `main` despliega automáticamente
- **Base de datos**: Azure SQL Database
- **Variables de entorno**: configuradas en Azure App Service Settings
  - `ConnectionStrings__DefaultConnection` — Connection string de Azure SQL
  - `JwtSettings__SecretKey` — Clave secreta para firma de tokens JWT

### Docker (alternativa)

```bash
docker build -t usermanagement-api .
docker run -p 8080:8080 \
  -e "ConnectionStrings__DefaultConnection=Server=...;Database=...;..." \
  -e "JwtSettings__SecretKey=TuClaveSecreta256Bits" \
  usermanagement-api
```

---

## Base de Datos

### Diagrama de entidades

```
┌──────────────┐       ┌──────────────┐
│    Users     │       │  SessionLogs │
├──────────────┤       ├──────────────┤
│ Id (PK)      │──┐    │ Id (PK)      │
│ Nombre       │  │    │ UserId (FK)  │──┐
│ Apellido     │  │    │ FechaInicio  │  │
│ Email (UQ)   │  │    │ FechaFin     │  │
│ PasswordHash │  │    │ RefreshToken │  │
│ Rol          │  │    └──────────────┘  │
│ CreatedAt    │  │                      │
└──────────────┘  │    ┌──────────────┐  │
       │          ├───>│  Estudios    │  │
       │          │    ├──────────────┤  │
       │          │    │ Id (PK)      │  │
       │          │    │ Institucion  │  │
       │          │    │ Titulo       │  │
       │          │    │ NivelEstudio │  │
       │          │    │ FechaInicio  │  │
       │          │    │ FechaFin     │  │
       │          │    │ UserId (FK)  │  │
       │          │    └──────────────┘  │
       │          │                      │
       │          │    ┌──────────────┐  │
       │          └───>│ Direcciones  │  │
       │               ├──────────────┤  │
       │               │ Id (PK)      │  │
       │               │ Calle        │  │
       │               │ Ciudad       │  │
       │               │ Estado       │  │
       │               │ Pais         │  │
       │               │ CodigoPostal │  │
       │               │ UserId (FK)  │  │
       └──────────────>└──────────────┘  │
                                         │
       └─────────────────────────────────┘
```

- Todas las FK tienen **cascade delete**
- `Email` tiene índice único
- Los enums `Role` y `NivelEstudio` se almacenan como **string** en la DB
- Las migraciones están en `/Migrations/` y se aplican automáticamente al iniciar
