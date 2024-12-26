using frontend.Models;
namespace frontend;

public partial class App : Application
{
    static IdeaDatabase database;

    public static IdeaDatabase Database
    {
        get
        {
            database ??= new IdeaDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Ideas.db3"));
            return database;
        }
    }
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}
