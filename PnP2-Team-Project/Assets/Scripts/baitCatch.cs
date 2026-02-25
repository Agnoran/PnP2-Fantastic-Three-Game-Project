using UnityEngine;
using TMPro;

public class baitCatch : MonoBehaviour
{

    [Header("Bait")]
    [SerializeField] BaitType type = BaitType.Worm;
    [SerializeField] int amount = 1;

    bool collected = false;

   

    [Header("Floating Text")]
    [SerializeField] GameObject floatingText;
    [SerializeField] string promptMessage = "BAIT!";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Collider col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
            col.isTrigger = true;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {

        Debug.Log("Something hit me : " + other.gameObject.name);

        //audio boatSounds

        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            boatEquipment equip = other.GetComponent<boatEquipment>();
            if (equip != null)
            {
                equip.addBait(type, amount);
                Debug.Log(" Bait Caught!");
            }

            boatSounds sounds = other.GetComponent<boatSounds>();
            if (sounds != null)
            {
                sounds.PlayPickup();
               
            }

            Destroy(gameObject);

        }

    }

 
   
}
