using Godot;

public partial class CameraOrbit : Node3D
{
    [Export]
    public float RotationSpeed { get; set; } = 0.25f; // Radians per second

    public override void _Process(double delta)
    {
        RotateY(RotationSpeed * (float)delta);
    }
}