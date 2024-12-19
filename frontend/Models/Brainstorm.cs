using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace frontend.Models
{
    public class BrainstormInput
    {
        public string? Topic { get; set; }
        public string? Context { get; set; }
        public List<string> Goals { get; set; } = [];
        public List<string> Preferences { get; set; } = [];
        public List<string> Tags { get; set; } = [];
        public int Idea_count { get; set; } = 2;

    }

    public partial class IdeaDetail : ObservableObject
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public List<string> Highlights { get; set; } = [];
        public List<string> Activities { get; set; } = [];
        public List<string> Tips { get; set; } = [];

        public bool IsSaved { get; set; } = false;
        private string _imgSource = "heart.png";
        public string ImgSource
        {
            get => _imgSource;
            set => SetProperty(ref _imgSource, value);
        }
        public IdeaDetail()
        {
        }
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

    public class ChatMessage
    {
        public string? Content { get; set; } // The text of the message
        public bool IsUserMessage { get; set; } // True if the message is from the user
        public DateTime Timestamp { get; set; } // When the message was sent or received
    }

    public class IdeaRefineChat
    {
        public List<ChatMessage> Messages { get; set; } = [];

    }

}
