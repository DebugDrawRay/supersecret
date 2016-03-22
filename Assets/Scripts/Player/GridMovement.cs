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
    public Vector2 currentGridPosition
    {
        get;
        private set;
    }
    private Vector2 destinationGridPosition;
    private Vector3 currentTargetPosition;

    private Grid targetGrid;
    private bool initialized;

    private float destinationUpdatePeriod = .15f;
    private float baseMoveTime = .05f;
    private float moveTimeRange = .025f;
    private float currentDestinationUpdate;
    private float destinationDistance;
    private Vector3 destinationDirection;

    //input control
    private PlayerActions input;
    private bool directionHeld;

    private SkeletonAnimation anim;
    private TrackEntry lean;

    //components
    private Stats stats;
    private PlayerAnimationController animation;

    public void Init(Stats statsComponent, Grid grid, PlayerAnimationController animationComponent)
    {
        currentDestinationUpdate = destinationUpdatePeriod;
        stats = statsComponent;
        animation = animationComponent;
        targetGrid = grid;
        currentGridPosition = startGridPosition;
        //PlayerEventManager.CollisionReaction += Invulnerable;
        if (targetGrid)
        {
            transform.localPosition = targetGrid.GridToWorldPoisiton(startGridPosition);
            initialized = true;
        }
        else
        {
            Debug.LogError("No Grid Found, Was It Initialized?");
        }
    }

    float MoveTime(float stat)
    {
        float rawTime = baseMoveTime - (moveTimeRange * (1 - stat));
        return rawTime;
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
                    currentDestinationUpdate = destinationUpdatePeriod;
                    destinationGridPosition.x = currentGridPosition.x + 1;
                    destinationGridPosition.y = currentGridPosition.y;
                    destinationDistance = Vector3.Distance(transform.localPosition, destinationGridPosition);
                    if (targetGrid.CheckIfValidUnit(destinationGridPosition))
                    {
                        currentGridPosition = destinationGridPosition;
                    }
                }
                else if (x < -.2f)
                {
                    currentDestinationUpdate = destinationUpdatePeriod;
                    destinationGridPosition.x = currentGridPosition.x - 1;
                    destinationGridPosition.y = currentGridPosition.y;
                    if (targetGrid.CheckIfValidUnit(destinationGridPosition))
                    {
                        currentGridPosition = destinationGridPosition;
                    }
                }
                else if (y > .2f)
                {
                    currentDestinationUpdate = destinationUpdatePeriod;
                    destinationGridPosition.x = currentGridPosition.x;
                    destinationGridPosition.y = currentGridPosition.y + 1;
                    if (targetGrid.CheckIfValidUnit(destinationGridPosition))
                    {
                        currentGridPosition = destinationGridPosition;
                    }
                }
                else if (y < -.2f)
                {
                    currentDestinationUpdate = destinationUpdatePeriod;
                    destinationGridPosition.x = currentGridPosition.x;
                    destinationGridPosition.y = currentGridPosition.y - 1;
                    if (targetGrid.CheckIfValidUnit(destinationGridPosition))
                    {
                        currentGridPosition = destinationGridPosition;
                    }
                }
            }

            Vector3 newPos = targetGrid.GridToWorldPoisiton(currentGridPosition);

            currentTargetPosition.x = Mathf.Lerp(transform.localPosition.x, newPos.x, MoveTime(stats.agility));
            currentTargetPosition.y = Mathf.Lerp(transform.localPosition.y, newPos.y, MoveTime(stats.speed));

            transform.localPosition = currentTargetPosition;

            Vector3 direction = newPos - transform.localPosition;
            animation.AnimateLean(direction);
        }

    }

    public void CollisionMove(Vector3 from)
    {
        Vector3 direction = from - transform.localPosition;
        float zDir = 1;
        float xDir = -direction.normalized.x;

        if(currentGridPosition.x + Mathf.Sign(xDir) * Mathf.Abs(Mathf.Ceil(xDir)) < 0 ||
           currentGridPosition.x + Mathf.Sign(xDir) * Mathf.Abs(Mathf.Ceil(xDir)) >= targetGrid.xUnits )
        {
            xDir = -xDir;
            if (currentGridPosition.y + Mathf.Sign(zDir) * Mathf.Abs(Mathf.Ceil(zDir)) < 0 ||
                currentGridPosition.y + Mathf.Sign(zDir) * Mathf.Abs(Mathf.Ceil(zDir)) >= targetGrid.yUnits)
            {
                zDir = -zDir;
            }
        }
        else
        {
            zDir = 0;
        }

        StartCoroutine(ForceMove(xDir, zDir, .25f));
    }

    IEnumerator ForceMove(float xDirection, float zDirection, float time)
    {
        moving = true;
        lerpStartPostion = transform.localPosition;

        Vector2 newPoint = currentGridPosition;
        newPoint.x += Mathf.Sign(xDirection) * Mathf.Abs(Mathf.Ceil(xDirection));
        newPoint.y += Mathf.Sign(zDirection) * Mathf.Abs(Mathf.Ceil(zDirection));
        Vector3 newPosition = targetGrid.GridToWorldPoisiton(newPoint);

        for (float i = 0; i <= time; i += Time.deltaTime)
        {
            float moveTime = i / time;
            transform.localPosition = Vector3.Lerp(lerpStartPostion, newPosition, moveTime);
            yield return null;
        }
        currentGridPosition = newPoint;
        currentTargetPosition = newPosition;
        moving = false;
    }

    IEnumerator Move(float time)
    {
        moving = true;
        lerpStartPostion = transform.localPosition;

        Vector3 newPosition = targetGrid.GridToWorldPoisiton(currentGridPosition);

        for (float i = 0; i <= time; i += Time.deltaTime)
        {
            float moveTime = i / time;
			Debug.Log(time);
            transform.localPosition = Vector3.Lerp(lerpStartPostion, newPosition, moveTime);
            yield return null;
        }
        destinationGridPosition = currentGridPosition; 
        currentTargetPosition = transform.localPosition;
        moving = false;
    }

    /*void Invulnerable()
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
    }*/

    void OnTriggerEnter(Collider hit)
    {
        /*EnvironmentalHazard isEnviro = hit.GetComponent<EnvironmentalHazard>();
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
        }*/
        
    }
}
