using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InGameInfo_Chunk : MonoBehaviour
{
    public static InGameInfo_Chunk instance;

    //[Header("public so it can")]
    //[SerializeField] public UnityEngine.UI.Image obfuscatorImage;

    [Header("left text")]
    [SerializeField] GameObject titleObj;
    [SerializeField] int fontSizeA;
    
    [Header("right text")]
    [SerializeField] GameObject valueObj;
    [SerializeField] int fontSizeB;

    [Header("fish images - hardcoded for testing")]
    [SerializeField] UnityEngine.UI.Image silhouette;
    [SerializeField] UnityEngine.UI.Image fullImage;
    UnityEngine.UI.Image fishImage;

    //private void Awake()
    //{
    //    if (instance != null && instance != this)
    //    {
    //        Destroy(this);
    //        return;
    //    }
    //    instance = this;
    //}

    public InGameInfo_Chunk createChunk_Image(FishType type, bool knownFish)
    {
        //use fish type to grab the appropriate images

        //add image field
        this.AddComponent<UnityEngine.UI.Image>();
        if (knownFish)
        {
            fishImage = fullImage;
        }
        else
        {
            fishImage = silhouette;
        }
        this.GetComponent<UnityEngine.UI.Image>().sprite = fishImage.sprite;

        return this;
    }


    public InGameInfo_Chunk createChunk_Text(string title, string value)
    {
        //instantiate 
        //InGameInfo_Chunk chunk = Instantiate(this);

        //fill text
        titleObj.GetComponent<TextMeshPro>().text = title;
        //format
        titleObj.GetComponent<TextMeshPro>().fontSize = fontSizeA;
        titleObj.GetComponent<TextMeshPro>().fontStyle = FontStyles.Bold;

        //fill text
        valueObj.GetComponent<TextMeshPro>().text = value;
        //format
        valueObj.GetComponent<TextMeshPro>().fontSize = fontSizeB;
        valueObj.GetComponent<TextMeshPro>().fontStyle = FontStyles.Italic;

        return this;
    }

    
    public void revealField()
    {
        GetComponentInChildren<UnityEngine.UI.Image>().enabled = false;
    }

}
