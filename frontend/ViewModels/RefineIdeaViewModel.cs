

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

using frontend.Models;
using frontend.Views;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text;
using CommunityToolkit.Maui.Views;
namespace frontend.ViewModels;

public partial class RefineIdeaViewModel : ObservableObject, IQueryAttributable
{
    public static string BaseAddress =
       DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:8000" : "http://localhost:8000";
    public static string RefineChatUrl = $"{BaseAddress}/refine_idea_chat/";

    private string _idea;

    public string Idea
    {
        get => _idea;
        set => SetProperty(ref _idea, value);
    }

    private RefineIdeaOutput _output;
    public RefineIdeaOutput Output
    {
        get => _output;
        set => SetProperty(ref _output, value);
    }

    private IdeaRefineChat chat;

    private ObservableCollection<ChatMessage> _messages;

    public ObservableCollection<ChatMessage> Messages
    {
        get => _messages;
        set => SetProperty(ref _messages, value);
    }

    private string _newMessage;
    public string NewMessage
    {
        get => _newMessage;
        set => SetProperty(ref _newMessage, value);
    }

    private string _sessionId;
    public RefineIdeaViewModel()
    {
        Messages = new ObservableCollection<ChatMessage>
        {
            new ChatMessage
            {
                Content = "Hello! How can I assist you today?",
                IsUserMessage = false,
                Timestamp = DateTime.Now
            }
        };

        chat = new IdeaRefineChat();
        chat.Messages = new List<ChatMessage>(Messages);
        _sessionId = Guid.NewGuid().ToString();
    }
    
    public ICommand SendMessageCommand => new AsyncRelayCommand(SendMessage);

    private void Back()
    {
        var navigationParams = new Dictionary<string, object>
        {
            ["Output"] = _output!
        };

        Shell.Current.GoToAsync(nameof(RefinePage), navigationParams);
    }
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var value = query["output"] as RefineIdeaOutput;  // Dealing with
                                                  // CS8601 - Possible null reference assignment
        Output = value!;

        var value2 = query["idea"] as string;
        Idea = value2!;
    }

    private async Task SendMessage()
    {
        if (string.IsNullOrWhiteSpace(NewMessage))
        {
            return;
        }
        var message = new ChatMessage
        {
            Content = NewMessage,
            IsUserMessage = true,
            Timestamp = DateTime.Now
        };
        Messages.Add(message);
        chat.Messages.Add(message);

        var spinner = new SpinnerPopup();
        Application.Current!.Windows![0].Page!.ShowPopup(spinner);
        var response = await GetResponse(GetUri());
        NewMessage = "";
        spinner.Close();
        await Application.Current!.Windows[0]!.Page!.DisplayAlert("Response", response, "OK");

        var responseMessage = new ChatMessage
        {
            Content = response,
            IsUserMessage = false,
            Timestamp = DateTime.Now
        };
        Messages.Add(responseMessage);
        chat.Messages.Add(responseMessage);

    }

    private async Task<string> GetResponse(UriBuilder uri)
    {
        using var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromMinutes(10);
        try
        {
            var response = await httpClient.GetAsync(uri.ToString());
            if (!response.IsSuccessStatusCode)
            {
                string errorJson = await response.Content.ReadAsStringAsync();
                await Application.Current!.Windows![0].Page!.DisplayAlert("Error", errorJson, "OK");
                return "";
            }
            string responseJson = await response.Content.ReadAsStringAsync();
            var responseObj = JsonSerializer.Deserialize<TestOutput>(responseJson);
            return responseObj!.Answer;
        }
        catch (Exception ex)
        {
            await Application.Current!.Windows![0].Page!.DisplayAlert("Error", ex.ToString(), "OK");
            return "";
        }
    }

    private UriBuilder GetUri()
    {
        UriBuilder uri = new(RefineChatUrl);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        query["idea"] = Idea;
        query["org_reply"] = JsonSerializer.Serialize(Output);
        query["question"] = NewMessage;
        query["session_id"] = _sessionId;
        uri.Query = query.ToString();
        return uri;
    }
}



