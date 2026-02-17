using UnityEngine;

[CreateAssetMenu(fileName = "FishingPoleDefinition", menuName = "Scriptable Objects/FishingPoleDefinition")]
public class FishingPoleDefinition : ScriptableObject
{
    [SerializeField] int lineHealthMax;
    [SerializeField] int rodHealthMax;
    [SerializeField] int rodDamagePower;
    [SerializeField] float rodLuck;
    [SerializeField] float rodControl;


    public int lineHealthCurrent;
    public int rodHealthCurrent;


}
