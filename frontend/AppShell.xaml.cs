using frontend.Views;
namespace frontend;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("RefinePage", typeof(RefinePage));
        Routing.RegisterRoute("RefineIdeaPage", typeof(RefineIdeaPage));
        Routing.RegisterRoute("BrainstormPage", typeof(BrainstormPage));
        Routing.RegisterRoute("RiddlePage", typeof(RiddlePage));
        Routing.RegisterRoute("TestPage", typeof(TestPage));
        Routing.RegisterRoute("BrainstormChatPage", typeof(BrainstormChatPage));
        Routing.RegisterRoute("BrainstormIdeaPage", typeof(BrainstormIdeaPage));
        Routing.RegisterRoute("SavedIdeasPage", typeof(SavedIdeasPage));
    }
}
