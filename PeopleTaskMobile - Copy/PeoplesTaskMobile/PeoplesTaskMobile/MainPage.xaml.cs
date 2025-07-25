namespace PeoplesTaskMobile.PeoplesTaskMobile;


public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void TaskWebView_Navigating(object sender, WebNavigatingEventArgs e)
    {
        LoadingIndicator.IsRunning = true;
        LoadingIndicator.IsVisible = true;
    }

    private void TaskWebView_Navigated(object sender, WebNavigatedEventArgs e)
    {
        LoadingIndicator.IsRunning = false;
        LoadingIndicator.IsVisible = false;

        if (e.Result != WebNavigationResult.Success)
        {
            DisplayAlert("Error", "Failed to load the page. Please check your internet connection.", "OK");
        }
    }

    protected override bool OnBackButtonPressed()
    {
        if (TaskWebView.CanGoBack)
        {
            TaskWebView.GoBack();
            return true;
        }
        return base.OnBackButtonPressed();
    }
}
