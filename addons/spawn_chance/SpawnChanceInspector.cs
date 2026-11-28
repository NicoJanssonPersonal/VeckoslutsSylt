#if TOOLS
using Godot;

[Tool]
public partial class SpawnChanceInspector : EditorInspectorPlugin
{
    public override bool _CanHandle(GodotObject obj) => obj is WeedSpawner;

    public override bool _ParseProperty(
        GodotObject obj,
        Variant.Type type,
        string name,
        PropertyHint hintType,
        string hintString,
        PropertyUsageFlags usageFlags,
        bool wide)
    {
        if (name != nameof(WeedSpawner.Table))
            return false;

        AddPropertyEditor(name, new SpawnTableEditor());
        return false;
    }
}

#endif
