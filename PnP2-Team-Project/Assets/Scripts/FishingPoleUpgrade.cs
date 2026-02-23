using UnityEngine;

[CreateAssetMenu(fileName = "FishingPoleUpgrade", menuName = "Scriptable Objects/FishingPoleUpgrade")]
public class FishingPoleUpgrade : UpgradeDefinitions
{

    [Min(0)] [SerializeField] float lineHealthMaxUpgrade = 0;
    [Min(0)] [SerializeField] float rodHealthMaxUpgrade = 0;
    [Min(0)] [SerializeField] float rodDamagePowerUpgrade = 0;
    [Min(0)] [SerializeField] float rodLuckUpgrade = 0;
    [Min(0)] [SerializeField] float rodControlUpgrade = 0;

    public override void Apply(IUpgrade upgradeTarget)
    {
        if (upgradeTarget == null)
        {
            return;
        }

        if (lineHealthMaxUpgrade > 0)
        {
            upgradeTarget.upgradeRodStat(RodStat.lineHealthMax, lineHealthMaxUpgrade);
        }
        if(rodHealthMaxUpgrade > 0)
        {
            upgradeTarget.upgradeRodStat(RodStat.rodHealthMax, rodHealthMaxUpgrade);
        }
        if (rodDamagePowerUpgrade > 0)
        {
            upgradeTarget.upgradeRodStat(RodStat.rodDamagePower, rodDamagePowerUpgrade);
        }
        if (rodLuckUpgrade > 0)
        {
            upgradeTarget.upgradeRodStat(RodStat.rodLuck, rodLuckUpgrade);
        }
        if (rodControlUpgrade > 0)
        {
            upgradeTarget.upgradeRodStat(RodStat.rodControl, rodControlUpgrade);
        }
    }
}
