export function formatMoney(value: number, currency: string | null): string {
  const amount = value.toLocaleString(undefined, {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  });
  return currency ? `${currency} ${amount}` : amount;
}

export function formatDate(value: string | null): string {
  if (!value) return "—";
  // Dates come back as ISO (date or datetime). Show the date part only.
  return value.slice(0, 10);
}

export function orDash(value: string | null): string {
  return value && value.trim() !== "" ? value : "—";
}
