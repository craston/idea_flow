TEST_PROMPT = """
    You are helpful assistant.
    Please answer the following question in two sentences or less.

    {question}

    The output must be provided as JSON object with the following schema:
    {format_instructions}
    """

TEST_PROMPT_WITH_HISTORY = """
    You are helpful assistant.
    Using the following history, please answer the following question in two sentences or less.

    History = {history}
    Question = {question}

    The output must be provided as JSON object with the following schema:
    {format_instructions}
    """