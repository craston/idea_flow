BRAINSTORM_PROMPT = """
    You are helpful assistant.
    Your task is to generate creative ideas for a brainstorming session. 
    You will be provided with a topic, context, goals, preferences, and tags to guide your idea generation process.
    
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

BRAINSTORM_CONTEXT_PROMPT = """
    You are helpful assistant.
    You are provided with a brainstorming topic. Your task is to provide a 
    list of 5 possible contexts or scenarios related to the topic. Keep the descriptions of the
    contexts brief (not more the 5 words) and relevant to the topic.

    The topic for brainstorming is: "{topic}".
    Generate 5 different contexts or scenarios related to this topic.

    The output must be provided as JSON object with the following schema:
    {format_instructions}
    """

BRAINSTORM_GOALS_PROMPT = """
    You are helpful assistant.
    You are provided with a brainstorming topic. Your task is to provide a 
    list of 5 possible goals or objectives related to the topic. Keep the descriptions of the
    goals brief (not more the 3 words) and relevant to the topic.

    The topic for brainstorming is: "{topic}".
    The context for brainstorming is: "{context}".
    Generate 5 different goals or objectives related to this topic.

    The output must be provided as JSON object with the following schema:
    {format_instructions}
    """

BRAINSTORM_PREFERENCES_PROMPT = """
    You are helpful assistant.
    You are provided with a brainstorming topic, along with its context and goals. Your task is to provide a
    list of 5 possible preferences or constraints related to the topic (e.g., 'budget-friendly', 'adventurous'). 
    Keep the descriptions of the
    preferences brief (not more the 3 words) and relevant to the topic.
    
    The topic for brainstorming is: "{topic}".
    The context for brainstorming is: "{context}".
    The goals for brainstorming are: "{goals}".
    Generate 5 different preferences or constraints related to this topic.

    The output must be provided as JSON object with the following schema:
    {format_instructions}
    """

BRAINSTORM_TAGS_PROMPT = """
    You are helpful assistant.
    You are provided with a brainstorming topic, along with its context, goals, and preferences. Your task is to provide a
    list of 5 possible tags or keywords related to the topic. Keep the descriptions of the
    tags brief (not more the 2 words) and relevant to the topic.

    The topic for brainstorming is: "{topic}".
    The context for brainstorming is: "{context}".
    The goals for brainstorming are: "{goals}".
    The preferences for brainstorming are: "{preferences}".
    Generate 5 different tags or keywords related to this topic.

    The output must be provided as JSON object with the following schema:
    {format_instructions}
    """

BRAINSTORM_IDEA_CHAT_PROMPT = """
    You are helpful assistant.
    You are provided with an idea generated using a brainstorm topic on {topic}. Your task is 
    to answer the question provided by the user using the following information on the idea:
    - **Title**: {title}
    - **Description**: {description}
    - **Highlights**: {highlights}
    - **Activities**: {activities}
    - **Tips**: {tips}

    The user question is: "{question}".

    The output must be provided as JSON object with the following schema:
    {format_instructions}
    """
