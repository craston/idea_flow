using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace frontend.ViewModels;

public partial class RiddleViewModel: ObservableObject
{
    private string _riddle;
    private string _answer;
    private string _userAnswer;
    private string _feedback;
    private bool _isFeedbackVisible;

    public string Riddle
    {
        get => _riddle;
        set => SetProperty(ref _riddle, value);
    }

    public string UserAnswer
    {
        get => _userAnswer;
        set => SetProperty(ref _userAnswer, value);
    }

    public string Feedback
    {
        get => _feedback;
        set => SetProperty(ref _feedback, value);
    }

    public bool IsFeedbackVisible
    {
        get => _isFeedbackVisible;
        set => SetProperty(ref _isFeedbackVisible, value);
    }

    public ICommand SubmitAnswerCommand => new AsyncRelayCommand(SubmitAnswer);
    public ICommand RevealAnswerCommand => new AsyncRelayCommand(RevealAnswer);

    public RiddleViewModel()
    {
    }

    private async Task SubmitAnswer()
    {
        
    }

    private async Task RevealAnswer()
    {
        // Implement this method
    }
}

