using UnityEngine;

[CreateAssetMenu(fileName = "BaitItem", menuName = "Scriptable Objects/BaitItem")]
public class BaitItem : UpgradeDefinitions
{
    [SerializeField] BaitType baitType;
    [Min(0)][SerializeField] int quantity;

    public override void Apply(IUpgrade upgradeTarget)
    {
        if(upgradeTarget == null)
        {
            return;
        }
        if(quantity > 0)
        {
            upgradeTarget.addBait(quantity);
        }
    }
}
