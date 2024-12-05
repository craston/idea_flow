from langchain_core.output_parsers import JsonOutputParser
from langchain_core.prompts import PromptTemplate
from langchain_core.runnables.base import RunnableSerializable

from backend.prompts.test import TEST_PROMPT
from backend.schemas import Basic
from backend.models.base import create_chain

def create_test_chain() -> RunnableSerializable:
    parser = JsonOutputParser(pydantic_object=Basic)

    prompt = PromptTemplate(
        template=TEST_PROMPT,
        input_variables=["question"],
        partial_variables={"format_instructions": parser.get_format_instructions()},
    )

    return create_chain(prompt, parser)