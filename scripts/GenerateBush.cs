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
        if (Multimesh == null || Multimesh.InstanceCount == 0)
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
            
            Vector3 outward = (pos - circleCenter).Normalized();
            float yaw = Mathf.Atan2(-outward.X, -outward.Z);

            float tiltX = (float)GD.RandRange(-TiltAmount, TiltAmount);
            float tiltZ = (float)GD.RandRange(-TiltAmount, TiltAmount);

            var basis = Basis.FromEuler(new Vector3(tiltX, yaw, tiltZ));
            // basis *= new Basis(Vector3.Back, Mathf.Pi);
            basis = basis.Scaled(Vector3.One * (float)GD.RandRange(0.8, 1.3));

            mm.SetInstanceTransform(i, new Transform3D(basis, pos));
        }

        Multimesh = mm;
    }
}
