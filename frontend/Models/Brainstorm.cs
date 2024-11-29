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

    public class IdeaDetail: ObservableObject
    { 
        public string? Title { get; set; }
        public string? Description { get; set; }
        public List<string> Highlights { get; set; } = [];
        public List<string> Activities { get; set; } = [];
        public List<string> Tips { get; set; } = [];

        private bool _isSaved;
        private string _imgSource = "heart.png";
        public bool IsSaved
        {
            get => _isSaved;
            set => SetProperty(ref _isSaved, value);
        }

        public string ImgSource
        {
            get => _imgSource;
            set => SetProperty(ref _imgSource, value);
        }
        public ICommand ToggleSaveCommand => new RelayCommand(ToggleSave);
        private void ToggleSave()
        {
            IsSaved = !IsSaved;

            ImgSource = IsSaved ? "heart_filled.png" : "heart.png";
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
}
