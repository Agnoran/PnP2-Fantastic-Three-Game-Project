using UnityEngine;

public abstract class UpgradeDefinitions : ScriptableObject
{
    public abstract void Apply(IUpgrade upgradeTarget);
}
