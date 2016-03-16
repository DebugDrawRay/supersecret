using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoGridMovement : MonoBehaviour
{
    [Header("Movement Properties")]
    public float delay;
    private float currentDelay;
    public float speed;

    //Grid properties
    public Vector2 startPoint;

    private Vector2 currentPoint;
    private Vector3 currentPosition;
    private Vector3 goalPosition;

    private Grid targetGrid;
    private bool initialized;

    private bool isMoving;

    void Awake()
    {
        EventManager.EnterGridEvent += EnterGrid;
    }

    public void Init(Grid grid)
    {
        targetGrid = grid;
        initialized = true;
    }

    public void MoveToDestination(Vector2 goal)
    {
        if (!isMoving)
        {
            List<Vector2> path = Pathfinder.FindPath(currentPoint, goal, targetGrid.gridSize);
            StartCoroutine(MoveAcrossPath(path, speed));
        }
    }

    public void MoveToRandomDestination()
    {
        if (!isMoving)
        {
            currentDelay -= Time.deltaTime;
            if (currentDelay <= 0)
            {
                int ranX = Random.Range(0, targetGrid.xUnits);
                int ranY = Random.Range(0, targetGrid.yUnits);

                Vector2 goal = new Vector2(ranX, ranY);
                List<Vector2> path = Pathfinder.FindPath(currentPoint, goal, targetGrid.gridSize);
                StartCoroutine(MoveAcrossPath(path, speed));
                currentDelay = delay;
            }
        }
    }

    public void EnterGrid()
    {
        if (!isMoving)
        {
            transform.SetParent(targetGrid.transform);
            Vector2 enterPoint = Vector2.zero;
            if (targetGrid.transform.position.x > transform.position.x)
            {
                int yPos = Random.Range(0, targetGrid.yUnits);
                int xPos = 0;

                if (yPos == 0)
                {
                    xPos = Random.Range(0, targetGrid.xUnits);
                }

                enterPoint = new Vector2(xPos, yPos);
            }
            else if (targetGrid.transform.position.x < transform.position.x)
            {
                int yPos = Random.Range(0, targetGrid.yUnits);
                int xPos = targetGrid.xUnits - 1;

                if(yPos == 0)
                {
                    xPos = Random.Range(0, targetGrid.xUnits);
                }

                enterPoint = new Vector2(xPos, yPos);
            }
            StartCoroutine(MoveToPoint(1, enterPoint));
        }
    }

    IEnumerator MoveAcrossPath(List<Vector2> path, float time)
    {
        isMoving = true;
        time = time / path.Count;
        for(int i = 1; i < path.Count; ++i)
        {
            Vector3 startPosition = transform.localPosition;
            Vector3 nextPosition = targetGrid.GridToWorldPoisiton(path[i]);
            for (float t = 0; t <= time; t += Time.deltaTime)
            {
                float moveTime = t / time;
                transform.localPosition = Vector3.Lerp(startPosition, nextPosition, moveTime);
                yield return null;
            }
            currentPoint = path[i];
        }
        isMoving = false;
    }

    IEnumerator MoveToPoint(float time, Vector2 point)
    {
        isMoving = true;
        Vector3 startPosition = transform.localPosition;
        Vector3 nextPosition = targetGrid.GridToWorldPoisiton(point);
        for (float t = 0; t <= time; t += Time.deltaTime)
        {
            float moveTime = t / time;
            transform.localPosition = Vector3.Lerp(startPosition, nextPosition, moveTime);
            yield return null;
        }
        currentPoint = point;
        isMoving = false;

    }
}
