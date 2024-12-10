import os
from pathlib import Path
from typing import Callable

from dotenv import load_dotenv
from langchain_community.cache import SQLiteCache
from langchain_core.output_parsers import JsonOutputParser
from langchain_core.prompts import PromptTemplate
from langchain_core.runnables.base import RunnableSerializable
from langchain_core.runnables.history import RunnableWithMessageHistory
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
    LLMModel.gemma2_9b: 8192,
}


def create_chain(
    prompt: PromptTemplate,
    output_parser: JsonOutputParser,
    temperature: float = 0.0,
    use_cache: bool = True,
    model: LLMModel = LLMModel.gemma2_9b,
) -> RunnableSerializable:
    CACHE_DIR.mkdir(exist_ok=True, parents=True)

    cache = SQLiteCache(
        str(CACHE_DIR / f"ollama-{LLMModel.gemma2_9b.replace(':', '-')}.db")
    )
    if not use_cache:
        cache = None

    model = OllamaLLM(
        model=model,
        cache=cache,
        temperature=temperature,
        top_p=1.0,
        num_ctx=CONTEXT[LLMModel.gemma2_9b],
        num_predict=-1,
        base_url=OLLAMA_BASE_URL,
    )

    chain: RunnableSerializable = prompt | model | output_parser

    return chain


def create_chat_chain(
    prompt: PromptTemplate,
    output_parser: JsonOutputParser,
    session_history: Callable,
    use_cache: bool = False,
    model: LLMModel = LLMModel.gemma2_9b,
    temperature: float = 0.0,
) -> RunnableSerializable:
    CACHE_DIR.mkdir(exist_ok=True, parents=True)

    cache = SQLiteCache(
        str(CACHE_DIR / f"ollama-{LLMModel.gemma2_9b.replace(':', '-')}.db")
    )
    if not use_cache:
        cache = None

    model = OllamaLLM(
        model=model,
        cache=cache,
        temperature=temperature,
        top_p=1.0,
        num_ctx=CONTEXT[LLMModel.gemma2_9b],
        num_predict=-1,
        base_url=OLLAMA_BASE_URL,
    )

    chain: RunnableSerializable = prompt | model | output_parser

    chain_with_history = RunnableWithMessageHistory(
        chain,
        session_history,
        input_messages_key="question",
        history_messages_key="history",
    )
    return chain_with_history
