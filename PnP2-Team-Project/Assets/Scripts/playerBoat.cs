using UnityEngine;

public class playerBoat : MonoBehaviour, IUpgrade
{
    //public enum BoatStat
    //{
    //    boatHealthMax,            // ACTUAL VALUES, or name of val
    //    boatSpeed,
    //    boatManeuverability
    //}


    //Storing stats

    [SerializeField]float maxHP;
    [SerializeField]public float maxSpeed;
    [SerializeField]int baitCount;
    [SerializeField] float boatManeuverability;

   // [SerializeField] int curPoleDamagePower;

    [SerializeField] FishingPoleInstance curPole;      // replaced with fishingPole instance(s) array


    float origMaxHP;
    float origMaxSpeed;
    int origBaitCount;
   

     FishingPoleInstance origCurPole;          // replaced with fishingPole instance(s) array

    public void adjustSmell(float amount)
    {
        throw new System.NotImplementedException();
    }

  

    public void upgradeBoatStat(BoatStat stat, float amount)
    {
        boatMovement boatMovement = GetComponentInParent<boatMovement>();

        switch (stat)
        {
            case BoatStat.boatHealthMax:        // increasing YOUR maxHP by (amount)
                maxHP += amount;
                break;
            case BoatStat.boatSpeed:
                maxSpeed += amount;
                boatMovement.ModifyMaxSpeed(amount);
                break;
            case BoatStat.boatManeuverability:
                boatManeuverability += amount;      // same as MaxSpeed
                break;

            default:
                break;

                
        }




    }



    public void upgradeRodStat(RodStat stat, float amount)
    {
        curPole.ApplyRodUpgrade(stat, amount);


    }

    public void damageCurrentRod(int amount)
    {
        curPole.DamageRod(amount);
    }

    public void repairRod()
    {
        curPole.RepairRod();
    }

    public FishingPoleInstance getCurrRod()             // access curPole
    {
        return curPole;
    }



    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        boatMovement boatMovement = GetComponent<boatMovement>();
        boatMovement.ModifyMaxSpeed(maxSpeed);
        

    }

    // Update is called once per frame
    void Update()
    {
        
       

    }

    public void addBait(int amount)
    {
        baitCount += amount;
    }

    public int getCurrBait()
    {
        return baitCount;
    }
}
