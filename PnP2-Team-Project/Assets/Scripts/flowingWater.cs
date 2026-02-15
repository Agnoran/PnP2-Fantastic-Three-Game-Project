using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System;

public class flowingWater : MonoBehaviour
{

    // water flowing animation

    [SerializeField] private float flowSpeedX = 0.05f;
    [SerializeField] private float flowSpeedY = 0.03f;

    // water waves animation
    [SerializeField] private bool enableWaveAnimation = false;
    [SerializeField] private float waveHeight = 0.3f;
    [SerializeField] private float waveSpeed = 1f;
    [SerializeField] private float waveFrequency = 2f;

    private Renderer waterRenderer;
    private Material waterMaterial;

   public  Color deepWaterColor = new Color(0.1f, 0.2f, 0.4f, 1f);
   public Color shallowWaterColor = new Color(0.2f, 0.5f, 0.7f, 1f);

    
    private MeshFilter meshFilter;
    private Mesh mesh;
    private Vector3[] baseVertices;
    private Vector3[] newVertices;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the material

        waterRenderer = GetComponent<Renderer>();
        if (waterRenderer != null)
        {
           Debug.LogError("flowingWater: No renderer found " + gameObject.name);
            return;
        }

        waterMaterial = waterRenderer.material;

        //if (enableWaveAnimation)
        //{
        //    mesh = meshFilter.mesh;
        //    baseVertices = mesh.vertices;
        //    baseVertices = new Vector3[baseVertices.Length];

        //    Debug.Log("Water mesh has " + baseVertices.Length + " vertices ");

        //    if (baseVertices.Length < 100)
        //    {
        //        Debug.LogWarning("Lets see: Water plane needs more for animations");
        //        enableWaveAnimation = false;
        //    }
        //}
        
    }

    // Update is called once per frame
    void Update()
    {
        if (waterMaterial == null) return;

        float offsetX = Time.time * flowSpeedX;
        float offsetY = Time.time * flowSpeedY;
        waterMaterial.mainTextureOffset = new Vector2(offsetX, offsetY);

        if (enableWaveAnimation)
        {
            float newY = Mathf.Sin(Time.time * waveFrequency) * waveHeight;
            transform.position = new Vector3(
                transform.position.x,
                newY,
                transform.position.z
                );
        }
    }

    

    //void AnimateWaterFlow()
    //{
    //    float offsetX = Time.time * flowSpeedX;
    //    float offsetY = Time.time * flowSpeedY;

    //    waterMaterial.mainTextureOffset = new Vector2(offsetX, offsetY);
    //}

    //void AnimateWaveVertices()
    //{
    //    for (int i = 0; i < newVertices.Length; i++)
    //    {
    //        Vector3 vertex = baseVertices[i];

    //        float wave1 = Mathf.Sin(vertex.x * waveFrequency + Time.time * waveSpeed) * waveHeight;
    //        float wave2 = Mathf.Cos(vertex.z * waveFrequency * 0.7f + Time.time * waveSpeed * 1.3f) * waveHeight * 0.5f;

    //        baseVertices[i] = new Vector3(vertex.x, wave1 + wave2, vertex.z);
    //    }

    //    mesh.vertices = newVertices;
    //    mesh.RecalculateNormals();
    //    mesh.RecalculateBounds();
    //}

    //public float GetWaterHeighAtPosition(Vector3 worldPosition)
    //{

    //    if (!enableWaveAnimation)
    //    {
    //        return transform.position.y;
    //    }
    //    Vector3 localPos = transform.InverseTransformPoint(worldPosition);

    //    float wave1 = Mathf.Sin(localPos.x * waveFrequency + Time.time * waveSpeed) * waveHeight;
    //    float wave2 = Mathf.Cos(localPos.z * waveFrequency * 0.7f + Time.time * waveSpeed * 1.3f) * waveHeight * 0.5f;

    //    return transform.position.y + wave1 + wave2;
    //}


}
