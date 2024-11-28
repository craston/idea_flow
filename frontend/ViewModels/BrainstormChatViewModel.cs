﻿using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System.Web;

using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

using frontend.Models;
using frontend.Views;
namespace frontend.ViewModels;
public partial class BrainstormChatViewModel : ObservableObject, IQueryAttributable
{
    public BrainstormChatViewModel()
    {

    }

    private readonly static string _baseAddress =
        DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:8000" : "http://localhost:8000";
    private readonly static string _bsUrl = $"{_baseAddress}/brainstorm/";

    private List<IdeaDetail> _ideas = [];

    public List<IdeaDetail> Ideas
    {
        get => _ideas;
        set => SetProperty(ref _ideas, value);
    }
    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var Input = query["Input"] as BrainstormInput;
        var Output = await GetBrainStormOutput(Input);

        Ideas = Output.Generated_ideas;

    }

    public ICommand IdeaClickedCommand => new AsyncRelayCommand<IdeaDetail>(IdeaClicked);


    private async Task IdeaClicked(IdeaDetail idea)
    {
        var navigationParams = new Dictionary<string, object>
        {
            ["Idea"]= idea 
        };

        await Shell.Current.GoToAsync(nameof(BrainstormIdeaPage), navigationParams);
        
    }
    private async Task<BrainstormingOutput> GetBrainStormOutput(BrainstormInput Input)
    {
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

    private async Task<BrainstormingOutput> StartBrainstorming(string url)
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
                    return new BrainstormingOutput();
                }
                string responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<BrainstormingOutput>(responseJson);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
                return new BrainstormingOutput();
            }
        }
    }
}
