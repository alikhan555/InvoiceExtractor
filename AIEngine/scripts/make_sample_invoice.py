"""Generate a small text-based sample invoice PDF for end-to-end testing.

Run with:  uv run python scripts/make_sample_invoice.py
Output:    samples/sample_invoice.pdf
"""

from pathlib import Path

from fpdf import FPDF

OUT = Path(__file__).resolve().parent.parent / "samples" / "sample_invoice.pdf"


def build() -> None:
    OUT.parent.mkdir(parents=True, exist_ok=True)

    pdf = FPDF()
    pdf.add_page()
    pdf.set_font("Helvetica", "B", 20)
    pdf.cell(0, 12, "INVOICE", ln=True)

    pdf.set_font("Helvetica", size=11)
    pdf.ln(2)
    pdf.cell(0, 7, "Acme Widgets Ltd.", ln=True)
    pdf.cell(0, 7, "Invoice Number: INV-2026-0042", ln=True)
    pdf.cell(0, 7, "Invoice Date: 2026-07-01", ln=True)
    pdf.cell(0, 7, "Due Date: 2026-07-31", ln=True)
    pdf.cell(0, 7, "Currency: USD", ln=True)
    pdf.ln(4)

    pdf.set_font("Helvetica", "B", 11)
    pdf.cell(90, 8, "Description")
    pdf.cell(25, 8, "Qty")
    pdf.cell(35, 8, "Unit Price")
    pdf.cell(35, 8, "Amount", ln=True)

    pdf.set_font("Helvetica", size=11)
    rows = [
        ("Blue Widget", "10", "12.50", "125.00"),
        ("Red Gadget", "4", "30.00", "120.00"),
        ("Assembly Service", "2", "55.00", "110.00"),
    ]
    for desc, qty, unit, amount in rows:
        pdf.cell(90, 8, desc)
        pdf.cell(25, 8, qty)
        pdf.cell(35, 8, unit)
        pdf.cell(35, 8, amount, ln=True)

    pdf.ln(4)
    pdf.cell(0, 7, "Subtotal: 355.00", ln=True)
    pdf.cell(0, 7, "Tax (10%): 35.50", ln=True)
    pdf.set_font("Helvetica", "B", 11)
    pdf.cell(0, 7, "Total: 390.50", ln=True)

    pdf.output(str(OUT))
    print(f"Wrote {OUT}")


if __name__ == "__main__":
    build()
