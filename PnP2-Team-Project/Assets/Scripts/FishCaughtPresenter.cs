using UnityEngine;

// This is a script to create a dragable fish after a catch.
// this fish is not in the inventory yet

public class FishCaughtPresenter : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] FishItemUI caughtFishPrefab;
    [SerializeField] RectTransform caughtFishParent;

    FishItemUI spawned;

    void OnEnable()
    {
        SpawnCaughtFish();
    }

    void OnDisable()
    {
        if (spawned != null)
        {
            Destroy(spawned.gameObject);
            spawned = null;
        }
    }

    void SpawnCaughtFish()
    {
        // check for a world controller and a fish that was "caught"
        if (WorldController.instance == null) return;
        if (WorldController.instance.fishToAttempt == null) return;

        // make sure we have a fishitem to display and a location to spawn it
        if (caughtFishPrefab == null || caughtFishParent == null) return;

        // create the item at the correct spot and make a reference to it
        spawned = Instantiate(caughtFishPrefab, caughtFishParent);

        // IMPORTANT: we still bind it to a grid view so drag-drop works.
        // If your FishCaughtMenu already has an InventoryGridView on the right,
        // drag that into this field or find it at runtime.
        InventoryGridView view = GetComponentInChildren<InventoryGridView>(true);
        spawned.BindFish(WorldController.instance.fishToAttempt, view);
    }

    /// <summary>
    /// Future Button that uses the inventory system's autoplace
    /// </summary>
    public void AutoPlaceCaughtFish()
    {
        if (WorldController.instance == null) return;
        FishInstance fish = WorldController.instance.fishToAttempt;
        if (fish == null) return;

        bool placed = InventorySystem.instance != null && InventorySystem.instance.TryAutoPlaceFish(fish);
        if (placed)
        {
            WorldController.instance.FinishFishingResult();
        }
    }
}