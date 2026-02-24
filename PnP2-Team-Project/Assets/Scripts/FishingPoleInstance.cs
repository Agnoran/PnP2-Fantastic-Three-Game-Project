using UnityEngine;

public class FishingPoleInstance : MonoBehaviour
{
    [Header("Base Definition")]
    [SerializeField] FishingPoleStats baseStats;

    float lineHealthMax;
    float rodHealthMax;
    float rodDamagePower;
    float rodLuck;
    float rodControl;

    float lineHealthCurrent;
    float rodHealthCurrent;

    public FishingPoleStats BaseStats => baseStats;

    public float LineHealthMax => lineHealthMax;
    public float RodHealthMax => rodHealthMax;
    public float RodDamagePower => rodDamagePower;
    public float RodLuck => rodLuck;
    public float RodControl => rodControl;

    public float LineHealthCurrent => lineHealthCurrent;
    public float RodHealthCurrent => rodHealthCurrent;

    void Awake()
    {
        InitializeFromBase();
    }

    public void InitializeFromBase()
    {
        if (baseStats == null)
        {
            Debug.LogWarning("FishingPoleInstance has no baseStats assigned.");
            return;
        }

        lineHealthMax = baseStats.LineHealthMax;
        rodHealthMax = baseStats.RodHealthMax;
        rodDamagePower = baseStats.RodDamagePower;
        rodLuck = baseStats.RodLuck;
        rodControl = baseStats.RodControl;

        lineHealthCurrent = lineHealthMax;
        rodHealthCurrent = rodHealthMax;
    }

    public void ApplyRodUpgrade(RodStat stat, float amount)
    {
        switch (stat)
        {
            case RodStat.lineHealthMax:
                lineHealthMax += amount;
                lineHealthCurrent = Mathf.Min(lineHealthCurrent, lineHealthMax);
                break;

            case RodStat.rodHealthMax:
                rodHealthMax += amount;
                rodHealthCurrent = Mathf.Min(rodHealthCurrent, rodHealthMax);
                break;

            case RodStat.rodDamagePower:
                rodDamagePower += amount;
                break;

            case RodStat.rodLuck:
                rodLuck += amount;
                break;

            case RodStat.rodControl:
                rodControl += amount;
                break;
            default:
                break;
        }
    }


    public void DamageLine(float amount)
    {
        lineHealthCurrent = Mathf.Max(0, lineHealthCurrent - amount);
    }

    public void DamageRod(float amount)
    {
        rodHealthCurrent = Mathf.Max(0, rodHealthCurrent - amount);
    }

    public void RepairLine()
    {
        lineHealthCurrent = lineHealthMax;
    }

    public void RepairRod()
    {
        rodHealthCurrent = rodHealthMax;
    }
}



