// Base URL of the .NET API. In dev it defaults to the local API port; in a
// production build it falls back to a relative path so a reverse proxy (nginx)
// can forward /api to the API container. Override with VITE_API_URL.
export const BASE =
  import.meta.env.VITE_API_URL ?? (import.meta.env.DEV ? "http://localhost:5080" : "");

async function errorMessage(res: Response, fallback: string): Promise<string> {
  try {
    const body = await res.json();
    if (body && typeof body.error === "string") return body.error;
  } catch {
    // no JSON body
  }
  return `${fallback} (${res.status})`;
}

export async function apiGet<T>(path: string): Promise<T> {
  const res = await fetch(`${BASE}${path}`);
  if (!res.ok) throw new Error(await errorMessage(res, "Request failed"));
  return (await res.json()) as T;
}

export async function apiPostForm<T>(path: string, form: FormData): Promise<T> {
  const res = await fetch(`${BASE}${path}`, { method: "POST", body: form });
  if (!res.ok) throw new Error(await errorMessage(res, "Upload failed"));
  return (await res.json()) as T;
}
