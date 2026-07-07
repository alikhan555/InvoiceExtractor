"""Runtime configuration, loaded from the environment / .env file."""

from pydantic_settings import BaseSettings, SettingsConfigDict


class Settings(BaseSettings):
    model_config = SettingsConfigDict(
        env_file=".env",
        env_file_encoding="utf-8",
        extra="ignore",
    )

    # OpenAI credentials + model. Keys live in .env (git-ignored).
    openai_api_key: str = ""
    openai_model: str = "gpt-4o-mini"

    # Reject uploads larger than this many megabytes.
    max_upload_mb: int = 10


settings = Settings()
