using UnityEngine;

public class barrelTracker : MonoBehaviour
{
    public baitCatchSpawner spawner;

    private void OnDestroy()
    {
        if (spawner != null)
            spawner.BarrelDied();
    }




}
