import { useEffect, useState } from "react";
import { Link, Navigate } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";
import { usersApi } from "../api/usersApi";
import type { User } from "../types";
import PageContainer from "../components/layout/PageContainer";
import Card from "../components/ui/Card";
import Button from "../components/ui/Button";
import LoadingSpinner from "../components/ui/LoadingSpinner";
import Modal from "../components/ui/Modal";
import ConfirmDialog from "../components/ui/ConfirmDialog";
import UserForm from "../components/forms/UserForm";
import Toast from "../components/ui/Toast";
import type { CreateUserFormData } from "../schemas/validationSchemas";

export default function DashboardPage() {
  const { isAdmin } = useAuth();

  if (!isAdmin) return <Navigate to="/profile" replace />;

  return <AdminDashboard />;
}

function AdminDashboard() {
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState("");
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [deleteTarget, setDeleteTarget] = useState<User | null>(null);
  const [submitting, setSubmitting] = useState(false);
  const [toast, setToast] = useState<{ message: string; type: "success" | "error" } | null>(null);

  const fetchUsers = async () => {
    try {
      const data = await usersApi.getAll();
      setUsers(data);
    } catch {
      setToast({ message: "Error al cargar usuarios", type: "error" });
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { fetchUsers(); }, []);

  const filteredUsers = users.filter(
    (u) =>
      u.nombre.toLowerCase().includes(search.toLowerCase()) ||
      u.apellido.toLowerCase().includes(search.toLowerCase()) ||
      u.email.toLowerCase().includes(search.toLowerCase())
  );

  const handleCreate = async (data: CreateUserFormData) => {
    setSubmitting(true);
    try {
      await usersApi.create(data);
      setShowCreateModal(false);
      setToast({ message: "Usuario creado exitosamente", type: "success" });
      fetchUsers();
    } catch {
      setToast({ message: "Error al crear usuario", type: "error" });
    } finally {
      setSubmitting(false);
    }
  };

  const handleDelete = async () => {
    if (!deleteTarget) return;
    setSubmitting(true);
    try {
      await usersApi.delete(deleteTarget.id);
      setDeleteTarget(null);
      setToast({ message: "Usuario eliminado", type: "success" });
      fetchUsers();
    } catch {
      setToast({ message: "Error al eliminar usuario", type: "error" });
    } finally {
      setSubmitting(false);
    }
  };

  if (loading) return <LoadingSpinner />;

  return (
    <PageContainer
      title="Dashboard"
      actions={<Button onClick={() => setShowCreateModal(true)}>Nuevo Usuario</Button>}
    >
      {/* Stats */}
      <div className="mb-6 grid grid-cols-1 gap-4 sm:grid-cols-3">
        <Card>
          <p className="text-sm text-gray-500">Total Usuarios</p>
          <p className="text-2xl font-bold text-gray-900">{users.length}</p>
        </Card>
        <Card>
          <p className="text-sm text-gray-500">Administradores</p>
          <p className="text-2xl font-bold text-gray-900">{users.filter((u) => u.rol === "Admin").length}</p>
        </Card>
        <Card>
          <p className="text-sm text-gray-500">Usuarios</p>
          <p className="text-2xl font-bold text-gray-900">{users.filter((u) => u.rol === "User").length}</p>
        </Card>
      </div>

      {/* Search */}
      <div className="mb-4">
        <input
          type="text"
          placeholder="Buscar por nombre o email..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          className="w-full rounded-lg border border-gray-300 px-4 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 sm:max-w-sm"
        />
      </div>

      {/* Users table (desktop) / cards (mobile) */}
      <Card>
        {/* Desktop table */}
        <div className="hidden md:block overflow-x-auto">
          <table className="w-full text-sm">
            <thead>
              <tr className="border-b border-gray-200 text-left text-gray-500">
                <th className="pb-3 font-medium">Nombre</th>
                <th className="pb-3 font-medium">Email</th>
                <th className="pb-3 font-medium">Rol</th>
                <th className="pb-3 font-medium">Creado</th>
                <th className="pb-3 font-medium">Acciones</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-100">
              {filteredUsers.map((user) => (
                <tr key={user.id} className="hover:bg-gray-50">
                  <td className="py-3">{user.nombre} {user.apellido}</td>
                  <td className="py-3 text-gray-600">{user.email}</td>
                  <td className="py-3">
                    <span className={`rounded-full px-2 py-0.5 text-xs font-medium ${
                      user.rol === "Admin" ? "bg-purple-100 text-purple-700" : "bg-gray-100 text-gray-700"
                    }`}>{user.rol}</span>
                  </td>
                  <td className="py-3 text-gray-600">{new Date(user.createdAt).toLocaleDateString("es-AR")}</td>
                  <td className="py-3">
                    <div className="flex gap-2">
                      <Link to={`/users/${user.id}`}>
                        <Button variant="outline" className="text-xs px-2 py-1">Ver</Button>
                      </Link>
                      <Button variant="danger" className="text-xs px-2 py-1" onClick={() => setDeleteTarget(user)}>
                        Eliminar
                      </Button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        {/* Mobile cards */}
        <div className="md:hidden space-y-3">
          {filteredUsers.map((user) => (
            <div key={user.id} className="rounded-lg border border-gray-200 p-4">
              <div className="flex items-center justify-between mb-2">
                <span className="font-medium text-gray-900">{user.nombre} {user.apellido}</span>
                <span className={`rounded-full px-2 py-0.5 text-xs font-medium ${
                  user.rol === "Admin" ? "bg-purple-100 text-purple-700" : "bg-gray-100 text-gray-700"
                }`}>{user.rol}</span>
              </div>
              <p className="text-sm text-gray-600 mb-3">{user.email}</p>
              <div className="flex gap-2">
                <Link to={`/users/${user.id}`} className="flex-1">
                  <Button variant="outline" className="w-full text-xs">Ver</Button>
                </Link>
                <Button variant="danger" className="flex-1 text-xs" onClick={() => setDeleteTarget(user)}>
                  Eliminar
                </Button>
              </div>
            </div>
          ))}
        </div>
      </Card>

      {/* Create modal */}
      <Modal isOpen={showCreateModal} onClose={() => setShowCreateModal(false)} title="Nuevo Usuario">
        <UserForm mode="create" onSubmit={handleCreate} loading={submitting} />
      </Modal>

      {/* Delete confirm */}
      <ConfirmDialog
        isOpen={!!deleteTarget}
        onClose={() => setDeleteTarget(null)}
        onConfirm={handleDelete}
        title="Eliminar Usuario"
        message={`¿Estás seguro de que querés eliminar a ${deleteTarget?.nombre} ${deleteTarget?.apellido}?`}
        loading={submitting}
      />

      {toast && <Toast message={toast.message} type={toast.type} onClose={() => setToast(null)} />}
    </PageContainer>
  );
}
