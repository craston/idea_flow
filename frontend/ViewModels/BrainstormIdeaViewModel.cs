

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

using frontend.Models;
using frontend.Views;
namespace frontend.ViewModels;

public partial class BrainstormIdeaViewModel : ObservableObject, IQueryAttributable
{
    private IdeaDetail? _idea;
    public IdeaDetail Idea
    {
        get => _idea!;
        set => SetProperty(ref _idea, value);
    }

    private string _topic;

    public string Topic
    {
        get => _topic;
        set => SetProperty(ref _topic, value);
    }

    private BrainstormingOutput? _output;
    public BrainstormIdeaViewModel()
    {

    }

    public ICommand BackCommand => new RelayCommand(Back);

    private void Back()
    {
        var navigationParams = new Dictionary<string, object>
        {
            ["Output"] = _output!
        };

        Shell.Current.GoToAsync(nameof(BrainstormChatPage), navigationParams);
    }
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var value = query["Idea"] as IdeaDetail;  // Dealing with
                                                  // CS8601 - Possible null reference assignment
        Idea = value!;

        _output = query["Brainstorm_output"] as BrainstormingOutput;
        Topic = "Generated Ideas for " + _output!.Topic;

    }
}

    
        
