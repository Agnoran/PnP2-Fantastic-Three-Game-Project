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
    [SerializeField]float maxSpeed;
    [SerializeField]int baitCount;
    [SerializeField] float boatManeuverability;

    [SerializeField] FishingPoleStats curPole;      // replaced with fishingPole instance(s) array


    float origMaxHP;
    float origMaxSpeed;
    int origBaitCount;
   

     FishingPoleStats origCurPole;          // replaced with fishingPole instance(s) array

    public void adjustSmell(float amount)
    {
        throw new System.NotImplementedException();
    }

  

    public void upgradeBoatStat(BoatStat stat, float amount)
    {
        switch (stat)
        {
            case BoatStat.boatHealthMax:        // increasing YOUR maxHP by (amount)
                maxHP += amount;
                break;
            case BoatStat.boatSpeed:
                maxSpeed += amount;
                break;
            case BoatStat.boatManeuverability:
                boatManeuverability += amount;
                break;

            default:
                break;

                
        }




    }



    public void upgradeRodStat(RodStat stat, float amount)
    {
        switch (stat)
        {
            case RodStat.lineHealthMax:
                curPole.LineHealthMax += amount;
                break;
            case RodStat.ro
        }


    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
