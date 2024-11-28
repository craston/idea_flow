

using CommunityToolkit.Mvvm.ComponentModel;

using frontend.Models;

namespace frontend.ViewModels;

public partial class BrainstormIdeaViewModel : ObservableObject, IQueryAttributable
{
    private IdeaDetail? _idea;

    public IdeaDetail Idea
    {
        get => _idea!;
        set => SetProperty(ref _idea, value);
    }
    public BrainstormIdeaViewModel()
    {

    }
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var value = query["Idea"] as IdeaDetail;  // Dealing with
                                                  // CS8601 - Possible null reference assignment
        Idea = value!;

    }
}

    
        
