using UnityEngine;
using InControl;
using Spine;

public class GridMovement : MonoBehaviour
{
    [Header("Movement Properties")]
    public Vector2 startPosition;
    public AnimationCurve speedCurve;
    public AnimationCurve timeCurve;
    public float minTimeToMove;
    public float maxTimeToMove;

    //Lerp control
    private Vector3 lerpStartPostion;
    private bool moving;
    private float currentMoveTime;

    private float timeToNextSpace;

    private Vector2 currentPosition;

    private Grid targetGrid;
    private bool initialized;

    //input control
    private PlayerActions input;
    private bool directionHeld;

    private SkeletonAnimation anim;
    private TrackEntry lean;

    //components
    private Stats stats;

    void Start()
    {
        stats = GetComponent<Stats>();
        targetGrid = Grid.instance;
        currentPosition = startPosition;

        if (targetGrid)
        {
            transform.position = GridPostion(targetGrid, currentPosition.x, currentPosition.y);
            initialized = true;
        }
        else
        {
            Debug.LogError("No Grid Found, Was It Initialized?");
        }
    }

    Vector3 GridPostion(Grid grid, float xPos, float yPos)
    {
        int newX = Mathf.RoundToInt(xPos);
        int newY = Mathf.RoundToInt(yPos);

        return grid.gridUnits[newX, newY].position;
    }

    float EvaluateTime(float stat)
    {
        float adjStat = stat / 100;
        return timeCurve.Evaluate(adjStat);
    }

    public void MovementListener(Vector2 axis)
    {
        if (stats)
        {
            float x = axis.x;
            float y = axis.y;

            if (!moving)
            {
                lerpStartPostion = transform.localPosition;
                if (x > .2f)
                {
                    currentPosition.x += 1;
                    directionHeld = true;
                    moving = true;
                    timeToNextSpace = EvaluateTime(stats.agility);
                }
                if (x < -.2f)
                {
                    currentPosition.x -= 1;
                    directionHeld = true;
                    moving = true;
                    timeToNextSpace = EvaluateTime(stats.agility);
                }
                if (y > .2f)
                {
                    currentPosition.y += 1;
                    directionHeld = true;
                    moving = true;
                    timeToNextSpace = EvaluateTime(stats.speed);
                }
                if (y < -.2f)
                {
                    currentPosition.y -= 1;
                    directionHeld = true;
                    moving = true;
                    timeToNextSpace = EvaluateTime(stats.speed);
                }
            }
            else
            {
                if (x < .2f && x > -.2f && y < .2f && y > -.2f)
                {
                    directionHeld = false;
                }
            }

            if (currentPosition.x < 0)
            {
                currentPosition.x = 0;
            }
            if (currentPosition.x >= targetGrid.xUnits)
            {
                currentPosition.x = targetGrid.xUnits - 1;
            }
            if (currentPosition.y < 0)
            {
                currentPosition.y = 0;
            }
            if (currentPosition.y >= targetGrid.yUnits)
            {
                currentPosition.y = targetGrid.yUnits - 1;
            }

            Vector3 newPosition = GridPostion(targetGrid, currentPosition.x, currentPosition.y);
            if (moving == true)
            {
                currentMoveTime += Time.deltaTime;
                float moveTime = currentMoveTime / timeToNextSpace;
                transform.localPosition = Vector3.Lerp(lerpStartPostion, newPosition, speedCurve.Evaluate(moveTime));

                if(moveTime >= 1f)
                {
                    currentMoveTime = 0;
                    moving = false;
                }
            }
        }
        else
        {
            Debug.LogError("No Stats Component Attached");
        }

    }

}
