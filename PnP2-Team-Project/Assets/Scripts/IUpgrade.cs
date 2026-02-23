using System.Runtime.InteropServices;
using UnityEngine;

public interface IUpgrade
{
    void upgradeRodStat(RodStat stat, float amount);
    void upgradeBoatStat(BoatStat stat, float amount);


    // void addBait(BaitType type, int amount);
    void addBait(int amount);
    void adjustSmell(float amount);
}
