using UnityEngine;

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
        if (WorldController.instance == null) return;
        if (WorldController.instance.fishToAttempt == null) return;

        if (caughtFishPrefab == null || caughtFishParent == null) return;

        spawned = Instantiate(caughtFishPrefab, caughtFishParent);

        // IMPORTANT: we still bind it to a grid view so drag-drop works.
        // If your FishCaughtMenu already has an InventoryGridView on the right,
        // drag that into this field or find it at runtime.
        InventoryGridView view = GetComponentInChildren<InventoryGridView>(true);
        spawned.BindFish(WorldController.instance.fishToAttempt, view);
    }

    // Optional: call from an Auto-Place button
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