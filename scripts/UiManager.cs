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
	public void displayMoney(string money)
	{
		_money.Text = money;
	}
	public void displayWeedsCollected(string weeds)
	{
		_weeds.Text = weeds;
	}
}
