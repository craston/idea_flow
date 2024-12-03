

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

using frontend.Models;
using frontend.Views;
using System.Collections.ObjectModel;
namespace frontend.ViewModels;

public partial class BrainstormIdeaViewModel : ObservableObject, IQueryAttributable
{
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

    private void SendMessage()
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
    }
}

    
        
