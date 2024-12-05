import logging
import os
from pathlib import Path

from dotenv import load_dotenv
from langchain_community.cache import SQLiteCache
from langchain_core.output_parsers import JsonOutputParser
from langchain_core.prompts import PromptTemplate
from langchain_core.runnables.base import RunnableSerializable
from langchain_ollama import OllamaLLM

from schemas import (
    LLMModel,
)

load_dotenv()

OLLAMA_BASE_URL: str = os.getenv("OLLAMA_BASE_URL", "")
CACHE_DIR = Path("tmp/llm_cache")

if not OLLAMA_BASE_URL:
    raise ValueError("Please provide OLLAMA_BASE_URL in the environment variables")

CONTEXT = {
    LLMModel.gemma2_7b: 8192,
}


def create_chain(
    prompt: PromptTemplate,
    output_parser: JsonOutputParser,
) -> RunnableSerializable:
    CACHE_DIR.mkdir(exist_ok=True, parents=True)

    model = OllamaLLM(
        model=LLMModel.gemma2_7b,
        cache=SQLiteCache(
            str(CACHE_DIR / f"ollama-{LLMModel.gemma2_7b.replace(':', '-')}.db")
        ),
        temperature=0.0,
        top_p=1.0,
        num_ctx=CONTEXT[LLMModel.gemma2_7b],
        num_predict=-1,
        base_url=OLLAMA_BASE_URL,
    )

    chain: RunnableSerializable = prompt | model | output_parser

    return chain