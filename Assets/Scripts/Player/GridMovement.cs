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
    public Vector2 startGridPosition;
    public AnimationCurve speedCurve;
    public AnimationCurve timeCurve;
    public float minTimeToMove;
    public float maxTimeToMove;

    //Lerp control
    private Vector3 lerpStartPostion;
    private bool moving;
    private float currentMoveTime;

    private float timeToNextSpace;

    //positioning control
    private Vector2 currentGridPosition;
    private Vector2 destinationGridPosition;

    private Vector3 currentTargetPosition;

    private Grid targetGrid;
    private bool initialized;

    private const float DESTINATIONUPDATEPERIOD = .15f;
    private const float SMOOTHING = .1f;
    private const float STATRANGE = .075f;
    private float currentDestinationUpdate;

    //input control
    private PlayerActions input;
    private bool directionHeld;

    private SkeletonAnimation anim;
    private TrackEntry lean;

    //components
    private Stats stats;

    private bool invul;
    private float invulTime;

    public void Init(Stats statsComponent, Grid grid)
    {
        currentDestinationUpdate = DESTINATIONUPDATEPERIOD;
        stats = statsComponent;
        targetGrid = grid;
        currentGridPosition = startGridPosition;
        invulTime = stats.invulTime;
        EventManager.CollisionReaction += Invulnerable;
        if (targetGrid)
        {
            currentTargetPosition = GridToWorldPoisiton(targetGrid, startGridPosition);
            transform.localPosition = currentTargetPosition;
            initialized = true;
        }
        else
        {
            Debug.LogError("No Grid Found, Was It Initialized?");
        }
    }

    Vector3 GridToWorldPoisiton(Grid grid, Vector2 position)
    {
        int newX = Mathf.RoundToInt(position.x);
        int newY = Mathf.RoundToInt(position.y);

        if (targetGrid.CheckIfValidUnit(new Vector2(newX, newY)))
        {
            return grid.gridUnits[newX, newY].position;
        }
        else
        {
            return transform.localPosition;
        }
    }

    float EvaluateTime(float stat)
    {
        float adjStat = stat / 100;
        return timeCurve.Evaluate(adjStat);
    }
    float EvaluateStat(float stat)
    {
        return STATRANGE * Mathf.Abs(1 - (stat / 100));
    }

    public void MovementListener(Vector2 axis)
    {
        if (initialized && !moving)
        {
            float x = axis.x;
            float y = axis.y;
            currentDestinationUpdate -= Time.deltaTime;
            if (currentDestinationUpdate <= 0)
            {
                if (x > .2f)
                {
                    currentDestinationUpdate = DESTINATIONUPDATEPERIOD + EvaluateStat(stats.agility);
                    destinationGridPosition.x = currentGridPosition.x + 1;
                    destinationGridPosition.y = currentGridPosition.y;
                }
                else if (x < -.2f)
                {
                    currentDestinationUpdate = DESTINATIONUPDATEPERIOD + EvaluateStat(stats.agility);
                    destinationGridPosition.x = currentGridPosition.x - 1;
                    destinationGridPosition.y = currentGridPosition.y;
                }
                else if (y > .2f)
                {
                    currentDestinationUpdate = DESTINATIONUPDATEPERIOD + EvaluateStat(stats.speed);
                    destinationGridPosition.x = currentGridPosition.x;
                    destinationGridPosition.y = currentGridPosition.y + 1;
                }
                else if (y < -.2f)
                {
                    currentDestinationUpdate = DESTINATIONUPDATEPERIOD + EvaluateStat(stats.speed);
                    destinationGridPosition.x = currentGridPosition.x;
                    destinationGridPosition.y = currentGridPosition.y - 1;
                }
            }

            if (targetGrid.CheckIfValidUnit(destinationGridPosition))
            {
                currentGridPosition = destinationGridPosition;
            }

            Vector3 newPos = GridToWorldPoisiton(targetGrid, currentGridPosition);

            currentTargetPosition.x = Mathf.Lerp(currentTargetPosition.x, newPos.x, SMOOTHING - EvaluateStat(stats.agility));
            currentTargetPosition.y = Mathf.Lerp(currentTargetPosition.y, newPos.y, SMOOTHING - EvaluateStat(stats.speed));

            transform.localPosition = Vector3.Lerp(transform.localPosition, currentTargetPosition, SMOOTHING);

            /*if (stats)
            {
                float x = axis.x;
                float y = axis.y;

                if (!moving)
                {
                    Vector2 newPosition = new Vector2();
                    if (x > .2f)
                    {
                        newPosition.x = currentGridPosition.x + 1;
                        newPosition.y = currentGridPosition.y;
                        StartCoroutine(Move(EvaluateTime(stats.agility), newPosition));
                    }
                    else if (x < -.2f)
                    {
                        newPosition.x = currentGridPosition.x - 1;
                        newPosition.y = currentGridPosition.y;
                        StartCoroutine(Move(EvaluateTime(stats.agility), newPosition));

                    }
                    else if (y > .2f)
                    {
                        newPosition.x = currentGridPosition.x;
                        newPosition.y = currentGridPosition.y + 1;
                        StartCoroutine(Move(EvaluateTime(stats.speed), newPosition));
                    }
                    else if (y < -.2f)
                    {
                        newPosition.x = currentGridPosition.x;
                        newPosition.y = currentGridPosition.y - 1;
                        StartCoroutine(Move(EvaluateTime(stats.speed), newPosition));
                    }
                }
            }
            else
            {
                Debug.LogError("No Stats Component Attached");
            }*/
        }

    }

    IEnumerator Move(float time)
    {
        moving = true;
        lerpStartPostion = transform.localPosition;

        Vector3 newPosition = GridToWorldPoisiton(targetGrid, currentGridPosition);

        for (float i = 0; i <= time; i += Time.deltaTime)
        {
            float moveTime = i / time;
            transform.localPosition = Vector3.Lerp(lerpStartPostion, newPosition, moveTime);
            yield return null;
        }
        destinationGridPosition = currentGridPosition; 
        currentTargetPosition = transform.localPosition;
        moving = false;
    }
    /*IEnumerator Move(float time, Vector2 toPosition)
    {
        if (targetGrid.CheckIfValidUnit(toPosition))
        {
            moving = true;
            lerpStartPostion = transform.localPosition;
            currentGridPosition = toPosition;

            Vector3 newPosition = GridToWorldPoisiton(targetGrid, currentGridPosition);

            for (float i = 0; i <= time; i += Time.deltaTime)
            {
                float moveTime = i / time;
                transform.localPosition = Vector3.Lerp(lerpStartPostion, newPosition, speedCurve.Evaluate(moveTime));
                yield return null;
            }
            currentTargetPosition = transform.localPosition;
            moving = false;
        }
        else
        {
            moving = true;
            lerpStartPostion = transform.localPosition;

            Vector3 newPosition = GridToWorldPoisiton(targetGrid, currentGridPosition);

            float deltaX = toPosition.x - currentGridPosition.x;
            float deltaY = toPosition.y - currentGridPosition.y;

            newPosition.x += (targetGrid.unitSize * 0.65f) * deltaX;
            newPosition.y += (targetGrid.unitSize * 0.65f) * deltaY;

            for (float i = 0; i <= time; i += Time.deltaTime)
            {
                float moveTime = i / time;
                transform.localPosition = Vector3.Lerp(lerpStartPostion, newPosition, speedCurve.Evaluate(moveTime));
                yield return null;
            }

            for (float i = 0; i <= time; i += Time.deltaTime)
            {
                float moveTime = i / time;
                transform.localPosition = Vector3.Lerp(newPosition, lerpStartPostion, speedCurve.Evaluate(moveTime));
                yield return null;
            }

            moving = false;
        }
    }*/

    void Invulnerable()
    {
        StartCoroutine(InvulnerableRoutine());
    }
    IEnumerator InvulnerableRoutine()
    {
        invul = true;

        for (float i = 0; i <= invulTime; i += Time.deltaTime)
        {
            GetComponent<MeshRenderer>().enabled = !GetComponent<MeshRenderer>().enabled;
            yield return null;
        }
        GetComponent<MeshRenderer>().enabled = true;
        invul = false;
    }

    void OnTriggerEnter(Collider hit)
    {
        if (!invul)
        {
            EnvironmentalHazard isEnviro = hit.GetComponent<EnvironmentalHazard>();
            if (isEnviro && !moving)
            {
                Debug.Log("isHit");

                Vector3 newPosition = currentGridPosition;
                float chance = Random.value;

                if (chance > .5)
                {
                    newPosition.x = currentGridPosition.x + 1;
                    if (!targetGrid.CheckIfValidUnit(newPosition))
                    {
                        newPosition.x = currentGridPosition.x - 1;
                    }
                }
                else
                {
                    newPosition.x = currentGridPosition.x - 1;
                    if (!targetGrid.CheckIfValidUnit(newPosition))
                    {
                        newPosition.x = currentGridPosition.x + 1;
                    }
                }
                currentGridPosition = newPosition;

                StartCoroutine(Move(.1f));
            }
        }
    }


}
