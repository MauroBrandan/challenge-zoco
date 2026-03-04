import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { createUserSchema, updateUserSchema, type CreateUserFormData, type UpdateUserFormData } from "../../schemas/validationSchemas";
import Input from "../ui/Input";
import Select from "../ui/Select";
import Button from "../ui/Button";

interface CreateProps {
  mode: "create";
  onSubmit: (data: CreateUserFormData) => Promise<void>;
  loading?: boolean;
}

interface EditProps {
  mode: "edit";
  defaultValues: UpdateUserFormData;
  onSubmit: (data: UpdateUserFormData) => Promise<void>;
  loading?: boolean;
}

type Props = CreateProps | EditProps;

export default function UserForm(props: Props) {
  if (props.mode === "create") {
    return <CreateForm {...props} />;
  }
  return <EditForm {...props} />;
}

function CreateForm({ onSubmit, loading }: CreateProps) {
  const { register, handleSubmit, formState: { errors } } = useForm<CreateUserFormData>({
    resolver: zodResolver(createUserSchema),
    defaultValues: { rol: "User" },
  });

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
        <Input label="Nombre" registration={register("nombre")} error={errors.nombre?.message} />
        <Input label="Apellido" registration={register("apellido")} error={errors.apellido?.message} />
      </div>
      <Input label="Email" type="email" registration={register("email")} error={errors.email?.message} />
      <Input label="Contraseña" type="password" registration={register("password")} error={errors.password?.message} />
      <Select
        label="Rol"
        registration={register("rol")}
        error={errors.rol?.message}
        options={[
          { value: "User", label: "Usuario" },
          { value: "Admin", label: "Administrador" },
        ]}
      />
      <div className="flex justify-end pt-2">
        <Button type="submit" loading={loading}>Crear Usuario</Button>
      </div>
    </form>
  );
}

function EditForm({ defaultValues, onSubmit, loading }: EditProps) {
  const { register, handleSubmit, formState: { errors } } = useForm<UpdateUserFormData>({
    resolver: zodResolver(updateUserSchema),
    defaultValues,
  });

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
        <Input label="Nombre" registration={register("nombre")} error={errors.nombre?.message} />
        <Input label="Apellido" registration={register("apellido")} error={errors.apellido?.message} />
      </div>
      <Input label="Email" type="email" registration={register("email")} error={errors.email?.message} />
      <div className="flex justify-end pt-2">
        <Button type="submit" loading={loading}>Guardar Cambios</Button>
      </div>
    </form>
  );
}
