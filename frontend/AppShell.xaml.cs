using frontend.Views;
namespace frontend
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("ConversationPage", typeof(ConversationPage));
            Routing.RegisterRoute("BrainstormPage", typeof(BrainstormPage));
            Routing.RegisterRoute("PuzzlesPage", typeof(PuzzlesPage));
            Routing.RegisterRoute("TestPage", typeof(TestPage));
            Routing.RegisterRoute("BrainstormChatPage", typeof(BrainstormChatPage));
            Routing.RegisterRoute("BrainstormIdeaPage", typeof(BrainstormIdeaPage));
        }
    }
}
