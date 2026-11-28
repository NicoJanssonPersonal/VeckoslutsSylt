using Godot;

public partial class StoreItemCard : MarginContainer
{
	protected Texture2D itemImage;
	protected string itemTitle, itemDesc, itemStats;
	protected float itemCost;
	private bool _isBought = false;
	[Export] private Label itemTitleLabel, itemDescLabel, itemStatsLabel;
	[Export] private TextureRect itemImageRect;
	[Export] private Button _buyButton;
	[Export] private ColorRect _bought;

	public override void _Ready()
	{
		if (_buyButton != null)
		{
			_buyButton.Pressed += Buy;
		}
		EventBus.Instance.currentMoney += CanAfford;
	}
	public void SetupCard(string title, string desc, string stats, string imagePath, float cost)
	{
		itemTitle = title;
		itemDesc = desc;
		itemStats = stats;
		itemImage = GD.Load<Texture2D>(imagePath);
		itemCost = cost;

		PopulateCard();
	}
	public override void _ExitTree()
	{
		// Always unsubscribe from Autoload signals when the card is destroyed
		if (EventBus.Instance != null)
		{
			EventBus.Instance.currentMoney -= CanAfford;
		}
	}

	protected void PopulateCard()
	{
		itemTitleLabel.Text = itemTitle;
		itemDescLabel.Text = itemDesc;
		itemStatsLabel.Text = itemStats;
		_buyButton.Text = itemCost.ToString() + "coins";

		itemImageRect.Texture = itemImage;
	}
	private void Buy()
	{
		if (_isBought) return;

		_isBought = true;
		_bought.Visible = true;
		_buyButton.Disabled = true;

		// Emit a purchase signal on your EventBus so your currency manager subtracts `cost`
		// Example: EventBus.Instance.EmitSignal(EventBus.SignalName.ItemPurchased, cost);
	}

	private void CanAfford(float money)
	{
		if (_isBought)
		{
			_buyButton.Disabled = true;
			return;
		}

		_buyButton.Disabled = money < itemCost;
	}
}
