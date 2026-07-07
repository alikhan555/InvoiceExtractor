# UI (React + Vite + TypeScript)

Upload screen + results view for the Invoice Extractor.

## Setup

```bash
cd UI
npm install
```

## Run

```bash
npm run dev        # http://localhost:5173
```

In dev, the app talks to the API at `http://localhost:5080` by default. To point
elsewhere, copy `.env.example` to `.env` and set `VITE_API_URL`.

## Build

```bash
npm run build      # type-checks then bundles into dist/
npm run preview    # serve the production build locally
```

## Structure

```
src/
├── api/         fetch wrappers + invoice endpoints
├── components/  UploadForm, InvoiceResult, InvoiceList
├── types/       shared TypeScript models
├── utils/       formatting helpers
├── App.tsx      screen composition
└── main.tsx     entry point
```
