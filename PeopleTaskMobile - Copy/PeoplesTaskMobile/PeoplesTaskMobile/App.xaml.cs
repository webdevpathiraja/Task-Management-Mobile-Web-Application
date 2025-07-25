namespace PeoplesTaskMobile.PeoplesTaskMobile;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new NavigationPage(new NativePage());
    }
}
