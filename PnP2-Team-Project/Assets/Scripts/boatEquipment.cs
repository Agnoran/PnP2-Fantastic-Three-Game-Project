using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class boatEquipment : MonoBehaviour
{

    [Header("Fishing Rod")]
    [SerializeField] List<rodStats> rodList = new List<rodStats>();
    [SerializeField] GameObject rodModel;
    int rodListPos;
    int selectedBaitIndex;

    [Header("Bait Inventory")]
    [SerializeField] List<Bait> baitInventory = new List<Bait>();

    [Header("UI")]
    [SerializeField] GameObject fishingPromptUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (fishingPromptUI != null)
            fishingPromptUI.SetActive(false);

        if (rodList.Count > 0)
            changeRod();


    }


    // Update is called once per frame
    void Update()
    {
        if (WorldController.instance != null && WorldController.instance.IsMenuOpen())
            return;

        selectRod();

        if (Input.GetKeyDown(KeyCode.Q))
            cycleBait(-1);
        if (Input.GetKeyDown(KeyCode.E))
            cycleBait(1);
    }

    // Triggers for prompting UI

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FishingSpot"))
        {
            if (fishingPromptUI != null)
                fishingPromptUI.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FishingSpot"))
        {
            if (fishingPromptUI != null) 
                fishingPromptUI.SetActive(false);
        }
    }

    // ROD SYSTEM (SCROLL WHEEL TO CHANGE, DAMAGE ROD, DAMAGE LINE) 

    void selectRod()
    {
        if (rodList.Count <= 1) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0 && rodListPos < rodList.Count - 1)
        {
            rodListPos++;
            changeRod();
        }
        else if (scroll < 0 && rodListPos > 0)
        {
            rodListPos--;
            changeRod();
        }

    }

    void changeRod()
    {
        if (rodList.Count == 0) return;

        rodStats rod = rodList[rodListPos];

        if (rod.rodModel != null && rodModel != null)
        {
            MeshFilter rodMF = rodModel.GetComponent<MeshFilter>();
            MeshRenderer rodMR = rodModel.GetComponent<MeshRenderer>();
            MeshFilter newMF = rod.rodModel.GetComponent<MeshFilter>();
            MeshRenderer newMR = rod.rodModel.GetComponent<MeshRenderer>();

            if (rodMF != null && newMF != null)
                rodMF.sharedMesh = newMF.sharedMesh;
            if (rodMR != null && newMR != null)
                rodMR.sharedMaterial = newMR.sharedMaterial;
        }

        Debug.Log("Rod: " + rod.rodName
            + " | Rod HP: " + rod.rodHealthCur + "/" + rod.rodHealthMax
            + " | Line HP: " + rod.lineHealthCur + "/" + rod.lineHealthMax);
    }

    public void getRodStats(rodStats rod)
    {
        rodList.Add(rod);
        rodListPos = rodList.Count - 1;
        changeRod();
    }

    public void damageRod(int amount)
    {
        if (rodList.Count == 0) return;

        rodStats rod = rodList[rodListPos];
        rod.rodHealthCur = Mathf.Max(rod.rodHealthCur -  amount, 0);

        Debug.Log(rod.rodName + " rod damaged! " + rod.lineHealthCur + "/" + rod.lineHealthMax);
        if (rod.IsLineBroken())
            Debug.Log(rod.rodName + " line snapped! ");
    }

    public void damageLine(int amount)
    {
        if (rodList.Count == 0) return;

        rodStats rod = rodList[rodListPos];
        rod.lineHealthCur = Mathf.Max(rod.lineHealthCur - amount, 0);

        Debug.Log(rod.rodName + " line damaged! " + rod.lineHealthCur + "/" + rod.lineHealthMax);
        if (rod.IsLineBroken())
            Debug.Log(rod.rodName + " line SNAPPED!");
    }

    void cycleBait(int direction)
    {
        if (baitInventory.Count == 0) return;

        selectedBaitIndex += direction;

        if (selectedBaitIndex < 0)
            selectedBaitIndex = baitInventory.Count - 1;
        else if (selectedBaitIndex >= baitInventory.Count)
            selectedBaitIndex = 0;

        Bait b = baitInventory[selectedBaitIndex];
        Debug.Log("Bait: " + b.baitName + " x" + b.quantity);
    }

    public void useBait()
    {
        Bait bait = getSelectedBait();
        if (bait == null) return;

        bait.Use();
        Debug.Log("Used " + bait.baitName + " (" + bait.quantity + " left)");
    }

    public void addBait(BaitType type, int amount)
    {
        for (int i = 0; i < baitInventory.Count; i++)
        {
            if (baitInventory[i].baitType == type)
            {
                baitInventory[i].Add(amount);
                Debug.Log(type + " +" + amount + " (total: " + baitInventory[i].quantity + ")");
                return;
            }
        }


        baitInventory.Add(new Bait
        {
            baitName = type.ToString(),
            baitType = type,
            quantity = amount
        });
    }


public Bait getSelectedBait()
{
    if (baitInventory.Count == 0) return null;
    return baitInventory[selectedBaitIndex];
}


}