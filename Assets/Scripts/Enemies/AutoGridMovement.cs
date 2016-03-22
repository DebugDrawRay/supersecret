using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoGridMovement : MonoBehaviour
{
    [Header("Base Movement Properties")]
    public float baseSpeed = .5f;
    public float baseAgility = .5f;
    public float speedRange = .5f;
    public float agilityRange = .5f;

    public AnimationCurve movementSmoothing;
    private float speed;
    private float agility;
    //Grid properties
    private Vector2 currentPoint;

    private Grid targetGrid;
    private Stats stats;
    private bool initialized;

    private bool isMoving;

    public void Init(Grid grid, Stats stat)
    {
        targetGrid = grid;
        stats = stat;
        initialized = true;

        speed = baseSpeed + (speedRange - (speedRange * stats.speed));
        agility = baseAgility + (agilityRange - (agilityRange * stats.agility));
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
            int ranX = Random.Range(0, targetGrid.xUnits);
            int ranY = Random.Range(0, targetGrid.yUnits);

            Vector2 goal = new Vector2(ranX, ranY);
            List<Vector2> path = Pathfinder.FindPath(currentPoint, goal, targetGrid.gridSize);
            StartCoroutine(MoveAcrossPath(path, speed));
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
                yPos = 0;

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
                yPos = 0;

                if (yPos == 0)
                {
                    xPos = Random.Range(0, targetGrid.xUnits);
                }

                enterPoint = new Vector2(xPos, yPos);
            }
            currentPoint = enterPoint;
            StartCoroutine(MoveToPoint(3, enterPoint));
        }
    }

    IEnumerator MoveAcrossPath(List<Vector2> path, float time)
    {
        isMoving = true;
        for(int i = 1; i < path.Count; ++i)
        {
            Vector3 startPosition = transform.localPosition;
            Vector3 nextPosition = targetGrid.GridToWorldPosition(path[i]);
            for (float t = 0; t <= time; t += Time.deltaTime)
            {
                float moveTime = t / time;
                transform.localPosition = Vector3.Lerp(startPosition, nextPosition, movementSmoothing.Evaluate(moveTime));
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
        Vector3 nextPosition = targetGrid.GridToWorldPosition(point);
        for (float t = 0; t <= time; t += Time.deltaTime)
        {
            float moveTime = t / time;
            transform.localPosition = Vector3.Lerp(startPosition, nextPosition, movementSmoothing.Evaluate(moveTime));
            yield return null;
        }
        currentPoint = point;
        isMoving = false;

    }
}
