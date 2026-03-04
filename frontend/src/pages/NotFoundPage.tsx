import { Link } from "react-router-dom";
import Button from "../components/ui/Button";

export default function NotFoundPage() {
  return (
    <div className="flex min-h-screen flex-col items-center justify-center bg-gray-50 px-4 text-center">
      <h1 className="text-6xl font-bold text-gray-300">404</h1>
      <p className="mt-4 text-lg text-gray-600">Página no encontrada</p>
      <Link to="/dashboard" className="mt-6">
        <Button>Volver al inicio</Button>
      </Link>
    </div>
  );
}
