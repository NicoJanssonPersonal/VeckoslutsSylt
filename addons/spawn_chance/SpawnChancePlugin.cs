#if TOOLS
using Godot;

[Tool]
public partial class SpawnChancePlugin : EditorPlugin
{
    private SpawnChanceInspector _inspector;

    public override void _EnterTree()
    {
        _inspector = new SpawnChanceInspector();
        AddInspectorPlugin(_inspector);
    }

    public override void _ExitTree()
    {
        RemoveInspectorPlugin(_inspector);
    }
}
#endif
