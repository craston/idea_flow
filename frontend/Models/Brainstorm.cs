using CommunityToolkit.Mvvm.ComponentModel;
namespace frontend.Models;

using SQLite;
using System.Text.Json;

public class BrainstormInput
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string? Topic { get; set; }
    public string? Context { get; set; }

    // JSON strings to store serialized lists
    public string GoalsSerialized { get; set; } = "[]";
    public string PreferencesSerialized { get; set; } = "[]";
    public string TagsSerialized { get; set; } = "[]";
    public int Idea_count { get; set; } = 2;
    public string IdeaListSerialized { get; set; } = "[]";

    // Non-mapped properties for easy access to lists
    [Ignore]
    public List<string> Goals
    {
        get => JsonSerializer.Deserialize<List<string>>(GoalsSerialized) ?? [];
        set => GoalsSerialized = JsonSerializer.Serialize(value);
    }

    [Ignore]
    public List<string> Preferences
    {
        get => JsonSerializer.Deserialize<List<string>>(PreferencesSerialized) ?? [];
        set => PreferencesSerialized = JsonSerializer.Serialize(value);
    }

    [Ignore]
    public List<string> Tags
    {
        get => JsonSerializer.Deserialize<List<string>>(TagsSerialized) ?? [];
        set => TagsSerialized = JsonSerializer.Serialize(value);
    }

    [Ignore]
    public List<int> IdeaList
    {
        get => JsonSerializer.Deserialize<List<int>>(IdeaListSerialized) ?? [];
        set => IdeaListSerialized = JsonSerializer.Serialize(value);
    }
}


public partial class IdeaDetail : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }

    // JSON strings to store serialized lists

    public string HighlightsSerialized { get; set; } = "[]";
    public string ActivitiesSerialized { get; set; } = "[]";
    public string TipsSerialized { get; set; } = "[]";

    [Ignore]
    public List<string> Highlights
    {
        get => JsonSerializer.Deserialize<List<string>>(HighlightsSerialized) ?? [];
        set => HighlightsSerialized = JsonSerializer.Serialize(value);
    }
    [Ignore]
    public List<string> Activities
    {
        get => JsonSerializer.Deserialize<List<string>>(ActivitiesSerialized) ?? [];
        set => ActivitiesSerialized = JsonSerializer.Serialize(value);
    }
    [Ignore]
    public List<string> Tips
    {
        get => JsonSerializer.Deserialize<List<string>>(TipsSerialized) ?? [];
        set => TipsSerialized = JsonSerializer.Serialize(value);
    }

    public bool IsSaved { get; set; } = false;
    private string _imgSource = "heart.png";
    public string ImgSource
    {
        get => _imgSource;
        set => SetProperty(ref _imgSource, value);
    }

    //Foreign keys
    public int BrainstormInputId { get; set; }
}
public class BrainstormingOutput
{
    public string? Topic { get; set; }
    public List<IdeaDetail> Generated_ideas { get; set; } = [];
}
public class BrainstormExamplesOuput
{
    public List<string> Examples { get; set; } = [];
}

public class ChatMessage
{
    public string? Content { get; set; } // The text of the message
    public bool IsUserMessage { get; set; } // True if the message is from the user
    public DateTime Timestamp { get; set; } // When the message was sent or received
}

public class IdeaRefineChat
{
    public List<ChatMessage> Messages { get; set; } = [];

}
