import { useRef, useState } from "react";
import { uploadInvoice } from "../api/invoices";
import type { Invoice } from "../types/invoice";

interface Props {
  onUploaded: (invoice: Invoice) => void;
}

export default function UploadForm({ onUploaded }: Props) {
  const [file, setFile] = useState<File | null>(null);
  const [busy, setBusy] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const inputRef = useRef<HTMLInputElement>(null);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!file) return;

    setBusy(true);
    setError(null);
    try {
      const invoice = await uploadInvoice(file);
      onUploaded(invoice);
      setFile(null);
      if (inputRef.current) inputRef.current.value = "";
    } catch (err) {
      setError(err instanceof Error ? err.message : "Something went wrong.");
    } finally {
      setBusy(false);
    }
  }

  return (
    <form className="card upload" onSubmit={handleSubmit}>
      <h2>Upload invoice</h2>
      <p className="muted">One text-based PDF at a time.</p>

      <input
        ref={inputRef}
        type="file"
        accept="application/pdf,.pdf"
        onChange={(e) => setFile(e.target.files?.[0] ?? null)}
        disabled={busy}
      />

      <button type="submit" disabled={!file || busy}>
        {busy ? "Extracting…" : "Extract"}
      </button>

      {error && <p className="error">{error}</p>}
    </form>
  );
}
