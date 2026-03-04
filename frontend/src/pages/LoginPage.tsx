import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { loginSchema, type LoginFormData } from "../schemas/validationSchemas";
import { useAuth } from "../hooks/useAuth";
import Input from "../components/ui/Input";
import Button from "../components/ui/Button";

export default function LoginPage() {
  const { login } = useAuth();
  const navigate = useNavigate();
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const { register, handleSubmit, formState: { errors } } = useForm<LoginFormData>({
    resolver: zodResolver(loginSchema),
  });

  const onSubmit = async (data: LoginFormData) => {
    setError("");
    setLoading(true);
    try {
      await login(data.email, data.password);
      navigate("/dashboard");
    } catch {
      setError("Credenciales inválidas. Intentá de nuevo.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-gray-50 px-4">
      <div className="w-full max-w-md">
        <div className="mb-8 text-center">
          <h1 className="text-3xl font-bold text-indigo-600">UserMgmt</h1>
          <p className="mt-2 text-sm text-gray-600">Iniciá sesión para continuar</p>
        </div>

        <div className="rounded-xl bg-white p-8 shadow-sm border border-gray-200">
          {error && (
            <div className="mb-4 rounded-lg bg-red-50 px-4 py-3 text-sm text-red-700">
              {error}
            </div>
          )}

          <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
            <Input
              label="Email"
              type="email"
              placeholder="tu@email.com"
              registration={register("email")}
              error={errors.email?.message}
            />
            <Input
              label="Contraseña"
              type="password"
              placeholder="******"
              registration={register("password")}
              error={errors.password?.message}
            />
            <Button type="submit" loading={loading} className="w-full">
              Iniciar Sesión
            </Button>
          </form>
        </div>
      </div>
    </div>
  );
}
