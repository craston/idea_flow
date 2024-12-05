import logging
import os
from pathlib import Path

from dotenv import load_dotenv
from langchain_community.cache import SQLiteCache
from langchain_core.output_parsers import JsonOutputParser
from langchain_core.prompts import PromptTemplate
from langchain_core.runnables.base import RunnableSerializable
from langchain_ollama import OllamaLLM

from backend.prompts.brainstorm import (
    BRAINSTORM_CONTEXT_PROMPT,
    BRAINSTORM_GOALS_PROMPT,
    BRAINSTORM_IDEA_CHAT_PROMPT,
    BRAINSTORM_PREFERENCES_PROMPT,
    BRAINSTORM_PROMPT,
    BRAINSTORM_TAGS_PROMPT,
)
from backend.prompts.test import TEST_PROMPT
from schemas import (
    BrainstormingExamplesOutput,
    BrainstormingOutput,
    LLMModel,
    TestOutput,
)

load_dotenv()

_LOGGER = logging.getLogger("main:model")

OLLAMA_BASE_URL: str = os.getenv("OLLAMA_BASE_URL", "")
CACHE_DIR = Path("tmp/llm_cache")

if not OLLAMA_BASE_URL:
    raise ValueError("Please provide OLLAMA_BASE_URL in the environment variables")

CONTEXT = {
    LLMModel.gemma2_7b: 8192,
}


def _create_chain(
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


def create_test_chain() -> RunnableSerializable:
    parser = JsonOutputParser(pydantic_object=TestOutput)

    prompt = PromptTemplate(
        template=TEST_PROMPT,
        input_variables=["question"],
        partial_variables={"format_instructions": parser.get_format_instructions()},
    )

    return _create_chain(prompt, parser)


def create_bs_chain() -> RunnableSerializable:
    parser = JsonOutputParser(pydantic_object=BrainstormingOutput)

    prompt = PromptTemplate(
        template=BRAINSTORM_PROMPT,
        input_variables=[
            "topic",
            "context",
            "goals",
            "preferences",
            "tags",
            "idea_count",
        ],
        partial_variables={"format_instructions": parser.get_format_instructions()},
    )

    return _create_chain(prompt, parser)


def create_bs_context_chain() -> RunnableSerializable:
    parser = JsonOutputParser(pydantic_object=BrainstormingExamplesOutput)

    prompt = PromptTemplate(
        template=BRAINSTORM_CONTEXT_PROMPT,
        input_variables=["topic"],
        partial_variables={"format_instructions": parser.get_format_instructions()},
    )

    return _create_chain(prompt, parser)


def create_bs_goals_chain() -> RunnableSerializable:
    parser = JsonOutputParser(pydantic_object=BrainstormingExamplesOutput)

    prompt = PromptTemplate(
        template=BRAINSTORM_GOALS_PROMPT,
        input_variables=["topic", "context"],
        partial_variables={"format_instructions": parser.get_format_instructions()},
    )

    return _create_chain(prompt, parser)


def create_bs_preferences_chain() -> RunnableSerializable:
    parser = JsonOutputParser(pydantic_object=BrainstormingExamplesOutput)

    prompt = PromptTemplate(
        template=BRAINSTORM_PREFERENCES_PROMPT,
        input_variables=["topic", "context", "goals"],
        partial_variables={"format_instructions": parser.get_format_instructions()},
    )

    return _create_chain(prompt, parser)


def create_bs_tags_chain() -> RunnableSerializable:
    parser = JsonOutputParser(pydantic_object=BrainstormingExamplesOutput)

    prompt = PromptTemplate(
        template=BRAINSTORM_TAGS_PROMPT,
        input_variables=["topic", "context", "goals", "preferences"],
        partial_variables={"format_instructions": parser.get_format_instructions()},
    )

    return _create_chain(prompt, parser)


def create_bs_chat_chain() -> RunnableSerializable:
    parser = JsonOutputParser(pydantic_object=TestOutput)

    prompt = PromptTemplate(
        template=BRAINSTORM_IDEA_CHAT_PROMPT,
        input_variables=["title", "description", "highlights", "activities", "tips"],
        partial_variables={"format_instructions": parser.get_format_instructions()},
    )

    return _create_chain(prompt, parser)
