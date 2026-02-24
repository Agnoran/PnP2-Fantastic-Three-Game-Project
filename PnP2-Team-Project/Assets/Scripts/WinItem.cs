using UnityEngine;

[CreateAssetMenu(fileName = "WinItem", menuName = "Scriptable Objects/WinItem")]
public class WinItem : UpgradeDefinitions
{
    public override void Apply(IUpgrade upgradeTarget)
    {
        if (WorldController.instance == null)
            return;

        WorldController.instance.StateWinGame();
        return;
    }
}