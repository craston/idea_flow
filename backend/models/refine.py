from typing import Callable
from langchain_core.output_parsers import JsonOutputParser
from langchain_core.prompts import PromptTemplate
from langchain_core.runnables.base import RunnableSerializable

from backend.models.base import create_chain, create_chat_chain

from backend.prompts.refine import REFINE_IDEA_PROMPT, REFINE_IDEA_PROMPT_2
from backend.schemas import RefineIdeaOutput, Basic 


def create_idea_refine_chain() -> RunnableSerializable:
    parser = JsonOutputParser(pydantic_object=RefineIdeaOutput)

    prompt = PromptTemplate(
        template=REFINE_IDEA_PROMPT,
        input_variables=["idea"],
        partial_variables={"format_instructions": parser.get_format_instructions()},
    )

    return create_chain(prompt, parser)

def create_idea_refine_chat_chain(sesssion_history: Callable) -> RunnableSerializable:
    parser = JsonOutputParser(pydantic_object=Basic)

    prompt = PromptTemplate(
        template=REFINE_IDEA_PROMPT_2,
        input_variables=["idea", "org_reply", "question", "history"],
        partial_variables={"format_instructions": parser.get_format_instructions()},
    )

    return create_chat_chain(prompt, parser, session_history=sesssion_history)