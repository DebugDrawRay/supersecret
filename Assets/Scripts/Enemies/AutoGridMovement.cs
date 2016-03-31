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
    private bool isForcedMoving;

    //Distance tracking
    private bool startPositionSet;
    public Vector3 distanceTrackStart
    {
        get;
        private set;
    }
    private float distanceTraveled;

    public void Init(Grid grid, Stats status)
    {
        targetGrid = grid;
        stats = status;
        initialized = true;

        speed = baseSpeed + (speedRange - (speedRange * stats.collection.speed));
        agility = baseAgility + (agilityRange - (agilityRange * stats.collection.agility));
    }

    void Update()
    {
        if (isMoving)
        {
            if (!startPositionSet)
            {
                distanceTrackStart = transform.localPosition;
                startPositionSet = true;
            }
            distanceTraveled = Vector3.Distance(transform.localPosition, distanceTrackStart);
        }
        else
        {
            distanceTraveled = 0;
            startPositionSet = false;
        }
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

    public void ForcedMove(Vector3 from)
    {
        if (!isForcedMoving)
        {
            Vector3 direction = from - transform.localPosition;
            direction = direction.normalized;
            direction.x = Mathf.Sign(direction.x) * Mathf.Ceil(Mathf.Abs(direction.x));
            direction.y = Mathf.Sign(direction.y) * Mathf.Ceil(Mathf.Abs(direction.y));

            float xDir = 0;
            float yDir = 0;

            if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
            {
                xDir = -direction.x;
                yDir = 0;

                float newX = currentPoint.x + xDir;
                if (newX < 0 || newX >= targetGrid.xUnits)
                {
                    xDir = 0;
                    yDir = -direction.x;
                    float newY = currentPoint.y + yDir;
                    if (newY < 0 || newY >= targetGrid.yUnits)
                    {
                        yDir = -yDir;
                    }
                }
            }
            else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
            {

                yDir = -direction.y;
                xDir = 0;

                float newY = currentPoint.y + yDir;
                if (newY < 0 || newY >= targetGrid.yUnits)
                {
                    yDir = 0;
                    xDir = -direction.y;
                    float newX = currentPoint.x + xDir;
                    if (newX < 0 || newX >= targetGrid.xUnits)
                    {
                        xDir = -xDir;
                    }
                }
            }

            Vector2 newDirection = new Vector2(xDir, yDir);
            StartCoroutine(ForcedMove(.25f, newDirection));
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

                enterPoint = new Vector2(currentPoint.x + xPos, currentPoint.y + yPos);
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
            currentPoint = path[i];
            for (float t = 0; t <= time; t += Time.deltaTime)
            {
                float moveTime = t / time;
                transform.localPosition = Vector3.Lerp(startPosition, nextPosition, movementSmoothing.Evaluate(moveTime));
                if (isForcedMoving)
                {
                    break;
                }
                else
                {
                    yield return null;
                }
            }
        }
        isMoving = false;
    }

    IEnumerator MoveToPoint(float time, Vector2 point)
    {
        isMoving = true;
        Vector3 startPosition = transform.localPosition;
        Vector3 nextPosition = targetGrid.GridToWorldPosition(point);
        currentPoint = point;
        for (float t = 0; t <= time; t += Time.deltaTime)
        {
            float moveTime = t / time;
            transform.localPosition = Vector3.Lerp(startPosition, nextPosition, movementSmoothing.Evaluate(moveTime));
            if (isForcedMoving)
            {
                break;
            }
            else
            {
                yield return null;
            }
        }
        isMoving = false;
    }

    IEnumerator ForcedMove(float time, Vector2 direction)
    {
        if (targetGrid.CheckIfValidUnit(direction + currentPoint))
        {
            currentPoint = direction + currentPoint;

            isForcedMoving = true;
            isMoving = true;
            Vector3 startPosition = transform.localPosition;
            Vector3 nextPosition = targetGrid.GridToWorldPosition(currentPoint);

            for (float t = 0; t <= time; t += Time.deltaTime)
            {
                float moveTime = t / time;
                transform.localPosition = Vector3.Lerp(startPosition, nextPosition, movementSmoothing.Evaluate(moveTime));
                yield return null;
            }
            isMoving = false;
            isForcedMoving = false;
        }
    }
}
