import type { InvoiceListItem } from "../types/invoice";
import { formatDate, formatMoney, orDash } from "../utils/format";

interface Props {
  items: InvoiceListItem[];
  selectedId: string | null;
  onSelect: (id: string) => void;
}

export default function InvoiceList({ items, selectedId, onSelect }: Props) {
  return (
    <div className="card list">
      <h2>Invoices</h2>
      {items.length === 0 ? (
        <p className="muted">Nothing yet — upload a PDF to get started.</p>
      ) : (
        <ul>
          {items.map((item) => (
            <li key={item.id}>
              <button
                className={item.id === selectedId ? "list-item active" : "list-item"}
                onClick={() => onSelect(item.id)}
              >
                <span className="list-vendor">{orDash(item.vendorName)}</span>
                <span className="list-meta">
                  {orDash(item.invoiceNumber)} · {formatDate(item.invoiceDate)}
                </span>
                <span className="list-total">{formatMoney(item.total, item.currency)}</span>
              </button>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
