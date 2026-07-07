"""Pull the text layer out of a PDF. No OCR, no image handling (V1)."""

import io

import pdfplumber


def extract_text(pdf_bytes: bytes) -> str:
    """Return the concatenated text layer of every page.

    Assumes a real text layer exists; scanned/image-only PDFs will come back
    empty and are rejected by the caller.
    """
    parts: list[str] = []
    with pdfplumber.open(io.BytesIO(pdf_bytes)) as pdf:
        for page in pdf.pages:
            parts.append(page.extract_text() or "")
    return "\n".join(parts).strip()
