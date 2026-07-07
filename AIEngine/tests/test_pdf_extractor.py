"""Offline tests that do not require an OpenAI key.

Covers PDF text extraction and the FastAPI validation guards (bad type,
empty body). LLM-dependent paths are intentionally not exercised here.
"""

import io

from fastapi.testclient import TestClient
from fpdf import FPDF

from app.main import app
from app.pdf_extractor import extract_text

client = TestClient(app)


def _make_pdf(text: str) -> bytes:
    pdf = FPDF()
    pdf.add_page()
    pdf.set_font("Helvetica", size=12)
    for line in text.splitlines():
        pdf.cell(0, 8, line, ln=True)
    return bytes(pdf.output())


def test_extract_text_reads_layer():
    pdf_bytes = _make_pdf("Invoice Number: INV-1\nTotal: 42.00")
    text = extract_text(pdf_bytes)
    assert "INV-1" in text
    assert "42.00" in text


def test_extract_rejects_non_pdf():
    resp = client.post(
        "/extract",
        files={"file": ("note.txt", b"hello", "text/plain")},
    )
    assert resp.status_code == 415


def test_extract_rejects_pdf_without_text_layer():
    # A PDF with no extractable text should be rejected as unsupported (422),
    # not passed on to the LLM.
    resp = client.post(
        "/extract",
        files={"file": ("blank.pdf", _blank_pdf(), "application/pdf")},
    )
    assert resp.status_code == 422


def _blank_pdf() -> bytes:
    pdf = FPDF()
    pdf.add_page()  # a page with no text drawn on it
    return bytes(pdf.output())


def test_health():
    resp = client.get("/health")
    assert resp.status_code == 200
    assert resp.json()["status"] == "ok"
