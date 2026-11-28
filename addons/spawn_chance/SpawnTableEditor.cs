#if TOOLS
using System.Collections.Generic;
using Godot;

[Tool]
public partial class SpawnTableEditor : EditorProperty
{
    private readonly VBoxContainer _rows = new();
    private readonly List<(HSlider Slider, Label PercentLabel)> _rowControls = new();
    private SpawnTable _renderedTable;
    private int _renderedCount = -1;
    private bool _updating;

    public SpawnTableEditor()
    {
        AddChild(_rows);
        SetBottomEditor(_rows);
    }

    public override void _UpdateProperty()
    {
        if (_updating)
            return;

        SpawnTable table = ReadTable();

        // Only rebuild controls if the table or entry count changed; otherwise sync in-place
        if (table == _renderedTable && (table?.Entries.Count ?? -1) == _renderedCount)
        {
            SyncControls(table);
            return;
        }

        RebuildRows(table);
    }

    private SpawnTable ReadTable()
    {
        Variant value = GetEditedObject().Get(GetEditedProperty());
        return value.AsGodotObject() as SpawnTable;
    }

    private void SyncControls(SpawnTable table)
    {
        if (table == null)
            return;

        _updating = true;
        try
        {
            for (int i = 0; i < _rowControls.Count && i < table.Entries.Count; i++)
            {
                float chance = table.Entries[i]?.Chance ?? 0f;
                _rowControls[i].Slider.SetValueNoSignal(chance);
                _rowControls[i].PercentLabel.Text = $"{chance * 100:0.#}%";
            }
        }
        finally
        {
            _updating = false;
        }
    }

    private void RebuildRows(SpawnTable table)
    {
        _updating = true;
        _rowControls.Clear();
        _renderedTable = table;
        _renderedCount = table?.Entries.Count ?? -1;

        foreach (Node child in _rows.GetChildren())
        {
            _rows.RemoveChild(child);
            child.QueueFree();
        }

        if (table == null)
        {
            _rows.AddChild(new Label { Text = "Assign a SpawnTable resource below." });
            _updating = false;
            return;
        }

        for (int i = 0; i < table.Entries.Count; i++)
        {
            int index = i;
            SpawnEntry entry = table.Entries[index];

            if (entry == null)
            {
                entry = new SpawnEntry { Chance = table.Entries.Count == 1 ? 1f : 0f };
                table.Entries[index] = entry;
            }

            var row = new HBoxContainer();

            var picker = new EditorResourcePicker
            {
                BaseType = nameof(PackedScene),
                EditedResource = entry.Scene,
                CustomMinimumSize = new Vector2(140, 0),
                SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
            };
            picker.ResourceChanged += res =>
            {
                entry.Scene = res as PackedScene;
                table.EmitChanged();
                EmitChanged(GetEditedProperty(), table);
            };

            var slider = new HSlider
            {
                MinValue = 0,
                MaxValue = 1,
                Step = 0.01,
                Value = entry.Chance,
                SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
            };

            var percentLabel = new Label
            {
                Text = $"{slider.Value * 100:0.#}%",
                CustomMinimumSize = new Vector2(45, 0)
            };

            slider.ValueChanged += value =>
            {
                if (!_updating)
                    ApplyChance(index, (float)value);
            };

            AddFocusable(slider);
            row.AddChild(picker);
            row.AddChild(slider);
            row.AddChild(percentLabel);
            _rows.AddChild(row);
            _rowControls.Add((slider, percentLabel));
        }

        var addButton = new Button { Text = "+ Add Entry" };
        addButton.Pressed += () =>
        {
            table.Entries.Add(new SpawnEntry { Chance = table.Entries.Count == 0 ? 1f : 0f });
            table.EmitChanged();
            EmitChanged(GetEditedProperty(), table);
            RebuildRows(table);
        };
        _rows.AddChild(addButton);

        _updating = false;
    }

    private void ApplyChance(int index, float chance)
    {
        SpawnTable current = ReadTable();
        if (current == null)
            return;

        _updating = true;
        try
        {
            current.SetChance(index, chance);
            SyncControls(current);
            current.EmitChanged();
            EmitChanged(GetEditedProperty(), current);
        }
        finally
        {
            _updating = false;
        }
    }
}
#endif
