from langchain_core.output_parsers import JsonOutputParser
from langchain_core.prompts import PromptTemplate
from langchain_core.runnables.base import RunnableSerializable

from backend.models.base import create_chain
from backend.prompts.riddle import (
    RIDDLE_ANSWER_PROMPT,
    RIDDLE_CHECK_ANSWER_PROMPT,
    RIDDLE_PROMPT,
)
from backend.schemas import RiddleAnswerOutput, RiddleCheckAnswerOutput, RiddleOutput


def create_riddle_chain() -> RunnableSerializable:
    parser = JsonOutputParser(pydantic_object=RiddleOutput)

    prompt = PromptTemplate(
        template=RIDDLE_PROMPT,
        partial_variables={"format_instructions": parser.get_format_instructions()},
    )

    return create_chain(prompt, parser, temperature=0.8, use_cache=False)
