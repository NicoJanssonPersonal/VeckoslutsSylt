using Godot;
using System;

public partial class Tools : Node3D
{
    [Export] protected AudioStreamPlayer3D _audioStreamPlayer3D;
    protected RayCast3D _raycast;
    [Export] protected AnimationPlayer _animPlayer;
    protected bool _isPickingUp = false;
    public string animationName;
    public int damage;

    public override void _Ready()
    {
        _raycast = GetParent<RayCast3D>();
        _audioStreamPlayer3D.VolumeDb = -40.0f;

        _animPlayer.AnimationFinished += OnAnimationFinished;
    }
    protected void OnAnimationFinished(StringName animName)
    {
        // 3. Unlock once the "take" animation finishes
        if (animName == "klipp")
        {
            _isPickingUp = false;
        }
    }
    public virtual void useTool()
    {
        
    }
}
