
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using frontend.Views;
namespace frontend.ViewModels;
public partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {

    }

    public ICommand NavigateToConversation => new AsyncRelayCommand(Conversation);
    public ICommand NavigateToBrainstorm => new AsyncRelayCommand(Brainstorm);
    public ICommand NavigateToPuzzles => new AsyncRelayCommand(Puzzles);

    public ICommand NavigateToTest => new AsyncRelayCommand(Test);

    private async Task Conversation()
    {
        await Shell.Current.GoToAsync(nameof(ConversationPage));
    }

    private async Task Brainstorm()
    {
        await Shell.Current.GoToAsync(nameof(BrainstormPage));
    }

    private async Task Puzzles()
    {
        await Shell.Current.GoToAsync(nameof(PuzzlesPage));
    }

    private async Task Test()
    {
        await Shell.Current.GoToAsync(nameof(TestPage));
    }

}
