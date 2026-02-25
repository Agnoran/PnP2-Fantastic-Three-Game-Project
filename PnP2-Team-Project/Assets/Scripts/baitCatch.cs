using UnityEngine;
using TMPro;

public class baitCatch : MonoBehaviour
{

    [Header("Bait")]
    [SerializeField] BaitType type = BaitType.Worm;
    [SerializeField] int amount = 1;

    [Header("Movement")]
    [SerializeField] float flySpeed = 3f;
    [SerializeField] float bobHeight = 0.2f;
    [SerializeField] float bobSpeed = 2f;
    [SerializeField] float spinSpeed = 90f;
    [SerializeField] float lifeTime = 15f;

    [Header("Path")] // will always be the objects tranform

   // [SerializeField] Transform startingPos;
   // [SerializeField] Transform endingPos;

    [Header("Floating Text")]
    [SerializeField] GameObject floatingText;
    [SerializeField] string promptMessage = "BAIT!";
    [SerializeField] float textBobSpeed = 0f;
    [SerializeField] float textBobHeight = 0f;

    Vector3 moveDirection;
    float baseY;
    bool collected = false;
    float spawnTime;
    Transform floatingtext;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        spawnTime = Time.time;

        moveDirection = transform.forward;

        baseY = transform.position.y;


        SetupFloatingText();



    }

    // Update is called once per frame
    void Update()
    {
        if (collected) return;

       
        transform.position += moveDirection * flySpeed * Time.deltaTime;

       

        if (Time.time - spawnTime > lifeTime)
        {
            Destroy(gameObject);
        }


        Vector3 textPos = floatingText.transform.position;          // There is a problem where the player is becoming undefined or something, gonna have to fix that somewhere.
                                                                    // Something in here is taking off the player's tag, and making it undefined somehow 

        textPos.y = 1.5f + Mathf.Sin(Time.time * textBobSpeed) * textBobHeight;
        floatingtext.localPosition = textPos;

        if(Time.time - spawnTime > lifeTime)
        {
            Debug.Log(" Bait pickup expired!");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
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

    void SetupFloatingText()
    {
        TMP_Text text = GetComponentInChildren<TMP_Text>();
        if (text != null)
        {
            text.text = promptMessage;
            floatingtext = text.transform;
        }
    }
   
}
