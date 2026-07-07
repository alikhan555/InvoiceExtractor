import type { Invoice } from "../types/invoice";
import { formatDate, formatMoney, orDash } from "../utils/format";

interface Props {
  invoice: Invoice;
}

export default function InvoiceResult({ invoice }: Props) {
  return (
    <div className="card result">
      <div className="result-header">
        <h2>{orDash(invoice.vendorName)}</h2>
        <span className={invoice.isValid ? "badge badge-ok" : "badge badge-warn"}>
          {invoice.isValid ? "Valid" : "Check needed"}
        </span>
      </div>

      {!invoice.isValid && invoice.validationMessages.length > 0 && (
        <ul className="warnings">
          {invoice.validationMessages.map((m, i) => (
            <li key={i}>{m}</li>
          ))}
        </ul>
      )}

      <dl className="fields">
        <div><dt>Invoice #</dt><dd>{orDash(invoice.invoiceNumber)}</dd></div>
        <div><dt>Invoice date</dt><dd>{formatDate(invoice.invoiceDate)}</dd></div>
        <div><dt>Due date</dt><dd>{formatDate(invoice.dueDate)}</dd></div>
        <div><dt>Currency</dt><dd>{orDash(invoice.currency)}</dd></div>
        <div><dt>File</dt><dd>{invoice.originalFileName}</dd></div>
      </dl>

      <table className="items">
        <thead>
          <tr>
            <th>Description</th>
            <th className="num">Qty</th>
            <th className="num">Unit price</th>
            <th className="num">Amount</th>
          </tr>
        </thead>
        <tbody>
          {invoice.lineItems.length === 0 ? (
            <tr>
              <td colSpan={4} className="muted">No line items extracted.</td>
            </tr>
          ) : (
            invoice.lineItems.map((li) => (
              <tr key={li.id}>
                <td>{orDash(li.description)}</td>
                <td className="num">{li.quantity}</td>
                <td className="num">{formatMoney(li.unitPrice, invoice.currency)}</td>
                <td className="num">{formatMoney(li.lineAmount, invoice.currency)}</td>
              </tr>
            ))
          )}
        </tbody>
      </table>

      <dl className="totals">
        <div><dt>Subtotal</dt><dd>{formatMoney(invoice.subtotal, invoice.currency)}</dd></div>
        <div><dt>Tax</dt><dd>{formatMoney(invoice.tax, invoice.currency)}</dd></div>
        <div className="grand"><dt>Total</dt><dd>{formatMoney(invoice.total, invoice.currency)}</dd></div>
      </dl>
    </div>
  );
}
