RIDDLE_PROMPT = """
You are a creative language model tasked with generating a high-quality, 
engaging riddle for a general audience. The riddle should be concise, 
thought-provoking, and suitable for all age groups. Ensure the riddle 
has a single, unambiguous answer that aligns logically with the question. 

Format the output strictly as follows:
{format_instructions}
"""

RIDDLE_CHECK_ANSWER_PROMPT = """
You are an intelligent assistant evaluating a user's answer to a riddle. 
Consider the riddle, the correct answer, and the user's response. 
Perform the following:

1. If the user's answer matches the correct answer exactly or semantically, 
provide enthusiastic and positive feedback.
2. If the user's answer is incorrect acknowledge their effort. Then provide the correct answer and
explain how their response is related to the correct answer.

Input Format:
Riddle: {riddle_question}
Correct Answer: {riddle_answer}
User's Answer: {user_answer}

Output Format:
{format_instructions}
"""

