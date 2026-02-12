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
        int amountOfTypes = 0;
        for (int i = 0; i < availableFishies.Length; i++)
        {
            amountOfTypes++;
        }

        // choice will include weight values in the future when we have them, for now it's just random
        int choice = Random.Range(0, amountOfTypes);


        FishDefinition chosenFish = availableFishies[choice];
        float rolledSize = Random.Range(
            chosenFish.SizeMin,
            chosenFish.SizeMax
        );
        int rolledQuality = Random.Range(0, 100);
        //int rolledValue = Mathf.RoundToInt(chosenFish.BaseValue * (1 + (rolledQuality / 100f)) * (1 + (rolledSize / chosenFish.SizeMax)));
        int rolledValue = chosenFish.BaseValue;
        float rolledSpoilTimeSeconds = chosenFish.BaseSpoilTime * (1 + (rolledQuality / 100f));

        return new FishInstance(chosenFish, rolledSize, rolledQuality, rolledValue, rolledSpoilTimeSeconds);
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
