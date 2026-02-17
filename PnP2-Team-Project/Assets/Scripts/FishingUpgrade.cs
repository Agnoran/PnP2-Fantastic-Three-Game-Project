using UnityEngine;

[CreateAssetMenu(fileName = "FishingUpgrade", menuName = "Scriptable Objects/FishingUpgrade")]
public class FishingUpgrade : ScriptableObject
{
    [SerializeField] GameObject player;

    [Min(0)] [SerializeField] int lineHealthMaxUpgrade = 0;
    [Min(0)] [SerializeField] int rodHealthMaxUpgrade = 0;
    [Min(0)] [SerializeField] int rodDamagePowerUpgrade = 0;
    [Min(0)] [SerializeField] float rodLuckUpgrade = 0;
    [Min(0)] [SerializeField] float rodControlUpgrade = 0;

    void upgrade()
    {
        IUpgrade upg = player.GetComponent<IUpgrade>();
        if (upg != null)
        {
            if (lineHealthMaxUpgrade > 0)
            {
                upg.upgradeStat("lineHealthMax", lineHealthMaxUpgrade);
            }
            if (rodHealthMaxUpgrade > 0)
            {
                upg.upgradeStat("rodHealthMax", rodHealthMaxUpgrade);
            }
            if (rodDamagePowerUpgrade > 0)
            {
                upg.upgradeStat("rodDamagePower", rodDamagePowerUpgrade);
            }
            if (rodLuckUpgrade > 0)
            {
                upg.upgradeStat("rodLuckUpgrade", rodLuckUpgrade);
            }
            if (rodControlUpgrade > 0)
            {
                upg.upgradeStat("rodControlUpgrade", rodLuckUpgrade);
            }
        }
    }
}
