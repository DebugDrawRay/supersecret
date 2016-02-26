using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour
{
    [Header("Grid Properties")]
    public float unitSize;
    public int xUnits;
    public int yUnits;

    public GameObject point;
    public GameObject player;

    public Unit[,] gridUnits
    {
        get;
        private set;
    }

    public class Unit
    {
        public Vector3 position;
        public Unit(Vector3 newPosition, Transform parent)
        {
            position = parent.transform.TransformPoint(newPosition);
        }
    }

    void Start()
    {
        BuildGrid();
        SetupPlayer();
    }

    void BuildGrid()
    {
        float totalUnits = xUnits * yUnits;
        gridUnits = new Unit[xUnits, yUnits];

        float xStart = transform.localPosition.x - (((xUnits / 2) - 0.5f ) * unitSize);
        float yStart = transform.localPosition.y - (((yUnits / 2) - 0.5f) * unitSize);
        float zStart = transform.localPosition.z;
        for (int i = 0; i < totalUnits; i++)
        {
            int x = i % xUnits;
            int y = (i - x) / xUnits;

            float xPos = (xStart + (unitSize * x));
            float yPos = (yStart + (unitSize * y));

            Vector3 unitPos = new Vector3(xPos, yPos, zStart);
            gridUnits[x,y] = new Unit(unitPos, transform);

            GameObject newPoint = Instantiate(point) as GameObject;
            newPoint.transform.SetParent(transform);

            newPoint.transform.position = gridUnits[x,y].position;
            newPoint.transform.rotation = transform.rotation;
        }
    }

    void SetupPlayer()
    {
        GridMovement newPlayer = Instantiate(player).GetComponent<GridMovement>();
        newPlayer.transform.SetParent(transform);
        newPlayer.Init(this, new Vector2(3,3));
    }
}