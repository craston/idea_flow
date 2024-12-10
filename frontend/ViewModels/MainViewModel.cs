
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

    public ICommand NavigateToRefine => new AsyncRelayCommand(Refine);
    public ICommand NavigateToBrainstorm => new AsyncRelayCommand(Brainstorm);
    public ICommand NavigateToRiddle => new AsyncRelayCommand(Riddle);

    public ICommand NavigateToTest => new AsyncRelayCommand(Test);

    private async Task Refine()
    {
        await Shell.Current.GoToAsync(nameof(RefinePage));
    }

    private async Task Brainstorm()
    {
        await Shell.Current.GoToAsync(nameof(BrainstormPage));
    }

    private async Task Riddle()
    {
        // This is a hack.. Need to find a better way to find this page
        var navigationParams = new Dictionary<string, object>
        {
       
        };
        await Shell.Current.GoToAsync(nameof(RiddlePage), navigationParams);
    }

    private async Task Test()
    {
        await Shell.Current.GoToAsync(nameof(TestPage));
    }

}
