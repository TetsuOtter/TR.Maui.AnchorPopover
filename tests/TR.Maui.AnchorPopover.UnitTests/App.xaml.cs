namespace TR.Maui.AnchorPopover.UnitTests;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // For unit tests, we don't need a UI
        MainPage = new ContentPage
        {
            Content = new Label
            {
                Text = "Unit Tests - Run via dotnet test",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            }
        };
    }
}
