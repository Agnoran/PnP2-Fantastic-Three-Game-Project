using UnityEngine;

[CreateAssetMenu(fileName = "Service", menuName = "Scriptable Objects/Service")]
public class Service : UpgradeDefinitions
{
    public enum ServiceType
    {
        RepairRod,
        RemoveStink,
        RepairBoat
    }

    [SerializeField] ServiceType serviceType;

    public override void Apply(IUpgrade upgradeTarget)
    {
        if (upgradeTarget == null)
        {
            return;
        }
        switch (serviceType)
        {
            case ServiceType.RepairRod:
                upgradeTarget.repairRod();
                break;
            case ServiceType.RemoveStink:
                //upgradeTarget.removeStink();
                break;
            case ServiceType.RepairBoat:
                //upgradeTarget.repairBoat();
                break;
        }
    }
}
