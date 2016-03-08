using UnityEngine;
using System.Collections;

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
    private GameObject[] wallProps;
    private GameObject[] groundProps;
    private GameObject[] obstacles;
    private GameObject[] enemies;

    void Start()
    {
        Init();
        BuildLevel(2000, 15, Grid.instance);
    }
    void Init()
    {
        biomeSet = availableBiomes[selectBiomeSet];

        roadMat = biomeSet.roadMaterial;
        wallMat = biomeSet.wallMaterial;
        wallProps = biomeSet.wallProps;
        groundProps = biomeSet.groundProps;
        obstacles = biomeSet.obstacles;
        enemies = biomeSet.enemies;
    }

    void BuildLevel(float levelLength, float roadWidth, Grid targetGrid)
    {
        BuildRoad(levelLength, roadWidth);
        BuildWalls(levelLength, roadWidth);
        SpawnWallProps();
        SpawnGroundProps();
        SpawnObstacles();
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
    void SpawnWallProps()
    {

    }
    void SpawnGroundProps()
    {

    }
    void SpawnObstacles()
    {

    }
    void SpawnEnemies()
    {

    }
}
