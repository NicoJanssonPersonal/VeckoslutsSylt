using Godot;
using System;

public partial class UiManager : Control
{
	[Export] private CanvasLayer _settingsMenu;
	[Export] private CanvasLayer _pauseMenu;

	[Export] private Label _money;
	[Export] private Label _weeds;

	public override void _Ready()
	{
		_settingsMenu.Visible = false;
		_pauseMenu.Visible = false;
		GetTree().Paused = false;

		Button playButton = GetNode<Button>("pauseMenu/VBoxContainer/Play");
        Button quitButton = GetNode<Button>("pauseMenu/VBoxContainer/Quit");
        Button settingsButton = GetNode<Button>("pauseMenu/VBoxContainer/Settings");

		playButton.Pressed += OnPlayButtonPressed;
        quitButton.Pressed += OnQuitButtonPressed;
        settingsButton.Pressed += OnsettingsButtonPressed;

		EventBus.Instance.ReOpenMainMenu += closeSettings;
	}
	public override void _ExitTree()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.ReOpenMainMenu -= closeSettings;
        }
    }
	private void OnPlayButtonPressed()
	{
		_pauseMenu.Visible = false;
		GetTree().Paused = false;
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
	private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }
    private void OnsettingsButtonPressed()
    {
        bool isOpen = !_settingsMenu.Visible;
        _pauseMenu.Visible = !isOpen;
        _settingsMenu.Visible = isOpen;
    }
	public void closeSettings()
    {
        _pauseMenu.Visible = true;
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
		if (Input.IsActionJustPressed("openPauseMenu"))
		{
			bool isOpen = !_pauseMenu.Visible;
			_pauseMenu.Visible = isOpen;

			GetTree().Paused = isOpen;

			Input.MouseMode = isOpen ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured;
		}
	}
	public void displayMoney(string money)
	{
		_money.Text = "MONEY | " + money;
	}
	public void displayWeedsCollected(string weeds)
	{
		_weeds.Text = "WEEDS | " +weeds;
	}
}
