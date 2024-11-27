namespace frontend.Models
{
    public class BrainstormInput
    {
        public string? Topic { get; set; }
        public string? Context { get; set; }
        public List<string> Goals { get; set; } = [];
        public List<string> Preferences { get; set; } = [];
        public List<string> Tags { get; set; } = [];
        public int Idea_count { get; set; } = 5;
    }

    public class IdeaDetail
    { 
        public string? Title { get; set; }
        public string? Description { get; set; }
        public List<string> Highlights { get; set; } = [];
        public List<string> Activities { get; set; } = [];
        public List<string> Tips { get; set; } = [];
    }
    public class BrainstormingOutput
    {
        public string? Topic { get; set; }
        public List<IdeaDetail> Generated_ideas { get; set; } = [];
    }
    public class BrainstormExamplesOuput
    {
        public List<string> Examples { get; set; } = [];
    }
}
