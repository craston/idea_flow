from langchain_core.output_parsers import JsonOutputParser
from langchain_core.prompts import PromptTemplate
from langchain_core.runnables.base import RunnableSerializable

from backend.models.base import create_chain
from backend.prompts.riddle import (
    RIDDLE_CHECK_ANSWER_PROMPT,
    RIDDLE_PROMPT,
)
from backend.schemas import (
    LLMModel,
    RiddleCheckAnswerOutput,
    RiddleOutput,
)


def create_riddle_chain() -> RunnableSerializable:
    parser = JsonOutputParser(pydantic_object=RiddleOutput)

    prompt = PromptTemplate(
        template=RIDDLE_PROMPT,
        partial_variables={"format_instructions": parser.get_format_instructions()},
    )

    return create_chain(
        prompt, parser, temperature=1.0, use_cache=False, model=LLMModel.gemma2_9b
    )


def create_riddle_check_answer() -> RunnableSerializable:
    parser = JsonOutputParser(pydantic_object=RiddleCheckAnswerOutput)

    prompt = PromptTemplate(
        template=RIDDLE_CHECK_ANSWER_PROMPT,
        input_variables=["riddle_question", "riddle_answer", "user_answer"],
        partial_variables={
            "format_instructions": parser.get_format_instructions(),
        },
    )

    return create_chain(prompt, parser)
