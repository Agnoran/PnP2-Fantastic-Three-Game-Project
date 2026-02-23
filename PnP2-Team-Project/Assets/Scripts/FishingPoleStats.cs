using UnityEngine;

[CreateAssetMenu(fileName = "FishingPoleStats", menuName = "Scriptable Objects/FishingPoleStats")]
public class FishingPoleStats : ScriptableObject
{
    public GameObject rodModel;

    [SerializeField] int lineHealthMax;
    [SerializeField] int rodHealthMax;
    [SerializeField] int rodDamagePower;
    [SerializeField] float rodLuck;
    [SerializeField] float rodControl;

    public int LineHealthMax => lineHealthMax;
    public int RodHealthMax => rodHealthMax;
    public int RodDamagePower => rodDamagePower;
    public float RodLuck => rodLuck;
    public float RodControl => rodControl;

    public ParticleSystem castEffect;
    public AudioClip[] castSounds;
    [Range(0f, 1f)] public float castSoundVol = 0.8f;
}
