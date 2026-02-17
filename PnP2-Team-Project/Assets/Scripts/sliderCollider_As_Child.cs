using UnityEditor;
using UnityEngine;

public class sliderCollider_As_Child : Minigame_Loops
{
    public static sliderCollider_As_Child instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (targetCenter.transform.position == leftCenter.transform.position)
        {
            targetCenter = rightCenter;
        }
        else
        {
            targetCenter = leftCenter;
        }
    }



}
