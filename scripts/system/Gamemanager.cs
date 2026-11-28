using Godot;
using System;

public partial class Gamemanager : Node
{
	public const string SavePath = "user://settings.cfg";
	[Export] private UiManager uiManager;
	private float _currentMoney;
	private int _weeds;
	private bool _isDirty = false;

	public override void _Ready()
	{
		LoadGameData();
		// Subscribe to the global EventBus
		EventBus.Instance.MoneyEarned += UpdateMoney;
		EventBus.Instance.WeedPicked += UpdateWeedsPicked;

		// Update UI with loaded save data immediately on start
		uiManager.displayMoney(_currentMoney.ToString("F2"));
		uiManager.displayWeedsCollected(_weeds.ToString());
	}
	public void UpdateMoney(float moneyToAdd)
	{
		_currentMoney += moneyToAdd;
		uiManager.displayMoney(_currentMoney.ToString("F2"));
		_isDirty = true;
	}

	public void UpdateWeedsPicked()
	{
		_weeds++;
		uiManager.displayWeedsCollected(_weeds.ToString());
		_isDirty = true;
	}

	public override void _Notification(int what)
	{
		if (what == NotificationWMCloseRequest)
		{
			SaveGameData();
		}
	}

	// Call this manually at checkpoints, level ends, or from a Timer node
	public void SaveGameData()
	{
		if (!_isDirty) return; // Skip disk write if nothing changed

		var config = new ConfigFile();
		if (FileAccess.FileExists(SavePath))
		{
			config.Load(SavePath);
		}

		config.SetValue("GameData", "money", _currentMoney);
		config.SetValue("GameData", "weeds", _weeds);

		Error err = config.Save(SavePath);
		if (err != Error.Ok)
		{
			GD.PrintErr($"Failed to save config: {err}");
			return;
		}

		_isDirty = false;
	}

	private void LoadGameData()
	{
		if (!FileAccess.FileExists(SavePath))
		{
			_currentMoney = 0f;
			_weeds = 0;
			return;
		}

		var config = new ConfigFile();
		Error err = config.Load(SavePath);
		if (err != Error.Ok)
		{
			GD.PrintErr($"Failed to load save file: {err}");
			return;
		}

		_currentMoney = config.GetValue("GameData", "money", 0f).AsSingle();
		_weeds = config.GetValue("GameData", "weeds", 0).AsInt32();
	}
	public override void _ExitTree()
	{
		if (EventBus.Instance != null)
		{
			EventBus.Instance.MoneyEarned -= UpdateMoney;
			EventBus.Instance.WeedPicked -= UpdateWeedsPicked;
		}
	}
}