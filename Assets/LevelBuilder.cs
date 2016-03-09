using UnityEngine;
using System.Collections.Generic;

public class LevelBuilder : MonoBehaviour
{
    [Header("Biome Setup")]
    public int selectBiomeSet;
    public BiomeSet[] availableBiomes;

    private BiomeSet biomeSet;

    [Header("Level Builder Objects")]
    public GameObject blankPlane;

    //Cached References
    private Material roadMat;
    private Material wallMat;
    private Material groundMat;
    private GameObject[] wallProps;
    private GameObject[] groundProps;
    private GameObject[] obstacles;
    private GameObject[] enemies;

    //Level
    private GameObject level;

    [Header("Level Properties")]
    public float levelLength;
    public float roadWidth;

    [Header("Wall Prop Properties")]
    public float wallPropsSpacing;
    public float wallPropOffset;

    [Header("Obstacle Properties")]
    public int obstacleSpawnBuffer;
    [Range(0, 1)]
    public float obstacleSpawnRate;
    public int maxObstaclesPerRow;

    void Start()
    {
        Init();
        BuildLevel(levelLength, roadWidth, Grid.instance);
    }
    void Init()
    {
        biomeSet = availableBiomes[selectBiomeSet];

        roadMat = biomeSet.roadMaterial;
        wallMat = biomeSet.wallMaterial;
        groundMat = biomeSet.groundMaterial;
        wallProps = biomeSet.wallProps;
        groundProps = biomeSet.groundProps;
        obstacles = biomeSet.obstacles;
        enemies = biomeSet.enemies;
    }

    void BuildLevel(float levelLength, float roadWidth, Grid targetGrid)
    {
        BuildRoad(levelLength, roadWidth);
        BuildGround(levelLength);
        //BuildWalls(levelLength, roadWidth);
        //SpawnWallProps(levelLength, roadWidth);
        SpawnGroundProps();
        SpawnObstacles(levelLength, targetGrid);
        SpawnEnemies();
    }

    void BuildRoad(float length, float width)
    {
        if(roadMat)
        {
            GameObject newRoad = Instantiate(blankPlane);

            Vector3 scale = new Vector3(length, 0, width);
            newRoad.transform.localScale = scale;
            newRoad.transform.position = Vector3.zero;
            newRoad.transform.rotation = Quaternion.Euler(0, 90, 0);

            Material newRoadMat = roadMat;
            Vector2 texScale = new Vector2(scale.x / 20, 1);
            newRoadMat.mainTextureScale = texScale;

            newRoad.GetComponent<MeshRenderer>().material = newRoadMat;

        }
        else
        {
            Debug.LogError("No Road Material Selected");
        }
    }

    void BuildGround(float length)
    {
        GameObject newGround = Instantiate(blankPlane);

        Vector3 scale = new Vector3(length, length, length);
        newGround.transform.localScale = scale;
        newGround.transform.position = new Vector3(0, -0.5f, 0);

        newGround.GetComponent<MeshRenderer>().material = groundMat;
    }
    void BuildWalls(float length, float roadWidth)
    {
        if(wallMat)
        {
            GameObject leftWall = Instantiate(blankPlane);
            GameObject rightWall = Instantiate(blankPlane);

            Vector3 scale = new Vector3(length, 0, 5);

            leftWall.transform.localScale = scale;
            rightWall.transform.localScale = scale;

            leftWall.transform.rotation = Quaternion.Euler(90, 90, 0);
            rightWall.transform.rotation = Quaternion.Euler(90, 270, 0);

            leftWall.transform.position = new Vector3(-roadWidth * 5, 22, 0);
            rightWall.transform.position = new Vector3(roadWidth * 5, 22, 0);
            Material newWallMat = wallMat;
            Vector2 texScale = new Vector2(scale.x / 20, 1);
            newWallMat.mainTextureScale = texScale;

            leftWall.GetComponent<MeshRenderer>().material = newWallMat;
            rightWall.GetComponent<MeshRenderer>().material = newWallMat;

        }
    }
    void SpawnWallProps(float length, float roadWidth)
    {
        int total = Mathf.RoundToInt(levelLength * 5 / wallPropsSpacing);

        for(int i = 0; i <= total; ++i)
        {
            int select = Random.Range(0, wallProps.Length);
            GameObject left = Instantiate(wallProps[select]);
            float xPos = -roadWidth * 5;
            float offset = xPos + Random.Range(-wallPropOffset, wallPropOffset);
            left.transform.position = new Vector3(offset, 20, wallPropsSpacing * i);
            left.transform.rotation = Quaternion.Euler(0, 180, 0);

            select = Random.Range(0, wallProps.Length);
            GameObject right = Instantiate(wallProps[select]);
            xPos = roadWidth * 5;
            offset = xPos + Random.Range(-wallPropOffset, wallPropOffset);
            right.transform.position = new Vector3(offset, 20, wallPropsSpacing * i);

        }
    }
    void SpawnGroundProps()
    {
       

    }
    void SpawnObstacles(float length, Grid grid)
    {
        float[] gridXPoints = new float[grid.xUnits];

        for (int i = 0; i < gridXPoints.Length; i++)
        {
            gridXPoints[i] = grid.gridUnits[i, 0].position.x;
        }

        int totalRows = Mathf.RoundToInt((length * 5) / grid.unitSize);

        for (int i = 0; i < totalRows; ++i)
        {
            float canSpawn = i % obstacleSpawnBuffer;

            if (canSpawn == 0)
            {
                float chance = Random.value;
                if (chance <= obstacleSpawnRate)
                {
                    int amount = Random.Range(1, maxObstaclesPerRow + 1);
                    int[] slots = new int[amount];
                    for (int j = 0; j < slots.Length; ++j)
                    {
                        int pos = Random.Range(1, 6);
                        for (int k = 0; k < slots.Length; ++k)
                        {
                            if (slots[k] == pos)
                            {
                                pos = Random.Range(1, 6);
                                k = -1;
                            }
                        }
                        slots[j] = pos;
                    }
                    for (int l = 0; l < slots.Length; ++l)
                    {
                        GameObject newObs = Instantiate(obstacles[0]);
                        float xPos = gridXPoints[slots[l] - 1];
                        float zPos = grid.unitSize * i;
                        newObs.transform.position = new Vector3(xPos, 2, zPos);
                    }
                }
            }
        }
    }
    void SpawnEnemies()
    {

    }
}
