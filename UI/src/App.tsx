import { useEffect, useState } from "react";
import UploadForm from "./components/UploadForm";
import InvoiceResult from "./components/InvoiceResult";
import InvoiceList from "./components/InvoiceList";
import { getInvoice, listInvoices } from "./api/invoices";
import type { Invoice, InvoiceListItem } from "./types/invoice";

export default function App() {
  const [items, setItems] = useState<InvoiceListItem[]>([]);
  const [selected, setSelected] = useState<Invoice | null>(null);
  const [listError, setListError] = useState<string | null>(null);

  async function refreshList() {
    try {
      setItems(await listInvoices());
      setListError(null);
    } catch (err) {
      setListError(err instanceof Error ? err.message : "Could not load invoices.");
    }
  }

  useEffect(() => {
    refreshList();
  }, []);

  function handleUploaded(invoice: Invoice) {
    setSelected(invoice);
    refreshList();
  }

  async function handleSelect(id: string) {
    try {
      setSelected(await getInvoice(id));
    } catch (err) {
      setListError(err instanceof Error ? err.message : "Could not load invoice.");
    }
  }

  return (
    <div className="app">
      <header>
        <h1>Invoice Extractor</h1>
        <p className="muted">Upload a text-based PDF and get structured, validated data.</p>
      </header>

      <div className="layout">
        <aside>
          <UploadForm onUploaded={handleUploaded} />
          <InvoiceList
            items={items}
            selectedId={selected?.id ?? null}
            onSelect={handleSelect}
          />
          {listError && <p className="error">{listError}</p>}
        </aside>

        <main>
          {selected ? (
            <InvoiceResult invoice={selected} />
          ) : (
            <div className="card empty muted">
              Upload an invoice or pick one from the list to see the extracted data.
            </div>
          )}
        </main>
      </div>
    </div>
  );
}
