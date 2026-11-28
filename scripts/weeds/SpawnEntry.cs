using Godot;

[Tool]
[GlobalClass]
public partial class SpawnEntry : Resource
{
    [Export] public PackedScene Scene { get; set; }
    [Export] public float Chance { get; set; } = 1.0f;
}
