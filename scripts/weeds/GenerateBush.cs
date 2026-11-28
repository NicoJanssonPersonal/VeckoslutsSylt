using Godot;
using System;

[Tool]
public partial class GenerateBush : MultiMeshInstance3D
{
    [Export] public Mesh BladeMesh{ get; set; }
    [Export] public int BladeCount{ get; set; } = 60;
    [Export] public float BushRadius{ get; set; } = 0.8f;
    [Export] public float TiltAmount{ get; set; } = 0.3f;
    
    [Export] 
    public bool Regenerate
    { 
        get => false;
        set
        {
            if (value)
            {
                BuildBush();
            }
        }
    }


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        if (Multimesh == null || Multimesh.InstanceCount == 0 || Multimesh.GetReferenceCount() > 1)
        {
            BuildBush();
        }
	}

    public void BuildBush()
    {
        if (BladeMesh == null)
        {
            return;
        }

        var mm = new MultiMesh();
        mm.TransformFormat = MultiMesh.TransformFormatEnum.Transform3D;
        mm.Mesh = BladeMesh;
        mm.InstanceCount = BladeCount;

        for (int i = 0; i < BladeCount; i++) {
            float angle = GD.Randf() * Mathf.Tau;
            float dist = Mathf.Sqrt(GD.Randf()) * BushRadius;
            var pos = new Vector3(Mathf.Cos(angle) * dist, 0.0f, Mathf.Sin(angle) * dist);

            float tiltX = (float)GD.RandRange(-TiltAmount * 0.5, TiltAmount);
            float tiltZ = (float)GD.RandRange(-TiltAmount * 0.5, TiltAmount) * 0.5f;
            var basis = Basis.FromEuler(new Vector3(tiltX, GD.Randf() * Mathf.Tau, tiltZ));
            
            basis = basis.Scaled(Vector3.One * (float)GD.RandRange(0.8, 1.3));

            mm.SetInstanceTransform(i, new Transform3D(basis, pos));
        }

        Multimesh = mm;
    }

    public override void _Notification(int what)
    {
        if (what == NotificationEditorPreSave)
        {
            // Prevent Godot from serializing MultiMesh data into the .tscn file
            Multimesh = null;
        }
        else if (what == NotificationEditorPostSave)
        {
            // Restore the visual preview in the editor viewport after saving
            BuildBush();
        }
    }
}
