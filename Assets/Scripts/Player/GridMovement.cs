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
    public Vector2 currentGridPosition;
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

    //components
    private Stats stats;
    private PlayerAnimationController animation;

    private bool initialized;

    public void Init(Stats statsComponent, Grid grid, PlayerAnimationController animationComponent)
    {
        
        PlayerEventManager.CollisionReaction += CollisionMove;

        currentDestinationUpdate = destinationUpdatePeriod;
        stats = statsComponent;
        animation = animationComponent;
        targetGrid = grid;
        currentGridPosition = startGridPosition;
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

        if (isMoving && !isHit)
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
                destinationGridPosition.x = currentGridPosition.x + x;
                destinationGridPosition.y = currentGridPosition.y + y;

                if (targetGrid.CheckIfValidUnit(destinationGridPosition))
                {
                    currentGridPosition = destinationGridPosition;

                    startTime = Time.time;
                    lerpStartPostion = transform.localPosition;
                    targetLocalPosition = targetGrid.GridToWorldPosition(destinationGridPosition);

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
        direction = direction.normalized;
        direction.x = Mathf.Sign(direction.x) * Mathf.Ceil(Mathf.Abs(direction.x));
        direction.y = Mathf.Sign(direction.y) * Mathf.Ceil(Mathf.Abs(direction.y));

        float xDir = 0;
        float yDir = 0;

        if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
        {
            xDir = -direction.x;
            yDir = 0;

            float newX = targetGrid.GetClosestUnit(transform.localPosition).x + xDir;
            if (newX < 0 || newX >= targetGrid.xUnits)
            {
                xDir = 0;
                yDir = -direction.x;
                float newY = targetGrid.GetClosestUnit(transform.localPosition).y + yDir;
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

            float newY = targetGrid.GetClosestUnit(transform.localPosition).y + yDir;
            if (newY < 0 || newY >= targetGrid.yUnits)
            {
                yDir = 0;
                xDir = -direction.y;
                float newX = targetGrid.GetClosestUnit(transform.localPosition).x + xDir;
                if (newX < 0 || newX >= targetGrid.xUnits)
                {
                    xDir = -xDir;
                }
            }
        }
        Vector2 newDirection = new Vector2(xDir, yDir);
        StartCoroutine(ForceMove(.1f, newDirection));
    }

    IEnumerator ForceMove(float time, Vector2 direction)
    {
        isHit = true;
        lerpStartPostion = transform.localPosition;

        Vector2 newPoint = targetGrid.GetClosestUnit(transform.localPosition);
        Debug.Log(newPoint);
        Vector3 newPosition = targetGrid.GridToWorldPosition(newPoint);

        for (float i = 0; i <= time; i += Time.deltaTime)
        {
            float moveTime = i / time;
            transform.localPosition = Vector3.Lerp(lerpStartPostion, newPosition, moveTime);
            yield return null;
        }
        currentGridPosition = newPoint;
        targetLocalPosition = newPosition;
        isHit = false;
    }
        
}
