"""The extraction contract. Field names are camelCase on purpose: they are the
wire format shared with the .NET API and the React UI, and FastAPI serialises
them verbatim. These same models drive LangChain's structured output, so the
`description` on each field doubles as guidance for the LLM."""

from pydantic import BaseModel, Field


class LineItem(BaseModel):
    description: str | None = Field(
        default=None, description="Text description of the line item."
    )
    quantity: float | None = Field(
        default=None, description="Quantity of units for this line."
    )
    unitPrice: float | None = Field(
        default=None, description="Price per single unit."
    )
    lineAmount: float | None = Field(
        default=None, description="Total amount for this line (quantity * unitPrice)."
    )


class InvoiceExtraction(BaseModel):
    """Structured invoice data extracted from a single text-based PDF."""

    vendorName: str | None = Field(
        default=None, description="Name of the vendor / supplier issuing the invoice."
    )
    invoiceNumber: str | None = Field(
        default=None, description="The invoice's unique identifier / number."
    )
    invoiceDate: str | None = Field(
        default=None, description="Invoice issue date in strict YYYY-MM-DD format."
    )
    dueDate: str | None = Field(
        default=None, description="Payment due date in strict YYYY-MM-DD format."
    )
    currency: str | None = Field(
        default=None, description="ISO currency code or symbol, e.g. USD, EUR, GBP."
    )
    subtotal: float | None = Field(
        default=None, description="Sum of line amounts before tax."
    )
    tax: float | None = Field(
        default=None, description="Total tax amount applied to the invoice."
    )
    total: float | None = Field(
        default=None, description="Grand total payable (subtotal + tax)."
    )
    lineItems: list[LineItem] = Field(
        default_factory=list, description="One entry per invoice line item."
    )
