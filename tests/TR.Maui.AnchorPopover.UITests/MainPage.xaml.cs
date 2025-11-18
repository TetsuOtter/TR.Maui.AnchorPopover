namespace TR.Maui.AnchorPopover.UITests;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnBasicPopoverClicked(object? sender, EventArgs e)
	{
		try
		{
			StatusLabel.Text = "Test 1: Showing basic popover...";
			
			var popover = AnchorPopover.Create();
			
			var content = new Label
			{
				Text = "✓ Basic popover test\n\nThis is a simple text popover anchored to the button above.",
				Padding = 20,
				FontSize = 16,
				LineBreakMode = LineBreakMode.WordWrap
			};

			await popover.ShowAsync(content, BasicPopoverBtn);
			
			StatusLabel.Text = "Test 1: ✓ Passed - Basic popover displayed";
		}
		catch (Exception ex)
		{
			StatusLabel.Text = $"Test 1: ✗ Failed - {ex.Message}";
		}
	}

	private async void OnCustomContentClicked(object? sender, EventArgs e)
	{
		try
		{
			StatusLabel.Text = "Test 2: Showing custom content...";
			
			var popover = AnchorPopover.Create();
			
			var content = new VerticalStackLayout
			{
				Padding = 20,
				Spacing = 10,
				Children =
				{
					new Label { Text = "✓ Custom Content Test", FontSize = 18, FontAttributes = FontAttributes.Bold },
					new Label { Text = "This popover contains multiple controls:", FontSize = 14 },
					new Entry { Placeholder = "Test input", WidthRequest = 250 },
					new Label { Text = "• Label ✓\n• Entry ✓\n• Button ✓", FontSize = 12 },
					new Button 
					{ 
						Text = "Close", 
						Command = new Command(() => popover.Dismiss())
					}
				}
			};

			var options = new PopoverOptions
			{
				PreferredWidth = 300,
				ArrowDirection = PopoverArrowDirection.Down
			};

			await popover.ShowAsync(content, CustomContentBtn, options);
			
			StatusLabel.Text = "Test 2: ✓ Passed - Custom content displayed";
		}
		catch (Exception ex)
		{
			StatusLabel.Text = $"Test 2: ✗ Failed - {ex.Message}";
		}
	}

	private async void OnArrowDirectionClicked(object? sender, EventArgs e)
	{
		try
		{
			StatusLabel.Text = "Test 3: Showing arrow direction test...";
			
			var popover = AnchorPopover.Create();
			
			var content = new Label
			{
				Text = "✓ Arrow Direction Test\n\nThis popover prefers to show above with an upward arrow.",
				Padding = 20,
				FontSize = 14,
				WidthRequest = 250,
				LineBreakMode = LineBreakMode.WordWrap
			};

			var options = new PopoverOptions
			{
				ArrowDirection = PopoverArrowDirection.Up,
				BackgroundColor = Colors.LightBlue
			};

			await popover.ShowAsync(content, ArrowDirectionBtn, options);
			
			StatusLabel.Text = "Test 3: ✓ Passed - Arrow direction configured";
		}
		catch (Exception ex)
		{
			StatusLabel.Text = $"Test 3: ✗ Failed - {ex.Message}";
		}
	}

	private async void OnPositionedPopoverClicked(object? sender, EventArgs e)
	{
		try
		{
			StatusLabel.Text = "Test 4: Showing positioned popover...";
			
			var popover = AnchorPopover.Create();
			
			var content = new Label
			{
				Text = "✓ Positioned Popover Test\n\nThis popover is shown at screen coordinates.",
				Padding = 20,
				FontSize = 14,
				LineBreakMode = LineBreakMode.WordWrap
			};

			var bounds = new Rect(
				x: Width / 2 - 25,
				y: Height / 2 - 25,
				width: 50,
				height: 50
			);

			await popover.ShowAsync(content, bounds);
			
			StatusLabel.Text = "Test 4: ✓ Passed - Positioned popover shown";
		}
		catch (Exception ex)
		{
			StatusLabel.Text = $"Test 4: ✗ Failed - {ex.Message}";
		}
	}

	private async void OnInteractiveClicked(object? sender, EventArgs e)
	{
		try
		{
			StatusLabel.Text = "Test 5: Showing interactive content...";
			
			var popover = AnchorPopover.Create();
			
			var testLabel = new Label 
			{ 
				Text = "Interaction count: 0",
				FontSize = 14,
				HorizontalOptions = LayoutOptions.Center
			};

			int interactionCount = 0;

			var content = new VerticalStackLayout
			{
				Padding = 20,
				Spacing = 10,
				Children =
				{
					new Label { Text = "✓ Interactive Test", FontSize = 18, FontAttributes = FontAttributes.Bold },
					testLabel,
					new Button 
					{ 
						Text = "Interact", 
						Command = new Command(() => 
						{
							interactionCount++;
							testLabel.Text = $"Interaction count: {interactionCount}";
						})
					},
					new Button 
					{ 
						Text = "Close & Verify", 
						Command = new Command(() => 
						{
							if (interactionCount > 0)
								StatusLabel.Text = $"Test 5: ✓ Passed - {interactionCount} interactions";
							else
								StatusLabel.Text = "Test 5: ✗ Failed - No interactions";
							popover.Dismiss();
						})
					}
				}
			};

			await popover.ShowAsync(content, InteractiveBtn);
		}
		catch (Exception ex)
		{
			StatusLabel.Text = $"Test 5: ✗ Failed - {ex.Message}";
		}
	}
}
