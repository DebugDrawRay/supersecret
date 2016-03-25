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
    public float minTimeToMove;
    public float maxTimeToMove;

    public float destinationUpdatePeriod;
    private float currentDestinationUpdate;

    //Lerp control
    private Vector3 lerpStartPostion;
    private float startTime;

    private float xTargetTime;
    private float yTargetTime;

    private float moveTime;
    private float xPercentageComplete;
    private float yPercentageComplete;

    //Status
    private bool isMoving;
    private bool isHit;

    //positioning control
    public Vector2 targetGridPosition
    {
        get;
        private set;
    }
    private Vector3 targetLocalPosition;

    //Distance tracking
    private bool startPositionSet;
    private Vector3 distanceTrackStart;
    public float distanceTraveled
    {
        get;
        private set;
    }

    //References
    private Grid targetGrid;

    private float destinationDistance;
    private Vector3 destinationDirection;

    //components
    private Stats stats;
    private PlayerAnimationController animation;

    private bool initialized;

    public void Init(Stats statsComponent, Grid grid, PlayerAnimationController animationComponent)
    {
        currentDestinationUpdate = destinationUpdatePeriod;
        stats = statsComponent;
        animation = animationComponent;
        targetGrid = grid;

        targetGridPosition = startGridPosition;
        if (targetGrid)
        {
            transform.localPosition = targetGrid.GridToWorldPosition(startGridPosition);
            initialized = true;
        }
        else
        {
            Debug.LogError("No Grid Found, Was It Initialized?");
        }
    }

    void Update()
    {
        if (currentDestinationUpdate > 0)
        {
            currentDestinationUpdate -= Time.deltaTime;
        }

        if (isMoving)
        {
            if(!startPositionSet)
            {
                distanceTrackStart = transform.localPosition;
                startPositionSet = true;
            }
            distanceTraveled = Vector3.Distance(transform.localPosition, distanceTrackStart);
            MoveAction();
        }
        else
        {
            distanceTraveled = 0;
            startPositionSet = false;
        }
        stats.distanceTraveled = distanceTraveled;
    }

    float MoveTime(float stat)
    {
        float deltaTime = maxTimeToMove - minTimeToMove;
        float rawTime = minTimeToMove + (deltaTime * (1 - stat));
        return rawTime;
    }

    public void Move(float x, float y)
    {
        if (initialized && !isHit)
        {
            if (currentDestinationUpdate <= 0)
            {
                Vector2 destinationGridPosition = Vector2.zero;
                destinationGridPosition.x = targetGridPosition.x + x;
                destinationGridPosition.y = targetGridPosition.y + y;

                if (targetGrid.CheckIfValidUnit(destinationGridPosition))
                {
                    targetGridPosition = destinationGridPosition;

                    startTime = Time.time;
                    lerpStartPostion = transform.localPosition;
                    targetLocalPosition = targetGrid.GridToWorldPosition(targetGridPosition);

                    moveTime = 0;
                    xPercentageComplete = 0;
                    yPercentageComplete = 0;

                    isMoving = true;
                    currentDestinationUpdate = destinationUpdatePeriod;
                }
            }

        }
    }

    void MoveAction()
    {
        Vector3 direction = targetLocalPosition - transform.localPosition;
        animation.AnimateLean(direction);

        moveTime = Time.time - startTime;

        xPercentageComplete = moveTime / MoveTime(stats.agility);
        yPercentageComplete = moveTime / MoveTime(stats.speed);

        Vector3 newPosition = transform.localPosition;
        newPosition.x = Mathf.Lerp(lerpStartPostion.x, targetLocalPosition.x, speedCurve.Evaluate(xPercentageComplete));
        newPosition.y = Mathf.Lerp(lerpStartPostion.y, targetLocalPosition.y, speedCurve.Evaluate(yPercentageComplete));
        transform.localPosition = newPosition;

        if (xPercentageComplete >= 1 && yPercentageComplete >= 1)
        {
            isMoving = false;
        }
    }

    public void CollisionMove(Vector3 from)
    {
        Vector3 direction = from - transform.localPosition;
        float xDir = -direction.normalized.x;
        float yDir = -direction.normalized.y;

        if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
        {
            xDir = -direction.normalized.x;
            yDir = 0;

            float newX = targetGridPosition.x + xDir;
            if (newX < 0 || newX >= targetGrid.xUnits)
            {
                yDir = -direction.normalized.x;
                float newY = targetGridPosition.y + yDir;
                if (newY < 0 || newY >= targetGrid.yUnits)
                {
                    yDir = -yDir;
                }
            }
        }
        else
        {
            yDir = -direction.normalized.y;
            xDir = 0;

            float newY = targetGridPosition.y + yDir;
            if (newY < 0 || newY >= targetGrid.yUnits)
            {
                xDir = -direction.normalized.y;
                float newX = targetGridPosition.x + xDir;
                if (newX < 0 || newX >= targetGrid.xUnits)
                {
                    xDir = -xDir;
                }
            }
        }

        StartCoroutine(ForceMove(xDir, yDir, .25f));
    }

    IEnumerator ForceMove(float xDirection, float zDirection, float time)
    {
        isHit = true;
        lerpStartPostion = transform.localPosition;

        Vector2 newPoint = targetGridPosition;
        newPoint.x += Mathf.Sign(xDirection) * Mathf.Abs(Mathf.Ceil(xDirection));
        newPoint.y += Mathf.Sign(zDirection) * Mathf.Abs(Mathf.Ceil(zDirection));
        Vector3 newPosition = targetGrid.GridToWorldPosition(newPoint);

        for (float i = 0; i <= time; i += Time.deltaTime)
        {
            float moveTime = i / time;
            transform.localPosition = Vector3.Lerp(lerpStartPostion, newPosition, moveTime);
            yield return null;
        }
        targetGridPosition = newPoint;
        targetLocalPosition = newPosition;
        isHit = false;
    }
        
}
