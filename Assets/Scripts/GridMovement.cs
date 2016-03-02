using UnityEngine;
using InControl;
using Spine;
using System.Collections;
public class GridMovement : MonoBehaviour
{
    private enum State
    {
        Inactive,
        InControl,
        InForcedMove
    }

    private State currentState = State.InControl;

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
                if (x > .2f)
                {
                    currentPosition.x += 1;
                    StartCoroutine(Move(EvaluateTime(stats.agility), currentPosition));
                }
                if (x < -.2f)
                {
                    currentPosition.x -= 1;
                    StartCoroutine(Move(EvaluateTime(stats.agility), currentPosition));

                }
                if (y > .2f)
                {
                    currentPosition.y += 1;
                    StartCoroutine(Move(EvaluateTime(stats.speed), currentPosition));
                }
                if (y < -.2f)
                {
                    currentPosition.y -= 1;
                    StartCoroutine(Move(EvaluateTime(stats.speed), currentPosition));
                }
            }
        }
        else
        {
            Debug.LogError("No Stats Component Attached");
        }

    }

    IEnumerator Move(float time, Vector2 toPosition)
    {
        moving = true;
        lerpStartPostion = transform.localPosition;
        currentPosition = toPosition;

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

        for (float i = 0; i <= time; i += Time.deltaTime)
        {
            float moveTime = i / time;
            transform.localPosition = Vector3.Lerp(lerpStartPostion, newPosition, speedCurve.Evaluate(moveTime));
            yield return null;
        }
        moving = false;
    }

    void OnTriggerEnter(Collider hit)
    {
        EnvironmentalHazard isEnviro = hit.GetComponent<EnvironmentalHazard>();
        if(isEnviro)
        {
            float chance = Random.value;
            if(chance >.5)
            {
                currentPosition.x += 1;
            }
            else
            {
                currentPosition.x -= 1;
            }
            StartCoroutine(Move(.1f, currentPosition));
        }
        
    }


}
