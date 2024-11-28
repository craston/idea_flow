
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using frontend.Models;
using System.Web;
using Newtonsoft.Json;
using CommunityToolkit.Maui.Views;

using frontend.Views;

namespace frontend.ViewModels;

public partial class BrainstormViewModel : ObservableObject
{
    public BrainstormViewModel()
    {

        Examples = _defaultExamples;
        _getPrompt();
    }

    private readonly static string _baseAddress =
        DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:8000" : "http://localhost:8000";
    private readonly static string _bs_contextUrl = $"{_baseAddress}/brainstorm_context";
    private readonly static string _bs_goalsUrl = $"{_baseAddress}/brainstorm_goals";
    private readonly static string _bs_preferencesUrl = $"{_baseAddress}/brainstorm_preferences";
    private readonly static string _bs_tagsUrl = $"{_baseAddress}/brainstorm_tags";

    private readonly static string start = "Start Brainstorming";

    private readonly List<string> _defaultExamples = ["Vacation Ideas", "Team Building", "Product Ideas"];
    private string _currentPrompt = "";
    private string _currentInput = "";
    private string _nextText = "Next";
    private int _currentPromptIndex = 0;
    private List<string> _examples = [];
    private bool _isNextEnabled = false;

    SpinnerPopup spinner = new();

    private BrainstormInput _brainstormInput = new();
    public string CurrentPrompt
    {
        get => _currentPrompt;
        set => SetProperty(ref _currentPrompt, value);
    }

    public string CurrentInput
    {
        get => _currentInput;
        set
        {
            SetProperty(ref _currentInput, value);
            if (CurrentPromptIndex == 0 && !string.IsNullOrEmpty(CurrentInput))
            {
                IsNextEnabled = true;
            }
            else if (CurrentPromptIndex > 0)
            {
                IsNextEnabled = true;
            }
            else
            {
                IsNextEnabled = false;
            }

        }
    }
    public int CurrentPromptIndex
    {
        get => _currentPromptIndex;
        set => SetProperty(ref _currentPromptIndex, value);
    }
    public List<string> Examples
    {
        get => _examples;
        set => SetProperty(ref _examples, value);
    }

    public string NextText
    {
        get => _nextText;
        set => SetProperty(ref _nextText, value);
    }

    public bool IsNextEnabled
    {
        get => _isNextEnabled;
        set => SetProperty(ref _isNextEnabled, value);
    }
    private void _getPrompt()
    {
        switch (CurrentPromptIndex)
        {
            case 0: CurrentPrompt = "What's the topic?"; break;
            case 1: CurrentPrompt = "Do you want to provide a context? (Optional)"; break;
            case 2: CurrentPrompt = "Your goals? (Optional)"; break;
            case 3: CurrentPrompt = "Any preferences? (Optional)"; break;
            case 4: CurrentPrompt = "Add tags? (Optional)"; break;
        }
    }

    private void SaveInput()
    {
        switch (CurrentPromptIndex)
        {
            case 0: _brainstormInput.Topic = CurrentInput; break;
            case 1: _brainstormInput.Context = CurrentInput; break;
            case 2: _brainstormInput.Goals = new List<string>(CurrentInput.Split(',')); break;
            case 3: _brainstormInput.Preferences.Add(CurrentInput); break;
            case 4: _brainstormInput.Tags.Add(CurrentInput); break;
        }
    }

    private async Task UpdateExamples()
    {
        switch (CurrentPromptIndex)
        {
            case 0: Examples = _defaultExamples; break;
            case 1: Examples = await GetContextExamples(); break;
            case 2: Examples = await GetGoalsExamples(); break;
            case 3: Examples = await GetPreferencesExamples(); break;
            case 4: Examples = await GetTagsExamples(); break;
        }
    }

    private async Task<List<string>> GetContextExamples()
    {
        UriBuilder uri = new(_bs_contextUrl);
        var query = HttpUtility.ParseQueryString(uri.Query);
        query["topic"] = _brainstormInput.Topic;
        uri.Query = query.ToString();

        return await GetExamples(uri.ToString());
    }

    private async Task<List<string>> GetGoalsExamples()
    {
        UriBuilder uri = new(_bs_goalsUrl);
        var query = HttpUtility.ParseQueryString(uri.Query);
        query["topic"] = _brainstormInput.Topic;
        query["context"] = _brainstormInput.Context;
        uri.Query = query.ToString();

        return await GetExamples(uri.ToString());
    }

    private async Task<List<string>> GetPreferencesExamples()
    {
        UriBuilder uri = new(_bs_preferencesUrl);
        var query = HttpUtility.ParseQueryString(uri.Query);
        query["topic"] = _brainstormInput.Topic;
        query["context"] = _brainstormInput.Context;
        query["goals"] = string.Join(",", _brainstormInput.Goals);
        uri.Query = query.ToString();
        return await GetExamples(uri.ToString());
    }

    private async Task<List<string>> GetTagsExamples()
    {
        UriBuilder uriBuilder = new(_bs_tagsUrl);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["topic"] = _brainstormInput.Topic;
        query["context"] = _brainstormInput.Context;
        query["goals"] = string.Join(",", _brainstormInput.Goals);
        query["preferences"] = string.Join(",", _brainstormInput.Preferences);
        uriBuilder.Query = query.ToString();
        return await GetExamples(uriBuilder.ToString());
    }

    private static async Task<List<string>> GetExamples(string url)
    {
        using (var httpClient = new HttpClient())
        {
            try
            {
                var response = await httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    string errorJson = await response.Content.ReadAsStringAsync();
                    await Application.Current.MainPage.DisplayAlert("Error", errorJson, "OK");
                    return new List<string>();
                }
                string responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<BrainstormExamplesOuput>(responseJson).Examples;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
                return new List<string>();
            }
        }
    }
    public ICommand NextCommand => new AsyncRelayCommand(Next);
    public ICommand BackCommand => new AsyncRelayCommand(Back);

    public ICommand SkipCommand => new AsyncRelayCommand(Skip);

    private async Task Next()
    {
        Application.Current.MainPage.ShowPopup(spinner);
        if (CurrentPromptIndex == 4)
        {
            await GoToBrainstormChatPage();
            return;
        }
        SaveInput();
        CurrentPromptIndex++;
        await UpdateExamples();
        _getPrompt();
        if (CurrentPromptIndex == 4)
        {
            NextText = start;

        }
        CurrentInput = "";
        spinner.Close();
    }

    private async Task Back()
    {
        Application.Current.MainPage.ShowPopup(spinner);
        CurrentPromptIndex--;
        await UpdateExamples();
        _getPrompt();
        switch(CurrentPromptIndex)
        {
            case 0: CurrentInput = _brainstormInput.Topic; break;
            case 1: CurrentInput = _brainstormInput.Context; break;
            case 2: CurrentInput = string.Join(",", _brainstormInput.Goals); break;
            case 3: CurrentInput = string.Join(",", _brainstormInput.Preferences); break;
            case 4: CurrentInput = string.Join(",", _brainstormInput.Tags); break;
        }
        spinner.Close();
    }

    private async Task Skip()
    {
        SaveInput();
        await GoToBrainstormChatPage();
    }

    private async Task GoToBrainstormChatPage()
    {
        var navigationParams = new Dictionary<string, object>
        {
            ["Input"] = _brainstormInput
        };
        await Shell.Current.GoToAsync(nameof(BrainstormChatPage), navigationParams);
    }

}
