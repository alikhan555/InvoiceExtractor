# AIEngine

Stateless FastAPI service. A text-based PDF goes in, structured invoice JSON
comes out. No storage, no state — all persistence lives in the .NET API.

Managed with [uv](https://docs.astral.sh/uv/).

## Setup

```bash
cd AIEngine
uv sync                       # create venv + install deps (uses .python-version = 3.12)
cp .env.example .env          # then edit .env and add your OPENAI_API_KEY
```

## Run

```bash
uv run uvicorn app.main:app --reload --port 8000
```

- `GET  /health`  — liveness + configured model
- `POST /extract` — multipart `file` (a text-based PDF) → structured JSON
- Swagger UI at http://localhost:8000/docs

## Test standalone

Generate a sample invoice PDF and post it:

```bash
uv run python scripts/make_sample_invoice.py         # -> samples/sample_invoice.pdf
curl -F "file=@samples/sample_invoice.pdf" http://localhost:8000/extract
```

Offline tests (no API key needed):

```bash
uv run pytest
```

## Response schema

```json
{
  "vendorName": "Acme Widgets Ltd.",
  "invoiceNumber": "INV-2026-0042",
  "invoiceDate": "2026-07-01",
  "dueDate": "2026-07-31",
  "currency": "USD",
  "subtotal": 355.0,
  "tax": 35.5,
  "total": 390.5,
  "lineItems": [
    { "description": "Blue Widget", "quantity": 10, "unitPrice": 12.5, "lineAmount": 125.0 }
  ]
}
```

Fields not found in the PDF come back as `null`. The engine never guesses.
