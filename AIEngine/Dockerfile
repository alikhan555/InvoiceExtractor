# Stateless FastAPI extraction service, managed with uv.
FROM ghcr.io/astral-sh/uv:python3.12-bookworm-slim

WORKDIR /app

# Copy sources first (the project is a package, so uv needs `app/` to sync).
COPY . .

# Install runtime dependencies into a project venv (no dev group in the image).
RUN uv sync --no-dev

EXPOSE 8000

CMD ["uv", "run", "--no-dev", "uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "8000"]
