import { useState } from "react";
import { Link, useNavigate, useLocation } from "react-router-dom";
import { useAuth } from "../../hooks/useAuth";
import { getFullName } from "../../utils/formatters";

export default function Navbar() {
  const { user, isAdmin, logout } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const [menuOpen, setMenuOpen] = useState(false);

  const handleLogout = async () => {
    await logout();
    navigate("/login");
  };

  const isActive = (path: string) =>
    location.pathname === path || location.pathname.startsWith(path + "/");

  const linkClass = (path: string) =>
    `px-3 py-2 rounded-lg text-sm font-medium transition ${
      isActive(path) ? "bg-primary-100 text-primary-700" : "text-gray-600 hover:bg-gray-100 hover:text-gray-900"
    }`;

  return (
    <nav className="border-b border-gray-200 bg-white">
      <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
        <div className="flex h-16 items-center justify-between">
          <div className="flex items-center gap-8">
            <Link to="/dashboard" className="text-lg font-bold text-primary-600">
              Zoco - Gestión de Usuarios
            </Link>
            <div className="hidden md:flex items-center gap-1">
              <Link to="/dashboard" className={linkClass("/dashboard")}>
                Dashboard
              </Link>
              <Link to="/profile" className={linkClass("/profile")}>
                Mi Perfil
              </Link>
              {isAdmin && (
                <Link to="/users" className={linkClass("/users")}>
                  Usuarios
                </Link>
              )}
            </div>
          </div>

          <div className="hidden md:flex items-center gap-4">
            <span className="text-sm text-gray-600">
              {user && getFullName(user.nombre, user.apellido)}
              <span className="ml-1 rounded bg-primary-100 px-1.5 py-0.5 text-xs text-primary-700">
                {user?.rol}
              </span>
            </span>
            <button
              onClick={handleLogout}
              className="rounded-lg px-3 py-2 text-sm font-medium text-red-600 hover:bg-red-50 transition"
            >
              Cerrar sesión
            </button>
          </div>

          {/* Mobile hamburger */}
          <button
            onClick={() => setMenuOpen(!menuOpen)}
            className="md:hidden rounded-lg p-2 text-gray-600 hover:bg-gray-100"
          >
            <svg className="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              {menuOpen ? (
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
              ) : (
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 6h16M4 12h16M4 18h16" />
              )}
            </svg>
          </button>
        </div>
      </div>

      {/* Mobile menu */}
      {menuOpen && (
        <div className="md:hidden border-t border-gray-200 px-4 py-3 space-y-1">
          <Link to="/dashboard" className={`block ${linkClass("/dashboard")}`} onClick={() => setMenuOpen(false)}>
            Dashboard
          </Link>
          <Link to="/profile" className={`block ${linkClass("/profile")}`} onClick={() => setMenuOpen(false)}>
            Mi Perfil
          </Link>
          {isAdmin && (
            <Link to="/users" className={`block ${linkClass("/users")}`} onClick={() => setMenuOpen(false)}>
              Usuarios
            </Link>
          )}
          <div className="border-t border-gray-200 pt-2 mt-2">
            <span className="block px-3 py-2 text-sm text-gray-500">
              {user && getFullName(user.nombre, user.apellido)} ({user?.rol})
            </span>
            <button
              onClick={handleLogout}
              className="block w-full text-left rounded-lg px-3 py-2 text-sm font-medium text-red-600 hover:bg-red-50"
            >
              Cerrar sesión
            </button>
          </div>
        </div>
      )}
    </nav>
  );
}
