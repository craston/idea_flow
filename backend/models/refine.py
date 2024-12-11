from langchain_core.output_parsers import JsonOutputParser
from langchain_core.prompts import PromptTemplate
from langchain_core.runnables.base import RunnableSerializable

from backend.models.base import create_chain

from backend.prompts.refine import REFINE_IDEA_PROMPT
from backend.schemas import RefineIdeaOutput


def create_idea_refine_chain() -> RunnableSerializable:
    parser = JsonOutputParser(pydantic_object=RefineIdeaOutput)

    prompt = PromptTemplate(
        template=REFINE_IDEA_PROMPT,
        input_variables=["idea"],
        partial_variables={"format_instructions": parser.get_format_instructions()},
    )

    return create_chain(prompt, parser)