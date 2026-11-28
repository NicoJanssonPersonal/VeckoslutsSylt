using Godot;

[Tool]
public partial class WeedSpawner : Node
{
    [Export]
    public SpawnTable Table { get; set; }

    [Export] public CollisionShape3D SpawnShape { get; set; }


    private Timer _spawnTimer;
    private readonly RandomNumberGenerator _rng = new();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _rng.Randomize();
        _spawnTimer = GetNode<Timer>("SpawnTimer");

        if (Engine.IsEditorHint())
        {
            _spawnTimer.Stop();
            return;
        }
        _spawnTimer.Timeout += SpawnObject;
        _spawnTimer.Start();
    }

    public PackedScene SpawnRandom()
    {
        if (Table == null)
            return null;

        float total = 0f;

        foreach (SpawnEntry entry in Table.Entries)
        {
            if (entry?.Scene != null && entry.Chance > 0f)
                total += entry.Chance;
        }

        if (total <= 0f)
            return null;

        float roll = _rng.Randf() * total;
        SpawnEntry lastValidEntry = null;

        foreach (SpawnEntry entry in Table.Entries)
        {
            if (entry?.Scene == null || entry.Chance <= 0f)
                continue;

            lastValidEntry = entry;

            if (roll < entry.Chance)
            {
                return entry.Scene;
            }

            roll -= entry.Chance;
        }

        // Handles the rare edge case where floating-point rounding reaches the end.
        PackedScene fallback = lastValidEntry?.Scene;

        return fallback;
    }

    private void SpawnObject()
    {
        if (Engine.IsEditorHint())
            return;

        var box = SpawnShape.Shape as BoxShape3D;
        var instance = SpawnRandom().Instantiate<Node3D>();

        Vector3 half = box.Size / 2.0f;
        var localPoint = new Vector3(
            _rng.RandfRange(-half.X, half.X),
            -half.Y,
            _rng.RandfRange(-half.Z, half.Z)
        );

        AddChild(instance);
        instance.Position = SpawnShape.GlobalTransform * localPoint;
        instance.Rotation = new Vector3(0, _rng.Randf() * Mathf.Tau, 0);
        float scale = _rng.RandfRange(0.5f, 2.0f);
        instance.Scale = new Vector3(scale, scale, scale);
    }
}
