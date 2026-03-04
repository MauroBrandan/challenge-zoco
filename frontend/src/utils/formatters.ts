export function formatDate(dateString: string): string {
  return new Date(dateString).toLocaleDateString("es-AR", {
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
  });
}

export function formatDateInput(dateString: string): string {
  return dateString.split("T")[0];
}

export function formatNivelEstudio(nivel: string): string {
  return nivel;
}

export function getFullName(nombre: string, apellido: string): string {
  return `${nombre} ${apellido}`;
}
