export interface LineItem {
  id: string;
  description: string | null;
  quantity: number;
  unitPrice: number;
  lineAmount: number;
}

export interface Invoice {
  id: string;
  vendorName: string | null;
  invoiceNumber: string | null;
  invoiceDate: string | null;
  dueDate: string | null;
  currency: string | null;
  subtotal: number;
  tax: number;
  total: number;
  originalFileName: string;
  createdAt: string;
  lineItems: LineItem[];
  isValid: boolean;
  validationMessages: string[];
}

export interface InvoiceListItem {
  id: string;
  vendorName: string | null;
  invoiceNumber: string | null;
  invoiceDate: string | null;
  currency: string | null;
  total: number;
  createdAt: string;
}
