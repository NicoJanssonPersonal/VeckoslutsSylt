using Godot;
using System;

public partial class SuperWeedWhackerShop : Control
{
	[Export] private GridContainer _grid;
	[Export] private PackedScene _storeCardScene;
	public override void _Ready()
	{
		foreach (Node child in _grid.GetChildren())
		{
			child.QueueFree();
		}

		AddShopItem("Scissor", "Cuts basic weeds.", "20 Damage\n 0.75s action time", 10f, "res://assets/icons/handIconArtboard-1.png");
		AddShopItem("Scissor", "Cuts basic weeds.", "20 Damage\n 0.75s action time", 10f, "res://assets/icons/saxIconArtboard-1.png");
		AddShopItem("Scythe", "cuts multiple weeds nearby.", "20 Damage\n 2s action time", 25f, "res://assets/icons/scytheIconArtboard-1.png");
		AddShopItem("WeedWhacker", "Continously cuts weed", "20 Damage\n No action time", 25f, "res://assets/icons/trimmerIconArtboard-1.png");
		AddShopItem("Poison sprayar", "kills everything", "20 Damage\n 2s action time", 25f, "res://assets/icons/scytheIconArtboard-1.png");
	}

	private void AddShopItem(string title, string desc, string stats, float cost, string iconPath)
	{
		var card = _storeCardScene.Instantiate<StoreItemCard>();
		_grid.AddChild(card);
		card.SetupCard(title, desc, stats, iconPath, cost);
	}
}
