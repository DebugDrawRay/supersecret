using UnityEngine;
using System.Collections;

public class ObjectFactory : MonoBehaviour
{
    public GameObject objectToSpawn;

    public float spawnDistance;

    [Range(0,1)]
    public float spawnFrequency;

    public float spawnRate;
    private float currentSpawnRate;

    private Grid grid;
    private float[] gridXPoints;

    private bool initialized;

    public void Init()
    {
        grid = Grid.instance;
        currentSpawnRate = spawnRate;

        gridXPoints = new float[grid.xUnits];

        for(int i = 0; i < gridXPoints.Length; i++)
        {
            gridXPoints[i] = grid.gridUnits[i, 0].position.x;
        }
        initialized = true;
    }

    void Update()
    {
        if (initialized)
        {
            currentSpawnRate -= Time.deltaTime;
            if (currentSpawnRate <= 0)
            {
                CreateObjects();
                currentSpawnRate = spawnRate;
            }
        }
    }

    void CreateObjects()
    {
        float chance = Random.value;
        int position = Random.Range(0, grid.xUnits);
        float distance = grid.transform.position.z + spawnDistance;
        if(spawnFrequency >= chance)
        {
            Vector3 pos = new Vector3(gridXPoints[position], 0, distance);
            Instantiate(objectToSpawn, pos, Quaternion.identity);
        }
    }
}
