

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

public partial class BrainstormIdeaViewModel : ObservableObject, IQueryAttributable
{
    public static string BaseAddress =
       DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:8000" : "http://localhost:8000";
    public static string IdeaChatUrl = $"{BaseAddress}/idea_chat/";

    private IdeaDetail? _idea;
    public IdeaDetail Idea
    {
        get => _idea!;
        set => SetProperty(ref _idea, value);
    }

    private string _topic;

    public string Topic
    {
        get => _topic;
        set => SetProperty(ref _topic, value);
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

    private BrainstormingOutput? _output;
    public BrainstormIdeaViewModel()
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
    }

    public ICommand BackCommand => new RelayCommand(Back);

    public ICommand SendMessageCommand => new RelayCommand(SendMessage);

    private void Back()
    {
        var navigationParams = new Dictionary<string, object>
        {
            ["Output"] = _output!
        };

        Shell.Current.GoToAsync(nameof(BrainstormChatPage), navigationParams);
    }
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var value = query["Idea"] as IdeaDetail;  // Dealing with
                                                  // CS8601 - Possible null reference assignment
        Idea = value!;

        _output = query["Brainstorm_output"] as BrainstormingOutput;
        Topic = "Generated Ideas for " + _output!.Topic;

    }

    private async void SendMessage()
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
        NewMessage = "";

        var spinner = new SpinnerPopup();
        Application.Current!.Windows![0].Page!.ShowPopup(spinner);
        var response = await GetResponse(message.Content);
        spinner.Close();
        await Application.Current!.MainPage!.DisplayAlert("Response", response, "OK");

        var responseMessage = new ChatMessage
        {
            Content = response,
            IsUserMessage = false,
            Timestamp = DateTime.Now
        };
        Messages.Add(responseMessage);
        chat.Messages.Add(responseMessage);


    }

    private async Task<string> GetResponse(string message)
    {
        var uri = GetUri(message);
        using var httpClient = new HttpClient();
        try
        {
            var ideaJson = JsonSerializer.Serialize(Idea);
            await Application.Current!.Windows![0].Page!.DisplayAlert("Request", ideaJson, "OK");
            var content = new StringContent(ideaJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(uri.ToString(), content);
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

    private UriBuilder GetUri(string message)
    {
        UriBuilder uri = new(IdeaChatUrl);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        query["brainstorm_topic"] = Topic;
        query["question"] = message;
        uri.Query = query.ToString();
        return uri;
    }
}

    
        
