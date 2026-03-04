# User Management — Frontend

SPA para gestión de usuarios, estudios y direcciones. Construida con React + TypeScript y diseño responsivo con Tailwind CSS.

> 🚀 **Deploy en producción** - La app está desplegada en **Vercel**: https://challenge-zoco-mb.vercel.app/

---

## Tecnologías Utilizadas

| Tecnología | Uso |
|---|---|
| React 19 + TypeScript | UI con tipado estático |
| Vite | Build tool y dev server |
| Tailwind CSS v4 | Estilos utility-first con tema custom |
| React Router DOM v7 | Routing con rutas protegidas por rol |
| Axios | HTTP client con interceptors para JWT |
| React Hook Form + Zod | Formularios con validación declarativa |
| Context API + sessionStorage | Estado de autenticación persistente |

---

## Arquitectura y Patrones

- **Componentes reutilizables**: UI atómica (`Button`, `Input`, `Select`, `Modal`, `Toast`, `Card`, `EmptyState`) usada consistentemente en todas las páginas
- **Formularios con validación**: React Hook Form + Zod schemas validan en el cliente antes de enviar al backend
- **Separación de responsabilidades**: API client (`api/`), lógica de auth (`context/`), componentes de presentación (`components/`), y páginas (`pages/`) están desacoplados
- **Auth con refresh automático**: Axios interceptor que renueva el access token de forma transparente cuando expira, con cola de requests pendientes
- **Rutas protegidas**: `ProtectedRoute` verifica autenticación y rol antes de renderizar

---

## Funcionalidades

### Autenticación
- Login con validación de formulario
- Persistencia de sesión en `sessionStorage` (token, refresh token, datos del usuario)
- Refresh automático del JWT via interceptor de Axios
- Logout con limpieza de estado y redirección

### Dashboard (Admin)
- Vista general con estadísticas de usuarios
- Tabla de usuarios con búsqueda en tiempo real
- Crear y eliminar usuarios desde modales

### Perfil (User y Admin)
- Ver y editar datos personales
- CRUD completo de estudios (con checkbox "En curso" y validación de fechas)
- CRUD completo de direcciones
- Todo gestionado con modales y confirmación de eliminación

### Gestión de Usuarios (Admin)
- Listado con filtro por nombre/email
- Vista de detalle con estudios y direcciones del usuario
- Editar datos, agregar/editar/eliminar estudios y direcciones

### Diseño Responsivo
- Navbar con menú hamburguesa en móvil
- Tablas en desktop, cards apiladas en mobile
- Formularios y modales adaptativos

---

## Ejecución Local

### Requisitos previos
- Node.js 18+
- Backend corriendo en `http://localhost:5198` (ver [backend/README.md](../backend/README.md))

### 1. Instalar dependencias

```bash
cd frontend
npm install
```

### 2. Configurar variables de entorno

Crear `.env` en la raíz del frontend:

```
VITE_API_URL=http://localhost:5198/api
```

### 3. Ejecutar

```bash
npm run dev
```

La app estará en `http://localhost:5173`.

### Credenciales de prueba

| Email | Contraseña | Rol |
|---|---|---|
| admin@example.com | Admin123! | Admin |
| mauro@example.com | User123! | User |
| juanpablo@example.com | User123! | User |

---

## Estructura del proyecto

```
src/
├── api/           # Axios client y módulos de API
├── components/
│   ├── ui/        # Button, Input, Modal, Toast, etc.
│   ├── layout/    # Navbar, Layout, PageContainer
│   └── forms/     # UserForm, EstudioForm, DireccionForm
├── context/       # AuthContext (React Context)
├── hooks/         # useAuth
├── pages/         # Login, Dashboard, Profile, Users, UserDetail
├── routes/        # AppRouter, ProtectedRoute
├── schemas/       # Zod validation schemas
├── types/         # TypeScript interfaces
└── utils/         # Formatters
```

---

## Deploy

La app está desplegada en **Vercel** con deploy automático desde `main`.

- `vercel.json` configura rewrites para SPA routing
- Variable de entorno en Vercel: `VITE_API_URL` = URL del backend en Azure
