using UnityEngine;

public class baitCatchSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] GameObject barrelPre;

    [Header("Spawn Timing")]
    [SerializeField] float spawnInterval = 15f;
    [SerializeField] float spawnVariance = 5f;

    [Header("Spawn Area")]
    [SerializeField] float spawnRangeX = 50f;
    [SerializeField] float spawnRangeZ = 50f;
    [SerializeField] float spawnY = 0.5f;

    [Header("Limits")]
    [SerializeField] int maxBarrels = 3;

    float timer;
    float nextSpawnTime;
    int currentBarrels = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetNextSpawnTime();
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextSpawnTime && currentBarrels < maxBarrels)
        {
            SpawnBarrel();
            timer = 0f;
            SetNextSpawnTime();
        }

    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = spawnInterval + Random.Range(-spawnVariance, spawnVariance);
        nextSpawnTime = Mathf.Max(nextSpawnTime, 3f);
    }

    void SpawnBarrel()
    {
        if (barrelPre == null) return;

        float x = transform.position.x + Random.Range(-spawnRangeX, spawnRangeX);
        float z = transform.position.z + Random.Range(-spawnRangeZ, spawnRangeZ);
        Vector3 spawnPos = new Vector3(x, spawnY, z);

        float randomAngle = Random.Range(0f, 360f);
        Quaternion rotation = Quaternion.Euler(0f, randomAngle, 0f);

        GameObject barrel = Instantiate(barrelPre, spawnPos, rotation);

        currentBarrels++;
        barrel.AddComponent<barrelTracker>().spawner = this;
    }
    public void BarrelDied()
    {
        currentBarrels--;
        if (currentBarrels < 0) currentBarrels = 0;
    }
}


