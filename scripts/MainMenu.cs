using Godot;

public partial class MainMenu : Control
{
    // Export allows you to pick your gameplay scene directly in the Inspector
    [Export]
    public PackedScene GameScene { get; set; }
    [Export] private CanvasLayer _settingsMenu;

    [Export] private CanvasLayer buttonCont;

    public override void _Ready()
    {
        // Ensure the mouse cursor is visible (important if returning from a 3D FPS game)
        Input.MouseMode = Input.MouseModeEnum.Visible;

        Button playButton = GetNode<Button>("pauseMenu/VBoxContainer/Play");
        Button quitButton = GetNode<Button>("pauseMenu/VBoxContainer/Quit");
        Button settingsButton = GetNode<Button>("pauseMenu/VBoxContainer/Settings");

        playButton.Pressed += OnPlayButtonPressed;
        quitButton.Pressed += OnQuitButtonPressed;
        settingsButton.Pressed += OnsettingsButtonPressed;

        EventBus.Instance.ReOpenMainMenu += ReOpenMainMenu;
    }
    public override void _ExitTree()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.ReOpenMainMenu -= ReOpenMainMenu;
        }
    }

    private void OnPlayButtonPressed()
    {
        if (GameScene != null)
        {
            GetTree().ChangeSceneToPacked(GameScene);
        }
        else
        {
            GD.PrintErr("GameScene is not assigned in the Inspector!");
        }
    }

    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }
    private void OnsettingsButtonPressed()
    {
        bool isOpen = !_settingsMenu.Visible;
        buttonCont.Visible = !isOpen;
        _settingsMenu.Visible = isOpen;
    }
    public void ReOpenMainMenu()
    {
        buttonCont.Visible = true;
    }
}
