
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using frontend.Models;
using System.Web;
using Newtonsoft.Json;

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
    private readonly static string _bsUrl = $"{_baseAddress}/brainstorm/";
    private readonly static string _bs_contextUrl = $"{_baseAddress}/brainstorm_context";
    private readonly static string _bs_goalsUrl = $"{_baseAddress}/brainstorm_goals";
    private readonly static string _bs_preferencesUrl = $"{_baseAddress}/brainstorm_preferences";

    private List<string> _defaultExamples = ["Vacation Ideas", "Team Building", "Product Ideas"];
    private string _currentPrompt = "";
    private string _currentInput = "";
    private int _currentPromptIndex = 0;
    private List<string> _examples = new List<string>();

    private BrainstormInput _brainstormInput = new BrainstormInput();
    public string CurrentPrompt
    {
        get => _currentPrompt;
        set => SetProperty(ref _currentPrompt, value);
    }

    public string CurrentInput
    {
        get => _currentInput;
        set => SetProperty(ref _currentInput, value);
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

    private void _getPrompt()
    {
        switch (CurrentPromptIndex)
        {
            case 0: CurrentPrompt = "What's the topic?"; break;
            case 1: CurrentPrompt = "Do you want to provide a context?"; break;
            case 2: CurrentPrompt = "Your goals?"; break;
            case 3: CurrentPrompt = "Any preferences?"; break;
            case 4: CurrentPrompt = "Add tags?"; break;
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
            case 4: Examples = GetTagsExamples(); break;
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

    private List<string> GetTagsExamples()
    {
        return new List<string> { "Technology", "Health", "Education" };
    }

    private async Task<List<string>> GetExamples(string url)
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
    public ICommand NextCommand => new RelayCommand(Next);
    public ICommand BackCommand => new RelayCommand(Back);

    private void Next()
    {
        SaveInput();
        CurrentPromptIndex++;
        UpdateExamples();
        _getPrompt();
    }

    private void Back()
    {
        CurrentPromptIndex--;
        UpdateExamples();
        _getPrompt();
    }




}
