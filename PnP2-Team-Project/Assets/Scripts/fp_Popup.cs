using System.Collections;
using UnityEngine;


public class fp_Popup : MonoBehaviour
{
    [SerializeField] GameObject model;
    public float rotSpeed;
    public float riseSpeed;

    bool playerInTrigger;

    Vector3 positOrig;
    Vector3 positHide;
    
    
    Vector3 direction;
    Quaternion targetRot;


    private void Start()
    {
        positOrig = model.transform.position;
        positHide = new Vector3(positOrig.x, -0.5f, positOrig.z);
        playerInTrigger = false;
    }

    private void Update()
    {
        if (playerInTrigger)
        {
            float t = Time.deltaTime / riseSpeed;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotSpeed * Time.deltaTime);
            //model.transform.position = Vector3.Lerp(positHide, positOrig, t);
        }
        else if (!playerInTrigger)
        {
            //model.transform.position = Vector3.Lerp(positOrig, positHide, .1f + riseSpeed * Time.deltaTime);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        playerInTrigger = true;
        direction = other.transform.position - transform.position;
        targetRot = Quaternion.LookRotation(direction);
        model.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        playerInTrigger = false;
        model.SetActive(false);
        //StartCoroutine(hideWait());

    }


/*    IEnumerator hideWait()
    {
        yield return new WaitForSeconds(1f);
        model.SetActive(false);
    }*/

}
