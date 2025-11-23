namespace TR.Maui.AnchorPopover.Sample;

public partial class MainPage : ContentPage
{
	private Guid? _currentPopoverId;

	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnSimplePopoverClicked(object? sender, EventArgs e)
	{
		try
		{
			StatusLabel.Text = "Showing simple popover...";

			var popover = AnchorPopover.Create();
			var popoverId = Guid.NewGuid();
			_currentPopoverId = popoverId;

			// Create simple label content
			var content = new Label
			{
				Text = "This is a simple popover!",
				Padding = 20,
				FontSize = 16
			};

			await popover.ShowAsync(content, SimplePopoverBtn);

			if (_currentPopoverId == popoverId)
			{
				StatusLabel.Text = "Popover dismissed";
			}
		}
		catch (Exception ex)
		{
			StatusLabel.Text = $"Error: {ex.Message}";
		}
	}
	private async void OnCustomContentClicked(object? sender, EventArgs e)
	{
		try
		{
			StatusLabel.Text = "Showing custom content popover...";

			var popover = AnchorPopover.Create();
			var popoverId = Guid.NewGuid();
			_currentPopoverId = popoverId;

			// Create custom content with multiple elements
			var content = new VerticalStackLayout
			{
				Padding = 20,
				Spacing = 10,
				Children =
				{
					new Label { Text = "Custom Popover", FontSize = 18, FontAttributes = FontAttributes.Bold },
					new Label { Text = "This popover contains multiple MAUI controls", FontSize = 14 },
					new Entry { Placeholder = "Type something...", WidthRequest = 250 },
					new Button
					{
						Text = "Close",
						Command = new Command(async () => await popover.DismissAsync())
					}
				}
			};

			var options = new PopoverOptions
			{
				PreferredWidth = 300,
				ArrowDirection = PopoverArrowDirection.Down
			};

			await popover.ShowAsync(content, CustomContentBtn, options);

			if (_currentPopoverId == popoverId)
			{
				StatusLabel.Text = "Custom popover dismissed";
			}
		}
		catch (Exception ex)
		{
			StatusLabel.Text = $"Error: {ex.Message}";
		}
	}

	private async void OnPositionedPopoverClicked(object? sender, EventArgs e)
	{
		try
		{
			StatusLabel.Text = "Showing positioned popover...";

			var popover = AnchorPopover.Create();
			var popoverId = Guid.NewGuid();
			_currentPopoverId = popoverId;

			var content = new Label
			{
				Text = "This popover is positioned at a specific location",
				Padding = 20,
				FontSize = 14
			};

			// Position at center of screen
			var bounds = new Rect(
				x: Width / 2,
				y: Height / 2,
				width: 50,
				height: 50
			);

			await popover.ShowAsync(content, bounds);

			if (_currentPopoverId == popoverId)
			{
				StatusLabel.Text = "Positioned popover dismissed";
			}
		}
		catch (Exception ex)
		{
			StatusLabel.Text = $"Error: {ex.Message}";
		}
	}

	private async void OnArrowDirectionClicked(object? sender, EventArgs e)
	{
		try
		{
			StatusLabel.Text = "Showing popover with arrow direction...";

			var popover = AnchorPopover.Create();
			var popoverId = Guid.NewGuid();
			_currentPopoverId = popoverId;

			var content = new Label
			{
				Text = "This popover prefers to show above the button with an upward arrow",
				Padding = 20,
				FontSize = 14,
				WidthRequest = 250
			};

			var options = new PopoverOptions
			{
				ArrowDirection = PopoverArrowDirection.Up,
				BackgroundColor = Colors.LightBlue
			};

			await popover.ShowAsync(content, ArrowDirectionBtn, options);

			if (_currentPopoverId == popoverId)
			{
				StatusLabel.Text = "Arrow direction popover dismissed";
			}
		}
		catch (Exception ex)
		{
			StatusLabel.Text = $"Error: {ex.Message}";
		}
	}
}
