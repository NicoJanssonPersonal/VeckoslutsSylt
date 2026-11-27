using Godot;
using System;

public partial class UiManager : Control
{
	[Export] private CanvasLayer _settingsMenu;
	[Export] private Label _money;
	[Export] private Label _weeds;

	public override void _Ready()
	{
		_settingsMenu.Visible = false;
		GetTree().Paused = false;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("openSettings"))
		{
			bool isOpen = !_settingsMenu.Visible;
			_settingsMenu.Visible = isOpen;

			GetTree().Paused = isOpen;

			Input.MouseMode = isOpen ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured;
		}
	}
	private void displayMoney()
	{
		//_money.Text = 
	}
	private void displayWeedsCollected()
	{
		//_weeds.Text =
	}
}
