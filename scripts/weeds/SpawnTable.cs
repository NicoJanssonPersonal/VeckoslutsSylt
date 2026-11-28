using Godot;

[Tool]
[GlobalClass]
public partial class SpawnTable : Resource
{
    [Export]
    public Godot.Collections.Array<SpawnEntry> Entries { get; set; } = new();

    public void SetChance(int changedIndex, float newChance)
    {
        if (changedIndex < 0 || changedIndex >= Entries.Count ||
            Entries[changedIndex] == null)
            return;

        newChance = Mathf.Clamp(newChance, 0f, 1f);

        float otherTotal = 0f;
        int otherCount = 0;

        for (int i = 0; i < Entries.Count; i++)
        {
            if (i == changedIndex || Entries[i] == null)
                continue;

            otherTotal += Mathf.Max(0f, Entries[i].Chance);
            otherCount++;
        }

        if (otherCount == 0)
        {
            Entries[changedIndex].Chance = 1f;
            return;
        }

        Entries[changedIndex].Chance = newChance;
        float remaining = 1f - newChance;

        for (int i = 0; i < Entries.Count; i++)
        {
            if (i == changedIndex || Entries[i] == null)
                continue;

            Entries[i].Chance = otherTotal > 0f
                ? Mathf.Max(0f, Entries[i].Chance) / otherTotal * remaining
                : remaining / otherCount;
        }
    }
}
