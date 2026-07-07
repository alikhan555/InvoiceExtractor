# Invoice Extractor (V1)

Ingest a single text-based PDF invoice and return clean, structured, validated
data. Three services:

| Service | Stack | Responsibility |
|---|---|---|
| **UI** | React + Vite + TypeScript | Upload screen, results view |
| **API** | ASP.NET Core 10, Clean Architecture, EF Core | Orchestration, validation, storage |
| **AIEngine** | Python 3.12, FastAPI, LangChain, OpenAI (managed with **uv**) | PDF text extraction → LLM → JSON |

```
UI (React)  ─▶  API (.NET Core)  ─▶  AIEngine (FastAPI)
            ◀─  Clean Arch + EF  ◀─
                     │
                     ▼
                 PostgreSQL
```

The AIEngine is stateless: a PDF goes in, JSON comes out. All state and storage
live in the .NET API.

---

## Quick start (Docker — everything at once)

Requires Docker + an OpenAI API key.

```bash
cp .env.example .env      # then edit .env and set OPENAI_API_KEY
docker compose up --build
```

Then open:

- **UI** → http://localhost:5173
- **API / Swagger** → http://localhost:5080/swagger
- **AIEngine / docs** → http://localhost:8000/docs

Postgres schema is created automatically (the API applies EF migrations on
startup, retrying while the database comes up).

---

## Run locally (without Docker)

Prereqs: .NET 10 SDK, Node 18+, [uv](https://docs.astral.sh/uv/), and a
PostgreSQL instance. The quickest way to get Postgres is:

```bash
docker compose up -d postgres
```

**1. AIEngine**

```bash
cd AIEngine
uv sync
cp .env.example .env        # set OPENAI_API_KEY
uv run uvicorn app.main:app --reload --port 8000
```

**2. API**

```bash
cd API
dotnet run --project InvoiceExtractor.Api    # http://localhost:5080
```

**3. UI**

```bash
cd UI
npm install
npm run dev                 # http://localhost:5173
```

---

## Try it

Generate a sample text-based invoice PDF and upload it via the UI, or straight
to the API:

```bash
cd AIEngine && uv run python scripts/make_sample_invoice.py   # -> samples/sample_invoice.pdf

curl -F "file=@AIEngine/samples/sample_invoice.pdf" http://localhost:5080/api/invoices/upload
```

---

## API endpoints

```
POST /api/invoices/upload   multipart "file" (text-based PDF) → extract → store → result
GET  /api/invoices/{id}     one invoice + line items
GET  /api/invoices          list invoices
```

## Data captured

- **Header:** vendor, invoice number, invoice date, due date, currency,
  subtotal, tax, total
- **Line items:** description, quantity, unit price, line amount
- **Validation** (surfaced as warnings, invoice is stored regardless):
  required fields present, `subtotal + tax ≈ total`, `Σ line amounts ≈ subtotal`

## Scope

V1 only: single text-based PDF per call, no OCR/images, no auth, no batch, no
export. See the per-service READMEs in `AIEngine/`, `API/`, and `UI/`.
