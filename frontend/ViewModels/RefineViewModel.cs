
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using frontend.Models;
using frontend.Views;
using Newtonsoft.Json;
using System.Web;
using System.Windows.Input;

namespace frontend.ViewModels;

public partial class RefineViewModel: ObservableObject
{
    private static readonly string BaseAddress =
    DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:8000" : "http://localhost:8000";
    private static readonly string RefineIdeaUrl = $"{BaseAddress}/refine_idea/";

    private string _idea;
    public string Idea
    {
        get => _idea;
        set => SetProperty(ref _idea, value);
    }

    public ICommand RefineIdeaCommand => new AsyncRelayCommand(RefineIdea);
    public RefineViewModel()
    { }

    private async Task RefineIdea()
    {
        if (Idea == null)
        {
            // Display alert
            await Application.Current!.Windows![0].Page!.DisplayAlert("Error", "Please enter an idea/draft", "OK");
            return;
        }

        var uri = GetUri(Idea);

        var spinner = new SpinnerPopup();
        Application.Current!.Windows![0].Page!.ShowPopup(spinner);
        var output = await GetResponse(uri);
        spinner.Close();

        if (output == null)
        {
            return;
        }

        var navigationParams = new Dictionary<string, object>
        {
            ["output"] = output,
            ["idea"] = Idea
        };

        await Shell.Current.GoToAsync(nameof(RefineIdeaPage), navigationParams);
    }
    private static async Task<RefineIdeaOutput?> GetResponse(UriBuilder uri)
    {
        using (var httpClient = new HttpClient())
        {
            try
            {
                var response = await httpClient.GetAsync(uri.ToString());
                if (!response.IsSuccessStatusCode)
                {
                    string errorJson = await response.Content.ReadAsStringAsync();
                    await Application.Current!.Windows![0].Page!.DisplayAlert("Error", errorJson, "OK");
                    return null;
                }

                string responseJson = await response.Content.ReadAsStringAsync();
                // convert to Json object
                var responseObj = JsonConvert.DeserializeObject<RefineIdeaOutput>(responseJson);
                return responseObj;
            }
            catch (Exception ex)
            {
                await Application.Current!.Windows![0].Page!.DisplayAlert("Error", ex.ToString(), "OK");
                return null;
            }
        }
    }
    private static UriBuilder GetUri(string question)
    {
        UriBuilder uri = new(RefineIdeaUrl);
        var query = HttpUtility.ParseQueryString(uri.Query);
        query["idea"] = question;
        uri.Query = query.ToString();
        return uri;
    }
}
