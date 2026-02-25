using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Minigame_CutLine : MonoBehaviour
{
    public static Minigame_CutLine instance;


    [SerializeField] GameObject bigWarningText;
    [SerializeField] TextMeshPro lineDisplay;
    [SerializeField] float scaleValue;
    float testLineHealth;

    Vector3 maxWarningSize;
    Vector3 minWarningSize;
    Vector3 targetSize;
    Vector3 currentSize;

    float scaleTimer;
    [SerializeField] float testScaleTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //get line health
        testLineHealth = 1000f;

        //set up target sizes
        maxWarningSize = new Vector3(bigWarningText.transform.localScale.x * 2, bigWarningText.transform.localScale.y * 2, bigWarningText.transform.localScale.z);
        minWarningSize = bigWarningText.transform.localScale;
        targetSize = maxWarningSize;
    }

    // Update is called once per frame
    void Update()
    {
        testLineHealth -= 1 * Time.deltaTime;

        pulseWarning();
    }

    void pulseWarning()
    {

        currentSize = bigWarningText.transform.localScale;
        //resize
        bigWarningText.transform.localScale = Vector3.Lerp(currentSize, targetSize, scaleValue * Time.deltaTime);
        //check target
        if (scaleTimer > testScaleTime)
        {
            targetSize = minWarningSize;
            scaleTimer -= Time.deltaTime;
        }
        else if (scaleTimer < 0)
        {
            targetSize = maxWarningSize;
            scaleTimer += Time.deltaTime;
        }

    }
}
