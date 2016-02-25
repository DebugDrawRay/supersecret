using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour
{
    [Header("Grid Properties")]
    public float unitSize;
    public float xUnits;
    public float yUnits;

    public GameObject point;

    public Unit[] gridUnits
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
    }

    void BuildGrid()
    {
        float totalUnits = xUnits * yUnits;
        gridUnits = new Unit[Mathf.RoundToInt(totalUnits)];

        float xStart = transform.localPosition.x - (((xUnits / 2) - 0.5f ) * unitSize);
        float yStart = transform.localPosition.y - (((yUnits / 2) - 0.5f) * unitSize);
        float zStart = transform.localPosition.z;
        for (int i = 0; i < totalUnits; i++)
        {
            float x = i % xUnits;
            float y = (i - x) / xUnits;

            float xPos = (xStart + (unitSize * x));
            float yPos = (yStart + (unitSize * y));

            Vector3 unitPos = new Vector3(xPos, yPos, zStart);
            gridUnits[i] = new Unit(unitPos, transform);

            GameObject newPoint = Instantiate(point) as GameObject;
            newPoint.transform.SetParent(transform);

            newPoint.transform.position = gridUnits[i].position;
            newPoint.transform.rotation = transform.rotation;
        }
    }

    void Update()
    {
        Debug.Log(gridUnits[0].position);
    }
}