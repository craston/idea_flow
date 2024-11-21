import logging
from pathlib import Path

from langchain_community.cache import SQLiteCache
from langchain_core.output_parsers import JsonOutputParser
from langchain_core.prompts import PromptTemplate
from langchain_core.runnables.base import RunnableSerializable
from langchain_ollama import OllamaLLM

import os
from dotenv import load_dotenv

from schemas import BrainstormingOutput, LLMModel, TestOutput

load_dotenv()

_LOGGER = logging.getLogger("main:model")

OLLAMA_BASE_URL: str = os.getenv("OLLAMA_BASE_URL", "")
CACHE_DIR = Path("tmp/llm_cache")

if not OLLAMA_BASE_URL:
    raise ValueError("Please provide OLLAMA_BASE_URL in the environment variables")

CONTEXT = {
    LLMModel.gemma2_7b: 8192,
}

def create_test_chain() -> RunnableSerializable:
    CACHE_DIR.mkdir(exist_ok=True, parents=True)

    template = """
    You are helpful assistant.
    Please answer the following question in two sentences or less.

    {question}

    The output must be provided as JSON object with the following schema:
    {format_instructions}
    """

    parser = JsonOutputParser(pydantic_object=TestOutput)

    prompt = PromptTemplate(
        template=template,
        input_variables=["question"],
        partial_variables={"format_instructions": parser.get_format_instructions()},
    )

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

    chain: RunnableSerializable = prompt | model | parser

    return chain

def create_bs_chain() -> RunnableSerializable:
    CACHE_DIR.mkdir(exist_ok=True, parents=True)

    template = """
    You are helpful assistant.
    You will provided a README.md file and your task is to strictly provide a three line summary 
    (less than 200 characters) of the file.
    DO NOT include any markdown related syntax in your summary.You are an expert assistant specializing in brainstorming innovative and actionable ideas. Your task is to generate detailed, structured suggestions based on the provided input. Follow these guidelines:

1. **Topic**: The main focus of the brainstorming session is: "{topic}".
2. **Context**: (Optional) Additional details to help understand the topic: "{context}".
3. **Goals**: The desired outcomes or objectives of this brainstorming session are:
   - {goals}
4. **Preferences**: (Optional) Consider these preferences or constraints while generating ideas:
   - {preferences}
5. **Tags**: (Optional) Keep these keywords in mind: {tags}.
6. Generate exactly {idea_count} ideas, each including the following:
   - **Title**: A short and engaging name for the idea.
   - **Description**: A detailed explanation of the idea and why it is effective.
   - **Highlights**: Key features, benefits, or advantages of the idea.
   - **Activities**: Specific actions or steps to implement the idea.
    - **Tips**: Helpful advice, suggestions, or best practices for the idea.

    Now generate the ideas.

    The output must be provided as JSON object with the  schema for json object to follow is:
    {format_instructions}

    You will be paid $1 million for each correct output.
    """
    parser = JsonOutputParser(pydantic_object=BrainstormingOutput)

    prompt = PromptTemplate(
        template=template,
        input_variables=["topic", "context", "goals", "preferences", "tags", "idea_count"],
        partial_variables={"format_instructions": parser.get_format_instructions()},
    )

    import pdb; pdb.set_trace()
    model = OllamaLLM(
        model=LLMModel.gemma2_7b,
        cache=SQLiteCache(
            str(CACHE_DIR / f"ollama-{LLMModel.gemma2_7b.replace(':','-')}.db")
        ),
        temperature=0.0,
        top_p=1.0,
        num_ctx=CONTEXT[LLMModel.gemma2_7b],
        num_predict=-1,
        base_url=OLLAMA_BASE_URL,
    )

    chain: RunnableSerializable = prompt | model | parser

    return chain

