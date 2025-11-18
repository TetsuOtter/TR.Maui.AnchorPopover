using Microsoft.Extensions.DependencyInjection;

namespace TR.Maui.AnchorPopover.UITests;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}