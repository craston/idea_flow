REFINE_IDEA_PROMPT = """"
Analyze the following idea for its strengths, weaknesses, and provide actionable 
suggestions for improvement. Be specific and insightful:
Idea: {idea}

The output must follow the format below:
{format_instructions}
Example Input:
I want to start a business selling personalized eco-friendly stationery.

Example Output:
Strengths:
1. Appeals to eco-conscious consumers, a growing market segment.
2. Personalization adds a unique selling point compared to mass-produced options.

Weaknesses:
1. High competition from established brands in the eco-friendly product market.
2. Personalization might increase production costs and complexity.

Suggestions:
1. Focus on niche markets like small businesses or event planners to differentiate yourself.
2. Partner with local artists to add exclusive designs, creating a value proposition.
3. Develop a cost-effective production process for personalization to maintain profitability.
"""

REFINE_IDEA_PROMPT_2 = """
Using the reply to the orginal idea, answer the follow-up question:
Original idea: {idea}
Reply: {org_reply}
Follow-up Question: {question}
Conversation History: {history}

The output must follow the format below:
{format_instructions}
"""

REFINE_DRAFT_PROMPT = """
Refine the following text for clarity, simplicity, and engagement. 
Ensure it matches the intended tone (e.g., professional, casual, persuasive) and improves readability:
Draft: {draft}
Tone: {tone}

Example Input:
Draft: Our company is dedicated to providing quality products that meet customer needs.
Tone: Persuasive

Example Output:
We’re passionate about delivering exceptional products designed to exceed your expectations.

"""

REFINE_PLANNING_PROMPT = """
Break down the following goal into actionable steps with a timeline, milestones, and potential risks. 
Provide practical and detailed outputs:
Goal: {goal}

Example Input:
I want to write a book about personal finance for young professionals.

Example Output:
Actionable Steps:
1. Research key personal finance topics relevant to young professionals (1–2 weeks).
2. Outline chapters and key takeaways for the book (1 week).
3. Write one chapter per week, focusing on actionable advice and relatable examples (12 weeks).
4. dit and refine the draft for flow, accuracy, and engagement (2 weeks).
5. Design and format the book for publication (1–2 weeks).

Timeline: 16–18 weeks total.

Milestones:
Week 2: Complete chapter outlines.
Week 6: Finish the first four chapters.
Week 12: Complete all chapters.
Week 16: Finalize editing and formatting.

Potential Risks:
Struggling to find enough time to write consistently—mitigate by setting aside dedicated writing hours daily.
"Difficulty in sourcing credible examples—resolve by consulting experts or referring to industry reports.
These prompts ensure high-quality, actionable outputs tailored to the user’s needs for idea refinement, draft 
improvement, and project planning, aligning well with the AI-powered application’s functionality.

"""
