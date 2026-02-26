using UnityEngine;

public class FishingSpot : MonoBehaviour
{
    // actual fishing spot name
    public string spotName = "Fishing Spot";
    // actual fish available to be fished, or catch from the spot(s)
    [Tooltip("What types of fish can be caught here")]
    public string[] availableFish = { "Bass", "Trout" };
    [SerializeField] FishDefinition[] availableFishies;
    // how rare are the fish
    [Tooltip("How rare are catches here? 0 = common, 1 = uncommon, 2  = legendary ")]
    [Range(0f,2f)]
    public float rarityLevel = 0f;

    public GameObject visualIndicator;



    // Start is called once before the first execution of Update after the MonoBehaviour is created...!!! 
    void Start()
    {
        Collider col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
        {
            Debug.LogWarning(spotName + ": Collider is not set to is Trigger! Fixing automatically. ");
            col.isTrigger = true;
        }
        
        if (!gameObject.CompareTag("FishingSpot"))
        {
            Debug.LogWarning(spotName + ": Tag is not 'FishingSpot'! Make sure to create and assign the tag");
        }
    }

    //public string GetRandomFish()
    //{
    //    if (availableFish.Length == 0) return "Old Boot";
    //    int index = Random.Range(0, availableFish.Length);
    //    return availableFish[index];
    //}

    public FishInstance GenerateFishToAttempt()
    {
        if (availableFishies == null || availableFishies.Length == 0)
            return null;

        float totalWeight = 0f;

        for (int i = 0; i < availableFishies.Length; i++)
        {
            FishDefinition fish = availableFishies[i];
            if (fish == null)
                continue;

            float w = Mathf.Max(0f, fish.SpawnWeight);
            totalWeight += w;
        }

        if (totalWeight <= 0f)
            return null;

        float roll = Random.Range(0f, totalWeight);

        FishDefinition chosenFish = null;

        for (int i = 0; i < availableFishies.Length; i++)
        {
            FishDefinition fish = availableFishies[i];
            if (fish == null)
                continue;

            roll -= Mathf.Max(0f, fish.SpawnWeight);

            if (roll <= 0f)
            {
                chosenFish = fish;
                break;
            }
        }

        if (chosenFish == null)
        {
            for (int i = availableFishies.Length - 1; i >= 0; i--)
            {
                if (availableFishies[i] != null)
                {
                    chosenFish = availableFishies[i];
                    break;
                }
            }

            if (chosenFish == null)
                return null;
        }

        float rolledSize = Random.Range(chosenFish.SizeMin, chosenFish.SizeMax);
        int rolledQuality = Random.Range(0, 100);

        float qualityMultiplier = 1f + (rolledQuality / 100f);
        float sizeMultiplier = 1f + (rolledSize / Mathf.Max(0.0001f, chosenFish.SizeMax));

        int rolledValue = Mathf.RoundToInt(chosenFish.BaseValue * qualityMultiplier * sizeMultiplier);

        float rolledSpoilTimeSeconds = chosenFish.BaseSpoilTime;

        return new FishInstance(chosenFish, rolledSize, rolledQuality, rolledValue, rolledSpoilTimeSeconds);
    }
    public FishInstance GenerateFishToAttempt(float rodLuckBonus)
    {
        if (availableFishies == null || availableFishies.Length == 0)
            return null;

        float totalWeight = 0f;

        for (int i = 0; i < availableFishies.Length; i++)
        {
            FishDefinition fish = availableFishies[i];
            if (fish == null)
                continue;

            float w = Mathf.Max(0f, fish.SpawnWeight);
            totalWeight += w;
        }

        if (totalWeight <= 0f)
            return null;

        float roll = Random.Range(0f, totalWeight);

        FishDefinition chosenFish = null;

        for (int i = 0; i < availableFishies.Length; i++)
        {
            FishDefinition fish = availableFishies[i];
            if (fish == null)
                continue;

            roll -= Mathf.Max(0f, fish.SpawnWeight);

            if (roll <= 0f)
            {
                chosenFish = fish;
                break;
            }
        }

        if (chosenFish == null)
        {
            for (int i = availableFishies.Length - 1; i >= 0; i--)
            {
                if (availableFishies[i] != null)
                {
                    chosenFish = availableFishies[i];
                    break;
                }
            }

            if (chosenFish == null)
                return null;
        }

        float rolledSize = Random.Range(chosenFish.SizeMin, chosenFish.SizeMax);

        int rolledQuality = Random.Range(0, 100);
        int finalQuality = Mathf.Clamp(Mathf.RoundToInt(rolledQuality + rodLuckBonus), 0, 100);

        float qualityMultiplier = 1f + (finalQuality / 100f);
        float sizeMultiplier = 1f + (rolledSize / Mathf.Max(0.0001f, chosenFish.SizeMax));

        int rolledValue = Mathf.RoundToInt(chosenFish.BaseValue * qualityMultiplier * sizeMultiplier);

        float rolledSpoilTimeSeconds = chosenFish.BaseSpoilTime;

        return new FishInstance(chosenFish, rolledSize, finalQuality, rolledValue, rolledSpoilTimeSeconds);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WorldController.instance.EnterPool(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WorldController.instance.ExitPool();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
