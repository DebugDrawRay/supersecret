using UnityEngine;

public class Grid : MonoBehaviour
{
    [Header("Grid Properties")]
    public float unitSize;
    public int xUnits;
    public int yUnits;

    public Vector2 gridSize
    {
        get;
        set;
    }

    public GameObject point;

    public Unit[,] gridUnits
    {
        get;
        private set;
    }

    private GameObject[] units;

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
        gridSize = new Vector2(xUnits, yUnits);
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

    public bool CheckIfValidUnit(Vector2 unit)
    {
        if (unit.x < 0 || 
            unit.x >= xUnits || 
            unit.y < 0 || 
            unit.y >= yUnits)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    void BuildGrid()
    {
        float totalUnits = xUnits * yUnits;
        gridUnits = new Unit[xUnits, yUnits];
        units = new GameObject[xUnits * yUnits];

        float xStart = transform.localPosition.x - ((xUnits / 2) * unitSize);
        float yStart = transform.localPosition.y - ((yUnits / 2) * unitSize);
        float zStart = transform.localPosition.z;

        for (int i = 0; i < totalUnits; ++i)
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

            units[i] = newPoint;
        }

       /* CombineInstance[] combine = new CombineInstance[units.Length];

        for (int i = 0; i < units.Length; ++i)
        {
            Debug.Log(i);
            MeshFilter mesh = units[i].GetComponentInChildren<MeshFilter>();
            combine[i].mesh = mesh.sharedMesh;
            units[i].SetActive(false);
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);*/
    }

    public Vector3 GridToWorldPoisiton(Vector2 position)
    {
        int newX = Mathf.RoundToInt(position.x);
        int newY = Mathf.RoundToInt(position.y);

        if (CheckIfValidUnit(new Vector2(newX, newY)))
        {
            return gridUnits[newX, newY].position;
        }
        else
        {
            return transform.localPosition;
        }
    }
}