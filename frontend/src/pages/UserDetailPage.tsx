import { useEffect, useState, useCallback } from "react";
import { useParams, Link } from "react-router-dom";
import { usersApi } from "../api/usersApi";
import { estudiosApi } from "../api/estudiosApi";
import { direccionesApi } from "../api/direccionesApi";
import type { UserDetail, Estudio, Direccion } from "../types";
import PageContainer from "../components/layout/PageContainer";
import Card from "../components/ui/Card";
import Button from "../components/ui/Button";
import Modal from "../components/ui/Modal";
import ConfirmDialog from "../components/ui/ConfirmDialog";
import EmptyState from "../components/ui/EmptyState";
import LoadingSpinner from "../components/ui/LoadingSpinner";
import Toast from "../components/ui/Toast";
import UserForm from "../components/forms/UserForm";
import EstudioForm from "../components/forms/EstudioForm";
import DireccionForm from "../components/forms/DireccionForm";
import type { UpdateUserFormData, EstudioFormData, DireccionFormData } from "../schemas/validationSchemas";
import { formatDate, formatDateInput } from "../utils/formatters";

export default function UserDetailPage() {
  const { id } = useParams<{ id: string }>();
  const userId = Number(id);

  const [user, setUser] = useState<UserDetail | null>(null);
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [toast, setToast] = useState<{ message: string; type: "success" | "error" } | null>(null);

  const [showEditUser, setShowEditUser] = useState(false);
  const [estudioModal, setEstudioModal] = useState<{ open: boolean; editing?: Estudio }>({ open: false });
  const [direccionModal, setDireccionModal] = useState<{ open: boolean; editing?: Direccion }>({ open: false });
  const [deleteTarget, setDeleteTarget] = useState<{ type: "estudio" | "direccion"; id: number } | null>(null);

  const fetchUser = useCallback(async () => {
    try {
      const data = await usersApi.getById(userId);
      setUser(data);
    } catch {
      setToast({ message: "Error al cargar usuario", type: "error" });
    } finally {
      setLoading(false);
    }
  }, [userId]);

  useEffect(() => { fetchUser(); }, [fetchUser]);

  if (loading) return <LoadingSpinner />;
  if (!user) return <p className="p-8 text-center text-gray-600">Usuario no encontrado</p>;

  const handleUpdateUser = async (data: UpdateUserFormData) => {
    setSubmitting(true);
    try {
      await usersApi.update(userId, data);
      setShowEditUser(false);
      setToast({ message: "Usuario actualizado", type: "success" });
      fetchUser();
    } catch {
      setToast({ message: "Error al actualizar usuario", type: "error" });
    } finally {
      setSubmitting(false);
    }
  };

  const handleEstudioSubmit = async (data: EstudioFormData) => {
    setSubmitting(true);
    try {
      const dto = {
        institucion: data.institucion,
        titulo: data.titulo,
        nivelEstudio: data.nivelEstudio,
        fechaInicio: data.fechaInicio,
        fechaFin: data.enCurso ? null : (data.fechaFin || null),
      };
      if (estudioModal.editing) {
        await estudiosApi.update(userId, estudioModal.editing.id, dto);
        setToast({ message: "Estudio actualizado", type: "success" });
      } else {
        await estudiosApi.create(userId, dto);
        setToast({ message: "Estudio agregado", type: "success" });
      }
      setEstudioModal({ open: false });
      fetchUser();
    } catch {
      setToast({ message: "Error al guardar estudio", type: "error" });
    } finally {
      setSubmitting(false);
    }
  };

  const handleDireccionSubmit = async (data: DireccionFormData) => {
    setSubmitting(true);
    try {
      if (direccionModal.editing) {
        await direccionesApi.update(userId, direccionModal.editing.id, data);
        setToast({ message: "Dirección actualizada", type: "success" });
      } else {
        await direccionesApi.create(userId, data);
        setToast({ message: "Dirección agregada", type: "success" });
      }
      setDireccionModal({ open: false });
      fetchUser();
    } catch {
      setToast({ message: "Error al guardar dirección", type: "error" });
    } finally {
      setSubmitting(false);
    }
  };

  const handleDelete = async () => {
    if (!deleteTarget) return;
    setSubmitting(true);
    try {
      if (deleteTarget.type === "estudio") {
        await estudiosApi.delete(userId, deleteTarget.id);
        setToast({ message: "Estudio eliminado", type: "success" });
      } else {
        await direccionesApi.delete(userId, deleteTarget.id);
        setToast({ message: "Dirección eliminada", type: "success" });
      }
      setDeleteTarget(null);
      fetchUser();
    } catch {
      setToast({ message: "Error al eliminar", type: "error" });
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <PageContainer
      title={`${user.nombre} ${user.apellido}`}
      actions={
        <Link to="/users">
          <Button variant="outline">Volver a lista</Button>
        </Link>
      }
    >
      {/* User data */}
      <Card
        title="Datos del Usuario"
        actions={<Button variant="outline" onClick={() => setShowEditUser(true)}>Editar</Button>}
        className="mb-6"
      >
        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
          <div>
            <p className="text-xs text-gray-500">Nombre</p>
            <p className="text-sm font-medium text-gray-900">{user.nombre} {user.apellido}</p>
          </div>
          <div>
            <p className="text-xs text-gray-500">Email</p>
            <p className="text-sm font-medium text-gray-900">{user.email}</p>
          </div>
          <div>
            <p className="text-xs text-gray-500">Rol</p>
            <span className={`rounded-full px-2 py-0.5 text-xs font-medium ${
              user.rol === "Admin" ? "bg-purple-100 text-purple-700" : "bg-gray-100 text-gray-700"
            }`}>{user.rol}</span>
          </div>
          <div>
            <p className="text-xs text-gray-500">Fecha de Creación</p>
            <p className="text-sm font-medium text-gray-900">{formatDate(user.createdAt)}</p>
          </div>
        </div>
      </Card>

      {/* Estudios */}
      <Card
        title="Estudios"
        actions={<Button onClick={() => setEstudioModal({ open: true })}>Agregar</Button>}
        className="mb-6"
      >
        {user.estudios.length === 0 ? (
          <EmptyState message="No hay estudios registrados" />
        ) : (
          <div className="space-y-3">
            {user.estudios.map((e) => (
              <div key={e.id} className="flex items-start justify-between rounded-lg border border-gray-100 p-4">
                <div>
                  <p className="font-medium text-gray-900">{e.titulo}</p>
                  <p className="text-sm text-gray-600">{e.institucion}</p>
                  <p className="text-xs text-gray-500 mt-1">
                    {e.nivelEstudio} &middot; {formatDate(e.fechaInicio)} - {e.fechaFin ? formatDate(e.fechaFin) : "En curso"}
                  </p>
                </div>
                <div className="flex gap-2 shrink-0">
                  <Button variant="outline" className="text-xs px-2 py-1" onClick={() => setEstudioModal({ open: true, editing: e })}>
                    Editar
                  </Button>
                  <Button variant="danger" className="text-xs px-2 py-1" onClick={() => setDeleteTarget({ type: "estudio", id: e.id })}>
                    Eliminar
                  </Button>
                </div>
              </div>
            ))}
          </div>
        )}
      </Card>

      {/* Direcciones */}
      <Card
        title="Direcciones"
        actions={<Button onClick={() => setDireccionModal({ open: true })}>Agregar</Button>}
      >
        {user.direcciones.length === 0 ? (
          <EmptyState message="No hay direcciones registradas" />
        ) : (
          <div className="space-y-3">
            {user.direcciones.map((d) => (
              <div key={d.id} className="flex items-start justify-between rounded-lg border border-gray-100 p-4">
                <div>
                  <p className="font-medium text-gray-900">{d.calle}</p>
                  <p className="text-sm text-gray-600">{d.ciudad}, {d.estado}</p>
                  <p className="text-xs text-gray-500 mt-1">{d.pais} - CP {d.codigoPostal}</p>
                </div>
                <div className="flex gap-2 shrink-0">
                  <Button variant="outline" className="text-xs px-2 py-1" onClick={() => setDireccionModal({ open: true, editing: d })}>
                    Editar
                  </Button>
                  <Button variant="danger" className="text-xs px-2 py-1" onClick={() => setDeleteTarget({ type: "direccion", id: d.id })}>
                    Eliminar
                  </Button>
                </div>
              </div>
            ))}
          </div>
        )}
      </Card>

      {/* Modals */}
      <Modal isOpen={showEditUser} onClose={() => setShowEditUser(false)} title="Editar Usuario">
        <UserForm
          mode="edit"
          defaultValues={{ nombre: user.nombre, apellido: user.apellido, email: user.email }}
          onSubmit={handleUpdateUser}
          loading={submitting}
        />
      </Modal>

      <Modal
        isOpen={estudioModal.open}
        onClose={() => setEstudioModal({ open: false })}
        title={estudioModal.editing ? "Editar Estudio" : "Agregar Estudio"}
      >
        <EstudioForm
          key={estudioModal.editing?.id ?? "new"}
          defaultValues={
            estudioModal.editing
              ? {
                  institucion: estudioModal.editing.institucion,
                  titulo: estudioModal.editing.titulo,
                  nivelEstudio: estudioModal.editing.nivelEstudio,
                  fechaInicio: formatDateInput(estudioModal.editing.fechaInicio),
                  fechaFin: estudioModal.editing.fechaFin ? formatDateInput(estudioModal.editing.fechaFin) : null,
                }
              : undefined
          }
          onSubmit={handleEstudioSubmit}
          loading={submitting}
        />
      </Modal>

      <Modal
        isOpen={direccionModal.open}
        onClose={() => setDireccionModal({ open: false })}
        title={direccionModal.editing ? "Editar Dirección" : "Agregar Dirección"}
      >
        <DireccionForm
          key={direccionModal.editing?.id ?? "new"}
          defaultValues={
            direccionModal.editing
              ? {
                  calle: direccionModal.editing.calle,
                  ciudad: direccionModal.editing.ciudad,
                  estado: direccionModal.editing.estado,
                  pais: direccionModal.editing.pais,
                  codigoPostal: direccionModal.editing.codigoPostal,
                }
              : undefined
          }
          onSubmit={handleDireccionSubmit}
          loading={submitting}
        />
      </Modal>

      <ConfirmDialog
        isOpen={!!deleteTarget}
        onClose={() => setDeleteTarget(null)}
        onConfirm={handleDelete}
        title={deleteTarget?.type === "estudio" ? "Eliminar Estudio" : "Eliminar Dirección"}
        message="¿Estás seguro de que querés eliminar este registro?"
        loading={submitting}
      />

      {toast && <Toast message={toast.message} type={toast.type} onClose={() => setToast(null)} />}
    </PageContainer>
  );
}
