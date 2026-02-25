using UnityEngine;

public class baitCatchSpawner : MonoBehaviour
{
    [SerializeField] GameObject barrelToSpawn;
    [SerializeField] int spawnAmount = 5;
    [SerializeField] float spawnRate = 3f;
    [SerializeField] float spawnDist = 20f;
    [SerializeField] float spawnY = 0.5f;

    int spawnCount;
    float spawnTimer;
    bool startSpawning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      startSpawning = true;
        
    }

    // Update is called once per frame
    void Update()
    {
       if (startSpawning)
        {
            spawnTimer += Time.deltaTime;

            if (spawnCount < spawnAmount && spawnTimer >= spawnRate)
            {
                Spawn();
            }
        }

    }

    void Spawn()
    {

        spawnTimer = 0;
        spawnCount++;

        Vector3 ranPos = Random.insideUnitSphere * spawnDist;
        ranPos.y = 0;
        ranPos += transform.position;
        ranPos.y = spawnY;

        Instantiate(barrelToSpawn, ranPos, Quaternion.Euler(0, Random.Range(0f, 360f), 0f));
    }

    public void BarrelDied()
    {
        spawnCount--;
        if (spawnCount < 0) spawnCount = 0;


    }

}


