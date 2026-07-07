"""FastAPI app exposing the internal /extract endpoint.

Stateless by design: a PDF comes in, structured JSON goes out. No storage.
"""

from fastapi import FastAPI, File, HTTPException, UploadFile

from .config import settings
from .extractor import extract_invoice
from .pdf_extractor import extract_text
from .schemas import InvoiceExtraction

app = FastAPI(
    title="Invoice Extractor AIEngine",
    version="0.1.0",
    description="Stateless service: text-based PDF in, structured invoice JSON out.",
)


@app.get("/health")
def health() -> dict:
    return {"status": "ok", "model": settings.openai_model}


@app.post("/extract", response_model=InvoiceExtraction)
async def extract(file: UploadFile = File(...)) -> InvoiceExtraction:
    filename = (file.filename or "").lower()
    is_pdf = file.content_type in {"application/pdf", "application/octet-stream"} or filename.endswith(".pdf")
    if not is_pdf:
        raise HTTPException(status_code=415, detail="Only PDF files are supported.")

    data = await file.read()
    if not data:
        raise HTTPException(status_code=400, detail="Uploaded file is empty.")
    if len(data) > settings.max_upload_mb * 1024 * 1024:
        raise HTTPException(
            status_code=413,
            detail=f"File exceeds the {settings.max_upload_mb} MB limit.",
        )

    text = extract_text(data)
    if not text:
        raise HTTPException(
            status_code=422,
            detail="No text layer found. Scanned / image-only PDFs are not supported in V1.",
        )

    if not settings.openai_api_key:
        raise HTTPException(status_code=500, detail="OPENAI_API_KEY is not configured.")

    try:
        return extract_invoice(text)
    except Exception as exc:  # noqa: BLE001
        raise HTTPException(status_code=502, detail=f"Extraction failed: {exc}") from exc
