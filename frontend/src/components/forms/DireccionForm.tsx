import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { direccionSchema, type DireccionFormData } from "../../schemas/validationSchemas";
import Input from "../ui/Input";
import Button from "../ui/Button";

interface Props {
  defaultValues?: Partial<DireccionFormData>;
  onSubmit: (data: DireccionFormData) => Promise<void>;
  loading?: boolean;
}

export default function DireccionForm({ defaultValues, onSubmit, loading }: Props) {
  const { register, handleSubmit, formState: { errors } } = useForm<DireccionFormData>({
    resolver: zodResolver(direccionSchema),
    defaultValues,
  });

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
      <Input label="Calle" registration={register("calle")} error={errors.calle?.message} />
      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
        <Input label="Ciudad" registration={register("ciudad")} error={errors.ciudad?.message} />
        <Input label="Estado / Provincia" registration={register("estado")} error={errors.estado?.message} />
      </div>
      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
        <Input label="País" registration={register("pais")} error={errors.pais?.message} />
        <Input label="Código Postal" registration={register("codigoPostal")} error={errors.codigoPostal?.message} />
      </div>
      <div className="flex justify-end pt-2">
        <Button type="submit" loading={loading}>
          {defaultValues?.calle ? "Guardar Cambios" : "Agregar Dirección"}
        </Button>
      </div>
    </form>
  );
}
