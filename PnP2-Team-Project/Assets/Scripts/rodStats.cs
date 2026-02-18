using UnityEngine;

[CreateAssetMenu(fileName = "New Rod", menuName = "Fishing Rod Stats")]
public class rodStats : ScriptableObject
{
    public string rodName;
    public int durabilityMax;
    public int durabilityCur;
    public float lineStrength;
   // public float castRange;     // Im not sure if we need this 
    public float reelSpeed;
    public int lineDamageOnSnap;
    public GameObject rodModel;

    public AudioClip castSound;     // May have different sounds, for different rods?
    public float castSoundVol;          // may lower, or raise the volume on different rods casts
}
