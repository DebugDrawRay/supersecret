using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour
{
    [Header("Grid Properties")]
    public float unitSize;
    public int xUnits;
    public int yUnits;

    public GameObject point;

    public Unit[,] gridUnits
    {
        get;
        private set;
    }

    public static Grid instance;

    public class Unit
    {
        public Vector3 position;
        public Unit(Vector3 newPosition, Transform parent)
        {
            position = newPosition;
        }
    }

    void Awake()
    {
        Init();
        BuildGrid();
    }

    void Init()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;
    }

    void BuildGrid()
    {
        float totalUnits = xUnits * yUnits;
        gridUnits = new Unit[xUnits, yUnits];

        float xStart = transform.localPosition.x - ((xUnits / 2) * unitSize);
        float yStart = transform.localPosition.y - ((yUnits / 2) * unitSize);
        float zStart = transform.localPosition.z;

        for (int i = 0; i < totalUnits; i++)
        {
            int x = i % xUnits;
            int y = (i - x) / xUnits;

            float xPos = (xStart + (unitSize * x));
            float yPos = (yStart + (unitSize * y));

            Vector3 unitPos = new Vector3(xPos, yPos, 0);
            gridUnits[x,y] = new Unit(unitPos, transform);

            GameObject newPoint = Instantiate(point) as GameObject;
            newPoint.transform.SetParent(transform);

            newPoint.transform.localPosition = gridUnits[x,y].position;
            newPoint.transform.rotation = transform.rotation;
        }
    }
}