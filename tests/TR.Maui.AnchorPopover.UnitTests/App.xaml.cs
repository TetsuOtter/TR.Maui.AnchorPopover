namespace TR.Maui.AnchorPopover.UnitTests;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new ContentPage
        {
            Content = new Label
            {
                Text = "Unit Tests - Run via dotnet test",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            }
        });
    }
}
