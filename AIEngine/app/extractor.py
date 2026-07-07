"""Turn raw invoice text into a structured InvoiceExtraction via the LLM.

Uses LangChain's structured-output binding so the model is constrained to the
schema. Retries once on malformed / failed output, then gives up.
"""

from langchain_core.prompts import ChatPromptTemplate
from langchain_openai import ChatOpenAI

from .config import settings
from .schemas import InvoiceExtraction

SYSTEM_PROMPT = """You are an invoice data extraction engine.

You are given the raw text of a single invoice. Extract the fields defined by
the provided schema and nothing else.

Rules:
- Return ONLY structured data matching the schema. No prose, no markdown.
- If a field is not present in the text, return null. Do NOT guess or invent.
- Dates MUST be formatted as strict YYYY-MM-DD. If a date cannot be parsed with
  confidence, return null for it.
- Numbers must be plain numeric values (no currency symbols, no thousands
  separators). Put the currency in the `currency` field instead.
- `lineItems` should contain one entry per billed line. If there are no
  discernible line items, return an empty list.
"""


def _build_structured_llm():
    llm = ChatOpenAI(
        model=settings.openai_model,
        api_key=settings.openai_api_key,
        temperature=0,
    )
    return llm.with_structured_output(InvoiceExtraction)


def extract_invoice(text: str) -> InvoiceExtraction:
    """Extract structured invoice data from raw text, retrying once on failure."""
    structured_llm = _build_structured_llm()
    prompt = ChatPromptTemplate.from_messages(
        [
            ("system", SYSTEM_PROMPT),
            ("human", "Invoice text:\n\n{invoice_text}"),
        ]
    )
    chain = prompt | structured_llm

    last_error: Exception | None = None
    for _attempt in range(2):  # initial try + one retry
        try:
            result = chain.invoke({"invoice_text": text})
            if isinstance(result, InvoiceExtraction):
                return result
            # Some providers may hand back a dict; coerce it.
            return InvoiceExtraction.model_validate(result)
        except Exception as exc:  # noqa: BLE001 - surface any provider error
            last_error = exc

    raise RuntimeError(f"LLM extraction failed after retry: {last_error}")
