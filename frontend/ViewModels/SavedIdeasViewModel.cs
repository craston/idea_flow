using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using frontend.Models;
using System.Threading.Tasks;

namespace frontend.ViewModels;

public partial class SavedIdeasViewModel : ObservableObject
{
    private readonly IdeaDatabase _database;
    private ObservableCollection<IdeaDetail> _savedIdeas;

    public ObservableCollection<IdeaDetail> SavedIdeas
    {
        get => _savedIdeas;
        set => SetProperty(ref _savedIdeas, value);
    }

    public SavedIdeasViewModel()
    {
        LoadSavedIdeas();
    }

    private async void LoadSavedIdeas()
    {
        var savedIdeas = await App.Database.GetIdeaDetailsAsync();
        SavedIdeas = new ObservableCollection<IdeaDetail>(savedIdeas);
    }
}
