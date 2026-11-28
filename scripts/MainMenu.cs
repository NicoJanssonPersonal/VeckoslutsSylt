using Godot;

public partial class MainMenu : Control
{
    // Export allows you to pick your gameplay scene directly in the Inspector
    [Export]
    public PackedScene GameScene { get; set; }

    public override void _Ready()
    {
        // Ensure the mouse cursor is visible (important if returning from a 3D FPS game)
        Input.MouseMode = Input.MouseModeEnum.Visible;

        Button playButton = GetNode<Button>("VBoxContainer/Play");
        Button quitButton = GetNode<Button>("VBoxContainer/Quit");

        playButton.Pressed += OnPlayButtonPressed;
        quitButton.Pressed += OnQuitButtonPressed;
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
}