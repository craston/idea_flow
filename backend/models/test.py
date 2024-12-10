from typing import Callable
from langchain_core.chat_history import BaseChatMessageHistory
from langchain_core.output_parsers import JsonOutputParser
from langchain_core.prompts import PromptTemplate
from langchain_core.runnables.base import RunnableSerializable

from backend.models.base import create_chain, create_chat_chain
from backend.prompts.test import TEST_PROMPT, TEST_PROMPT_WITH_HISTORY
from backend.schemas import Basic


def create_test_chain() -> RunnableSerializable:
    parser = JsonOutputParser(pydantic_object=Basic)

    prompt = PromptTemplate(
        template=TEST_PROMPT,
        input_variables=["question"],
        partial_variables={"format_instructions": parser.get_format_instructions()},
    )

    return create_chain(prompt, parser)


def create_test_chain_with_history(
    session_history: Callable,
) -> RunnableSerializable:
    parser = JsonOutputParser(pydantic_object=Basic)

    prompt = PromptTemplate(
        template=TEST_PROMPT_WITH_HISTORY,
        input_variables=["history", "question"],
        partial_variables={"format_instructions": parser.get_format_instructions()},
    )

    return create_chat_chain(prompt, parser, session_history)
