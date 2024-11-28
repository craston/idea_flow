
using System.Web;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using frontend.Models;
using Newtonsoft.Json;
namespace frontend.ViewModels;
public partial class TestViewModel : ObservableObject
{
    public TestViewModel()
    {
    }

    public static string BaseAddress =
        DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:8000" : "http://localhost:8000";
    public static string TestUrl = $"{BaseAddress}/test/";

    private string questionInput="";
    // set default value for AnswerOutput
    private string answerOutput = "Answer will appear here";

    public string QuestionInput
    {
        get => questionInput;
        set => SetProperty(ref questionInput, value);

    }
    public string AnswerOutput
    {
        get => answerOutput;
        set => SetProperty(ref answerOutput, value);
    }

    public ICommand GetAnswer => new AsyncRelayCommand(Answer);

  
    private async Task Answer()
    {   
        if (QuestionInput == null) {
            // Display alert
            await Application.Current.MainPage.DisplayAlert("Error", "Please enter a question", "OK");
            return;
        }

        var uri = GetUri(QuestionInput);
        AnswerOutput = await GetResponse(uri);
    }

    private async Task<string> GetResponse(UriBuilder uri)
    {
        using (var httpClient = new HttpClient())
        {
            try
            {
                var response = await httpClient.GetAsync(uri.ToString());
                if (!response.IsSuccessStatusCode)
                {
                    string errorJson = await response.Content.ReadAsStringAsync();
                    await Application.Current.MainPage.DisplayAlert("Error", errorJson, "OK");
                    return "";
                }

                string responseJson = await response.Content.ReadAsStringAsync();
                // convert to Json object
                var responseObj = JsonConvert.DeserializeObject<TestOutput>(responseJson);
                return responseObj!.Answer;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
                return AnswerOutput;
            }
        }

    }
    private static UriBuilder GetUri(string question)
    {
        UriBuilder uri = new(TestUrl);
        var query = HttpUtility.ParseQueryString(uri.Query);
        query["question"] = question;
        uri.Query = query.ToString();
        return uri;
    }

}


