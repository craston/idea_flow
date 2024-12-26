using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json;
using System.Web;

using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

using frontend.Models;
using frontend.Views;
using CommunityToolkit.Maui.Views;
namespace frontend.ViewModels;
public partial class BrainstormChatViewModel : ObservableObject, IQueryAttributable
{
    public BrainstormChatViewModel()
    {

    }

    SpinnerPopup spinner = new();

    private readonly static string _baseAddress =
        DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:8000" : "http://localhost:8000";
    private readonly static string _bsUrl = $"{_baseAddress}/brainstorm/";

    private List<IdeaDetail> _ideas = [];

    private string _topic = "";
    public List<IdeaDetail> Ideas
    {
        get => _ideas;
        set => SetProperty(ref _ideas, value);
    }
    public string Topic
    {
        get => _topic;
        set => SetProperty(ref _topic, value);
    }

    private BrainstormingOutput Output = new();

    private BrainstormInput input = new();
    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var Input = query.TryGetValue("Input", out var input) ? (BrainstormInput)input! : new BrainstormInput();
        Application.Current!.Windows![0].Page!.ShowPopup(spinner);

        Output = query.TryGetValue("Output", out var output) ? (BrainstormingOutput)output! : await GetBrainStormOutput(Input!);

        spinner.Close();
        Ideas = Output.Generated_ideas;
        Topic = "Generated Ideas for " + Output!.Topic;
    }

    public ICommand IdeaClickedCommand => new AsyncRelayCommand<IdeaDetail>(IdeaClicked!);

    public ICommand SaveCommand => new RelayCommand<IdeaDetail>(Save);

    private async void Save(IdeaDetail? idea)
    {
        if (idea == null) return;
        idea.IsSaved = !idea.IsSaved;
        idea.ImgSource = idea.IsSaved ? "heart_filled.png" : "heart.png";

        if (idea.IsSaved)
        {
            input.IdeaList.Add(idea.Id);
            idea.BrainstormInputId = input.Id;
            await App.Database.SaveBrainstormInputAsync(input);
            await App.Database.SaveIdeaDetailAsync(idea);
        }
        else
        {
            input.IdeaList.Remove(idea.Id);
            if (input.IdeaList.Count() == 0)
            {
                await App.Database.DeleteBrainstormInputAsync(input);
            }
            else
            {
                await App.Database.SaveBrainstormInputAsync(input);
            }
            await App.Database.DeleteIdeaDetailAsync(idea);
        }
    }
    private async Task IdeaClicked(IdeaDetail idea)
    {
        var navigationParams = new Dictionary<string, object>
        {
            ["Idea"] = idea,
            ["Brainstorm_output"] = Output
        };

        await Shell.Current.GoToAsync(nameof(BrainstormIdeaPage), navigationParams);

    }
    private async Task<BrainstormingOutput> GetBrainStormOutput(BrainstormInput Input)
    {
        input = Input;
        UriBuilder uri = new(_bsUrl);
        var query = HttpUtility.ParseQueryString(uri.Query);
        query["topic"] = Input.Topic;
        query["context"] = Input.Context;
        query["goals"] = string.Join(",", Input.Goals);
        query["preferences"] = string.Join(",", Input.Preferences);
        query["tags"] = string.Join(",", Input.Tags);
        query["idea_count"] = Input.Idea_count.ToString();
        uri.Query = query.ToString();

        return await StartBrainstorming(uri.ToString());
    }

    private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true // Allow case-insensitive property matching
    };

    private async Task<BrainstormingOutput?> StartBrainstorming(string url)
    {
        using (var httpClient = new HttpClient())
        {
            httpClient.Timeout = TimeSpan.FromMinutes(10);
            try
            {
                var response = await httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    string errorJson = await response.Content.ReadAsStringAsync();
                    await Application.Current!.Windows![0].Page!.DisplayAlert("Error", errorJson, "OK");
                    return new BrainstormingOutput();
                }
                string responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BrainstormingOutput>(responseJson, _jsonOptions);
            }
            catch (Exception ex)
            {
                await Application.Current!.Windows![0].Page!.DisplayAlert("Error", ex.ToString(), "OK");
                return new BrainstormingOutput();
            }
        }
    }
}
