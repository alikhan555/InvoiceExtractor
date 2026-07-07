import { apiGet, apiPostForm } from "./client";
import type { Invoice, InvoiceListItem } from "../types/invoice";

export function uploadInvoice(file: File): Promise<Invoice> {
  const form = new FormData();
  form.append("file", file);
  return apiPostForm<Invoice>("/api/invoices/upload", form);
}

export function getInvoice(id: string): Promise<Invoice> {
  return apiGet<Invoice>(`/api/invoices/${id}`);
}

export function listInvoices(): Promise<InvoiceListItem[]> {
  return apiGet<InvoiceListItem[]>("/api/invoices");
}
