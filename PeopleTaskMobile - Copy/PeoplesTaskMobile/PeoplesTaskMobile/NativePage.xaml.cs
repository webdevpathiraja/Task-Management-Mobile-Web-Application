using Microsoft.Maui.Controls;

namespace PeoplesTaskMobile.PeoplesTaskMobile;

public partial class NativePage : ContentPage
{
    public NativePage()
    {
        InitializeComponent();
    }

    private async void OnButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }
}
