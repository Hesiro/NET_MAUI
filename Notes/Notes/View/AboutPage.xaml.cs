namespace Notes.View;

public partial class AboutPage : ContentPage
{
	public AboutPage()
	{
		InitializeComponent();
	}

    private async void LearnMore_Clicked(object sender, EventArgs e)
    {
        // Navigate to the specified URL in the system browser.
        if (BindingContext is Model.About about)  
            await Launcher.Default.OpenAsync(about.MoreInfoUrl);
    }
}