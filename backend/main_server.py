from __future__ import annotations

import logging
from contextlib import asynccontextmanager

from fastapi import FastAPI
from pydantic import Field
from pydantic.networks import IPv4Address
from pydantic_settings import BaseSettings

from langchain_core.chat_history import BaseChatMessageHistory
from langchain_community.chat_message_histories import ChatMessageHistory

from backend.models.brainstorm import (
    create_bs_chain,
    create_bs_chat_chain,
    create_bs_context_chain,
    create_bs_goals_chain,
    create_bs_preferences_chain,
    create_bs_tags_chain,
)
from backend.models.refine import create_idea_refine_chain
from backend.models.riddle import create_riddle_chain, create_riddle_check_answer
from backend.models.test import create_test_chain, create_test_chain_with_history
from schemas import IdeaDetail

_LOGGER = logging.getLogger("main:server")


class Config(BaseSettings):
    port: int = Field(default=8000)
    host: IPv4Address = Field(default="0.0.0.0")
    mangum_base_path: str = ""
    fastapi_root_path: str = ""


settings = Config()

store = {}


def get_session_history(session_id: str) -> BaseChatMessageHistory:
    if session_id not in store:
        store[session_id] = ChatMessageHistory()
    return store[session_id]


@asynccontextmanager
async def lifespan(app: FastAPI):
    # initialize the
    yield


app = FastAPI(
    title="Idea Flow API",
    version="0.1.0",
    lifespan=lifespan,
    root_path=settings.fastapi_root_path,
)


@app.get("/test")
def test(question: str):
    chain = create_test_chain()

    try:
        llm_response = chain.invoke(
            {
                "question": question,
            }
        )
    except Exception as e:
        return {"error": str(e)}

    return llm_response


@app.get("/test_with_history")
def test_with_history(question: str):
    chain = create_test_chain_with_history(get_session_history)

    try:
        llm_response = chain.invoke(  # noqa: T201
            {"question": question},
            config={"configurable": {"session_id": "foo"}},
        )
    except Exception as e:
        return {"error": str(e)}

    return llm_response


@app.get("/brainstorm")
def brainstorm(
    topic: str,
    context: str = "",
    goals: str = "",
    preferences: str = "",
    tags: str = "",
    idea_count: int = 5,
):
    chain = create_bs_chain()

    try:
        llm_responses = chain.invoke(
            {
                "topic": topic,
                "context": context,
                "goals": goals,
                "preferences": preferences,
                "tags": tags,
                "idea_count": idea_count,
            }
        )
    except Exception as e:
        return {"error": str(e)}

    return llm_responses


@app.get("/brainstorm_context")
def brainstorm_context(topic: str):
    chain = create_bs_context_chain()

    try:
        llm_response = chain.invoke(
            {
                "topic": topic,
            }
        )
    except Exception as e:
        return {"error": str(e)}

    return llm_response


@app.get("/brainstorm_goals")
def brainstorm_goals(topic: str, context: str):
    chain = create_bs_goals_chain()

    try:
        llm_response = chain.invoke(
            {
                "topic": topic,
                "context": context,
            }
        )
    except Exception as e:
        return {"error": str(e)}

    return llm_response


@app.get("/brainstorm_preferences")
def brainstorm_preferences(topic: str, context: str, goals: str):
    chain = create_bs_preferences_chain()

    try:
        llm_response = chain.invoke(
            {
                "topic": topic,
                "context": context,
                "goals": goals,
            }
        )

    except Exception as e:
        return {"error": str(e)}

    return llm_response


@app.get("/brainstorm_tags")
def brainstorm_tags(topic: str, context: str, goals: str, preferences: str):
    chain = create_bs_tags_chain()

    try:
        llm_response = chain.invoke(
            {
                "topic": topic,
                "context": context,
                "goals": goals,
                "preferences": preferences,
            }
        )
    except Exception as e:
        return {"error": str(e)}

    return llm_response


@app.post("/idea_chat")
def brainstorm_idea_chat(
    brainstorm_topic: str,
    question: str,
    idea: IdeaDetail,
):
    chain = create_bs_chat_chain()

    try:
        llm_response = chain.invoke(
            {
                "topic": brainstorm_topic,
                "title": idea.Title,
                "description": idea.description,
                "highlights": idea.highlights,
                "activities": idea.activities,
                "tips": idea.tips,
                "question": question,
            }
        )
    except Exception as e:
        return {"error": str(e)}

    return llm_response


@app.get("/gen_riddle")
def gen_riddle():
    chain = create_riddle_chain()

    try:
        llm_response = chain.invoke(
            {},
        )
    except Exception as e:
        return {"error": str(e)}

    return llm_response


@app.get("/riddle_check_answer")
def riddle_check_answer(riddle_question: str, riddle_answer: str, user_answer: str):
    chain = create_riddle_check_answer()

    try:
        llm_response = chain.invoke(
            {
                "riddle_question": riddle_question,
                "riddle_answer": riddle_answer,
                "user_answer": user_answer,
            }
        )
    except Exception as e:
        return {"error": str(e)}

    return llm_response


if __name__ == "__main__":
    import uvicorn

    logging.basicConfig(level=logging.INFO)
    uvicorn.run(app, host=str(settings.host), port=settings.port)
