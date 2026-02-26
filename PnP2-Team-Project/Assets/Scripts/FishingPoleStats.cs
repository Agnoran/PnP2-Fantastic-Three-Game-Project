using UnityEngine;

[CreateAssetMenu(fileName = "FishingPoleStats", menuName = "Scriptable Objects/FishingPoleStats")]
public class FishingPoleStats : ScriptableObject
{
    public GameObject rodModel;

    [SerializeField] float lineHealthMax;
    [SerializeField] float rodHealthMax;
    [SerializeField] float rodDamagePower;
    [SerializeField] float rodLuck;
    [SerializeField] float rodControl;

    public float LineHealthMax => lineHealthMax;
    public float RodHealthMax => rodHealthMax;
    public float RodDamagePower => rodDamagePower;
    public float RodLuck => rodLuck;
    public float RodControl => rodControl;

    public ParticleSystem castEffect;
    public AudioClip[] castSounds;
    [Range(0f, 1f)] public float castSoundVol = 0.8f;
}
