using Godot;
using System;

public partial class Settings : CanvasLayer
{
	[Export] private Button _applyButton;
	[Export] private LineEdit _Sensitivty;

	public const string SavePath = "user://settings.cfg";
	public static float MouseSensitivity { get; private set; } = 0.0001f;

	public override void _Ready()
	{
		if (_applyButton != null)
		{
			_applyButton.Pressed += ApplySettings;
		}

		LoadData();
		if (_Sensitivty != null)
		{
			_Sensitivty.Text = (MouseSensitivity * 1000).ToString();
		}
	}

	private void ApplySettings()
	{
		if (!float.TryParse(_Sensitivty.Text, out float parsedSens))
		{
			GD.PushWarning("Invalid sensitivity input, reverting to default.");
			parsedSens = 1f;
		}

		MouseSensitivity = parsedSens/1000;

		var config = new ConfigFile();
		config.SetValue("Settings", "Sens", MouseSensitivity);

		Error err = config.Save(SavePath);
		if (err != Error.Ok)
		{
			GD.PrintErr($"Failed to save config: {err}");
		}

		GetTree().Paused = false;
		EventBus.Instance.EmitSignal(EventBus.SignalName.ReOpenMainMenu);
		this.Visible = false;
	}

	public static void LoadData()
	{
		var config = new ConfigFile();
		Error err = config.Load(SavePath);
		if (err != Error.Ok)
		{
			MouseSensitivity = 0.05f;
			return;
		}

		MouseSensitivity = (float)config.GetValue("Settings", "Sens", 1/1000f);
	}
}