using Godot;

public partial class Tools : Node3D
{
    [Export] protected AudioStreamPlayer3D _audioStreamPlayer3D;
    protected RayCast3D _raycast;
    [Export] protected AnimationPlayer _animPlayer;
    protected bool _isPickingUp = false;
    [Export] protected int _damage;

    public override void _Ready()
    {
        _raycast = GetParent<RayCast3D>();
        _audioStreamPlayer3D.VolumeDb = 40.0f;

        _animPlayer.AnimationFinished += OnAnimationFinished;
    }
    protected void OnAnimationFinished(StringName animName)
    {
        // 3. Unlock once the "take" animation finishes
        if (animName == "use")
        {
            _isPickingUp = false;
        }
    }
    public virtual void useTool() {}
}
