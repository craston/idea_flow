using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Web;
using System.Collections.ObjectModel;
using frontend.Models;
using Newtonsoft.Json;
using frontend.Views;

namespace frontend.ViewModels;

public partial class BrainstormViewModel : ObservableObject
{
    public BrainstormViewModel()
    {
    }

    private UriBuilder _GetUri()
    {
        UriBuilder uri = new("http://127.0.0.1:8000/trending");
        var query = HttpUtility.ParseQueryString(uri.Query);
        uri.Query = query.ToString();
        return uri;
    }
}