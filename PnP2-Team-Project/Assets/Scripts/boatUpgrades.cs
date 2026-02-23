using UnityEngine;

public class boatUpgrades : MonoBehaviour, IUpgrade
{

    boatMovement movement;
    boatEquipment equipment;
    boatHealth health;

    // smell

    // float smell;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement = GetComponent<boatMovement>();
        equipment = GetComponent<boatEquipment>();
        //maybe stink ass well later on would go here 
        health = GetComponent<boatHealth>();
    }

    public void upgradeRodStat(RodStat stat, float amount)
    {
        rodStats rod = GetCurrentRod();
        if (rod == null) return;

        switch (stat)
        {
            case RodStat.lineHealthMax:
                rod.lineHealthMax += amount;
                rod.lineHealthCur += amount;
                Debug.Log("Line health +" + amount + " ->" + rod.lineHealthCur + "/" + rod.lineHealthMax);
                break;

            case RodStat.rodHealthMax:
                rod.rodHealthMax += amount;
                rod.rodHealthCur += amount;
                Debug.Log("Rod health +" + amount + " ->" + rod.rodHealthCur + "/" + rod.rodHealthMax);
                break;

            case RodStat.rodDamagePower:
                rod.rodDamagePower += amount;
                Debug.Log("Rod damage power +" + amount + " ->" + rod.rodDamagePower);
                break;

            case RodStat.rodLuck:
                rod.rodLuck += amount;
                Debug.Log("Rod luck +" + amount + " -> " + rod.rodLuck);
                break;

            case RodStat.rodControl:
                rod.rodControl += amount;
                Debug.Log("Rod Control + " + amount + " ->" + rod.rodControl);
                break;
        }


    }

    public void upgradeBoatStat(BoatStat stat, float amount)
    {
        switch (stat)
        {
            case BoatStat.boatHealthMax:
                if (health != null)
                {
                    health.ModifyHP((int)amount);
                    Debug.Log("Boat HP + " + amount);
                }
                break;

            case BoatStat.boatSpeed:
                if (movement != null)
                {
                    movement.ModifyMoveSpeed(amount);
                    movement.ModifyMaxSpeed(amount);
                    Debug.Log("Boat speed +" + amount);
                }
                break;
            case BoatStat.boatManueverability:
                if (movement != null)
                {
                    movement.ModifyTurnSpeed(amount);
                    Debug.Log("Boat manueverability +" + amount);
                }
                break;
        }
    }

    public void addBait(BaitType type, int amount)
    {
        if (equipment != null)
            equipment.addBait(type, amount);
    }

    //public void adjustSmell(float amount)
    //{
    //    smell += amount;
    //}

        // Update is called once per frame
    //    void Update()
    //{
        
    //}

    rodStats GetCurrentRod()
    {
        if (equipment == null)
        {
            Debug.LogWarning("No boatEquipment found.");
            return null;
        }

        return equipment.GetCurrentRod();
    }

    //public float GetSmell()
    //{
    //    return Get
    //}
}
