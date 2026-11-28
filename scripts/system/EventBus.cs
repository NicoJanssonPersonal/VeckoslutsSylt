using Godot;

public partial class EventBus : Node
{
    public static EventBus Instance { get; private set; }

    [Signal]
    public delegate void MoneyEarnedEventHandler(float moneyToAdd);

    [Signal]
    public delegate void WeedPickedEventHandler();

    [Signal]
    public delegate void ReOpenMainMenuEventHandler();

    [Signal]
    public delegate void currentMoneyEventHandler(float money);
    
    
    public override void _EnterTree()
    {
        Instance = this;
    }
}