from langchain_core.output_parsers import JsonOutputParser
from langchain_core.prompts import PromptTemplate
from langchain_core.runnables.base import RunnableSerializable

from backend.models.base import create_chain
from backend.prompts.brainstorm import (
    BRAINSTORM_CONTEXT_PROMPT,
    BRAINSTORM_GOALS_PROMPT,
    BRAINSTORM_IDEA_CHAT_PROMPT,
    BRAINSTORM_PREFERENCES_PROMPT,
    BRAINSTORM_PROMPT,
    BRAINSTORM_TAGS_PROMPT,
)
from schemas import (
    Basic,
    BrainstormingExamplesOutput,
    BrainstormingOutput,
)


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

    return create_chain(prompt, parser)


def create_bs_context_chain() -> RunnableSerializable:
    parser = JsonOutputParser(pydantic_object=BrainstormingExamplesOutput)

    prompt = PromptTemplate(
        template=BRAINSTORM_CONTEXT_PROMPT,
        input_variables=["topic"],
        partial_variables={"format_instructions": parser.get_format_instructions()},
    )

    return create_chain(prompt, parser)


def create_bs_goals_chain() -> RunnableSerializable:
    parser = JsonOutputParser(pydantic_object=BrainstormingExamplesOutput)

    prompt = PromptTemplate(
        template=BRAINSTORM_GOALS_PROMPT,
        input_variables=["topic", "context"],
        partial_variables={"format_instructions": parser.get_format_instructions()},
    )

    return create_chain(prompt, parser)


def create_bs_preferences_chain() -> RunnableSerializable:
    parser = JsonOutputParser(pydantic_object=BrainstormingExamplesOutput)

    prompt = PromptTemplate(
        template=BRAINSTORM_PREFERENCES_PROMPT,
        input_variables=["topic", "context", "goals"],
        partial_variables={"format_instructions": parser.get_format_instructions()},
    )

    return create_chain(prompt, parser)


def create_bs_tags_chain() -> RunnableSerializable:
    parser = JsonOutputParser(pydantic_object=BrainstormingExamplesOutput)

    prompt = PromptTemplate(
        template=BRAINSTORM_TAGS_PROMPT,
        input_variables=["topic", "context", "goals", "preferences"],
        partial_variables={"format_instructions": parser.get_format_instructions()},
    )

    return create_chain(prompt, parser)


def create_bs_chat_chain() -> RunnableSerializable:
    parser = JsonOutputParser(pydantic_object=Basic)

    prompt = PromptTemplate(
        template=BRAINSTORM_IDEA_CHAT_PROMPT,
        input_variables=["title", "description", "highlights", "activities", "tips"],
        partial_variables={"format_instructions": parser.get_format_instructions()},
    )

    return create_chain(prompt, parser)
