using UnityEngine;

[CreateAssetMenu(fileName = "New Rod", menuName = "Fishing Rod Stats")]
public class rodStats : ScriptableObject
{

    [Header("Identity")]
    public string rodName;
    public GameObject rodModel;

    [Header("Health / Durability")]
    public int rodHealthMax = 100;
    public int rodHealthCur = 100;
    public int lineHealthMax = 50;
    public int lineHealthCur = 50;
    public int lineDamageOnSnap = 15;

    [Header("Performance")]
   //public float castRange = 10f;     // Im not sure if we need this 
    public float reelSpeed = 1f;
    public float rodLuck = 0f;      // RodStat.rodLuck
    public float rodControl = 1f; // RodStat.rodControl
    
    
    public bool IsBroken()
    {
        return rodHealthCur <= 0;
    }

    public bool IsLineBroken()
    {
        return lineHealthCur <= 0;
    }

    public bool IsUsable()
    {
        return rodHealthCur > 0 && lineHealthCur > 0;
    }

    public float GetRodHealthPercent()
    {
        return rodHealthMax > 0 ? (float)rodHealthCur / rodHealthMax : 0f;
    }

    public float GetLineHealthPercent()
    {
        return lineHealthMax > 0 ? (float)(lineHealthCur / lineHealthMax) : 0f;
    }

    public void RepairRod(int amount)
    {
        rodHealthCur = Mathf.Min(rodHealthCur + amount, rodHealthMax);
    }

    public void RepairLine(int amount)
    {
        lineHealthCur = Mathf.Min(lineHealthCur + amount, lineHealthMax);
    }

    public void FullRepair()
    {
            rodHealthCur = rodHealthMax;
            lineHealthCur = lineHealthMax;
    }



    public AudioClip castSound;     // May have different sounds, for different rods?
    public float castSoundVol;          // may lower, or raise the volume on different rods casts
}
