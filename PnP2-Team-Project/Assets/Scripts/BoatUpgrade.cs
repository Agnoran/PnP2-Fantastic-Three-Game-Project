using UnityEngine;

[CreateAssetMenu(fileName = "BoatUpgrade", menuName = "Scriptable Objects/BoatUpgrade")]
public class BoatUpgrade : UpgradeDefinitions
{
    [Min(0)][SerializeField] float maxHPUpgrade;
    [Min(0)][SerializeField] float maxSpeedUprade;
    [Min(0)][SerializeField] float boatManeuverabilityUpgrade;

    public override void Apply(IUpgrade upgradeTarget)
    {
        if(upgradeTarget ==null) return;
        if(maxHPUpgrade > 0)
        {
            upgradeTarget.upgradeBoatStat(BoatStat.boatHealthMax, maxHPUpgrade);
        }
        if (maxSpeedUprade > 0)
        {
            upgradeTarget.upgradeBoatStat(BoatStat.boatSpeed, maxSpeedUprade);
        }
        if (boatManeuverabilityUpgrade > 0)
        {
            upgradeTarget.upgradeBoatStat(BoatStat.boatManeuverability, boatManeuverabilityUpgrade);
        }
    }
}
