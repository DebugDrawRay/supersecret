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

    public GameObject gridQuad;

    public Unit[,] gridUnits
    {
        get;
        private set;
    }

    public static Grid instance
    {
        get;
        private set;
    }

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
        }

        GameObject visual = Instantiate(gridQuad);
        Vector2 texScale = visual.GetComponent<MeshRenderer>().material.mainTextureScale;
        texScale.x *= xUnits;
        texScale.y *= yUnits;
        visual.GetComponent<MeshRenderer>().material.mainTextureScale = texScale;

        Vector3 scale = new Vector3(xUnits * unitSize, yUnits * unitSize, 1);

        visual.transform.localScale = scale;
        visual.transform.SetParent(transform);
        visual.transform.localPosition = new Vector3(0,0,-0.1f);



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