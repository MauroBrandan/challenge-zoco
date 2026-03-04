import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { estudioSchema, type EstudioFormData } from "../../schemas/validationSchemas";
import Input from "../ui/Input";
import Select from "../ui/Select";
import Button from "../ui/Button";

interface Props {
  defaultValues?: Partial<EstudioFormData>;
  onSubmit: (data: EstudioFormData) => Promise<void>;
  loading?: boolean;
}

const nivelOptions = [
  { value: "Primario", label: "Primario" },
  { value: "Secundario", label: "Secundario" },
  { value: "Terciario", label: "Terciario" },
  { value: "Universitario", label: "Universitario" },
  { value: "Posgrado", label: "Posgrado" },
];

export default function EstudioForm({ defaultValues, onSubmit, loading }: Props) {
  const { register, handleSubmit, watch, setValue, formState: { errors } } = useForm<EstudioFormData>({
    resolver: zodResolver(estudioSchema),
    defaultValues: {
      ...defaultValues,
      enCurso: !defaultValues?.fechaFin,
    },
  });

  const enCurso = watch("enCurso");

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
      <Input label="Institución" registration={register("institucion")} error={errors.institucion?.message} />
      <Input label="Título" registration={register("titulo")} error={errors.titulo?.message} />
      <Select label="Nivel de Estudio" registration={register("nivelEstudio")} error={errors.nivelEstudio?.message} options={nivelOptions} />
      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
        <Input label="Fecha Inicio" type="date" registration={register("fechaInicio")} error={errors.fechaInicio?.message} />
        <div>
          <Input
            label="Fecha Fin"
            type="date"
            registration={register("fechaFin")}
            error={errors.fechaFin?.message}
            disabled={enCurso}
          />
        </div>
      </div>
      <label className="flex items-center gap-2 text-sm text-gray-700">
        <input
          type="checkbox"
          {...register("enCurso")}
          onChange={(e) => {
            setValue("enCurso", e.target.checked);
            if (e.target.checked) setValue("fechaFin", null);
          }}
          className="rounded border-gray-300 text-indigo-600 focus:ring-indigo-500"
        />
        En curso
      </label>
      <div className="flex justify-end pt-2">
        <Button type="submit" loading={loading}>
          {defaultValues?.institucion ? "Guardar Cambios" : "Agregar Estudio"}
        </Button>
      </div>
    </form>
  );
}
