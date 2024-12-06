﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Newtonsoft.Json;
using CommunityToolkit.Maui.Views;

using frontend.Models;
using frontend.Views;

namespace frontend.ViewModels;

public partial class RiddleViewModel: ObservableObject, IQueryAttributable
{
    private string ?_riddle_question;
    private string ?_riddle_answer;
    private string ?_userAnswer;
    private string _feedback;
    private bool _isFeedbackVisible;

    public static string BaseAddress =
        DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:8000" : "http://localhost:8000";
    public static string RiddleUrl = $"{BaseAddress}/gen_riddle/";
    public static string RightAnswerUrl = $"{BaseAddress}/check_answer_riddle/";
    public static string RevealAnswerUrl = $"{BaseAddress}/reveal_answer_riddle/";
    public string Riddle_question
    {
        get => _riddle_question!;
        set => SetProperty(ref _riddle_question, value);
    }
   
    public string UserAnswer
    {
        get => _userAnswer!;
        set => SetProperty(ref _userAnswer, value);
    }

    public string Feedback
    {
        get => _feedback;
        set => SetProperty(ref _feedback, value);
    }

    public bool IsFeedbackVisible
    {
        get => _isFeedbackVisible;
        set => SetProperty(ref _isFeedbackVisible, value);
    }

    public ICommand SubmitAnswerCommand => new AsyncRelayCommand(SubmitAnswer);
    public ICommand RevealAnswerCommand => new AsyncRelayCommand(RevealAnswer);

    public RiddleViewModel()
    {
    }

    private async Task LoadRiddle()
    {
        var uri = new Uri(RiddleUrl);
        using (var httpClient = new HttpClient())
        {
            try
            {
                var response = await httpClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var output = JsonConvert.DeserializeObject<Riddle>(content)!;
                Riddle_question = output.Riddle_question!;
                _riddle_answer = output.Riddle_answer;
            }
            catch (Exception e)
            {
                await Application.Current!.Windows![0].Page!.DisplayAlert("Error", e.Message, "OK");
            }
        }
    }   

    private async Task SubmitAnswer()
    {
        
    }

    private async Task RevealAnswer()
    {
        Feedback = _riddle_answer!;
        IsFeedbackVisible = true;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var spinner = new SpinnerPopup();
        Application.Current!.Windows![0].Page!.ShowPopup(spinner);
        await LoadRiddle();
        spinner.Close();
    }
}

