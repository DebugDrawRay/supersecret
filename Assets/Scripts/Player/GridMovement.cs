﻿using UnityEngine;
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

    public enum MovementType
    {
        Free,
        Fixed
    }
    public MovementType currentMovementType;

    [Header("Movement Properties")]
    public Vector2 startGridPosition;
    public AnimationCurve freeSpeedCurve;
    public AnimationCurve fixedSpeedCurve;
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
        PlayerEventManager.EnemyCollision += CollisionMove;

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
        switch (currentMovementType)
        {
            case MovementType.Fixed:
                FixedMoveUpdate();
                break;
            case MovementType.Free:
                FreeMoveUpdate();
                break;
        }
    }

    void FixedMoveUpdate()
    {
        if(isMoving)
        {
            Vector3 direction = targetLocalPosition - transform.localPosition;
            animation.AnimateLean(direction);
        }
    }
    void FreeMoveUpdate()
    {
        if (currentDestinationUpdate > 0)
        {
            currentDestinationUpdate -= Time.deltaTime;
        }
        if (isMoving && !isHit)
        {
            FreeMoveAction();
        }   
    }

    float MoveTime(float stat)
    {
        float deltaTime = maxTimeToMove - minTimeToMove;
        float rawTime = minTimeToMove + (deltaTime * (1 - stat));
        return rawTime;
    }

    public void Move(float x, float y)
    {
        switch (currentMovementType)
        {
            case MovementType.Fixed:
                FixedMove(x, y);
                break;
            case MovementType.Free:
                FreeMove(x, y);
                break;
        }
    }

    //Fixed Movement
    void FixedMove(float x, float y)
    {
        if(!isMoving)
        {
            Vector2 destinationGridPosition = Vector2.zero;
            destinationGridPosition.x = currentGridPosition.x + x;
            destinationGridPosition.y = currentGridPosition.y + y;
            if (targetGrid.CheckIfValidUnit(destinationGridPosition))
            {
                currentGridPosition = destinationGridPosition;
                targetLocalPosition = targetGrid.GridToWorldPosition(currentGridPosition);
                StartCoroutine(FixedMoveAction(MoveTime(stats.collection.speed), targetLocalPosition));
            }
        }
    }

    IEnumerator FixedMoveAction(float time, Vector3 toPosition)
    {
        isMoving = true;
        lerpStartPostion = transform.localPosition;
        targetLocalPosition = toPosition;
        AkSoundEngine.PostEvent("TB_tireSkidShort", this.gameObject);

        for (float i = 0; i <= time; i += Time.deltaTime)
        {
            float moveTime = i / time;
            transform.localPosition = Vector3.Lerp(lerpStartPostion, targetLocalPosition, freeSpeedCurve.Evaluate(moveTime));
            if(isHit)
            {
                isMoving = false;
                break;
            }
            yield return null;
        }
        isMoving = false;
    }

    //Free Movement
    void FreeMove(float x, float y)
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

					AkSoundEngine.PostEvent("TB_tireSkidShort", this.gameObject);
                }
            }
        }
    }

    void FreeMoveAction()
    {
        Vector3 direction = targetLocalPosition - transform.localPosition;
        animation.AnimateLean(direction);

        moveTime = Time.time - startTime;

        xPercentageComplete = moveTime / MoveTime(stats.collection.agility);
        yPercentageComplete = moveTime / MoveTime(stats.collection.speed);

        Vector3 newPosition = transform.localPosition;
        newPosition.x = Mathf.Lerp(lerpStartPostion.x, targetLocalPosition.x, freeSpeedCurve.Evaluate(xPercentageComplete));
        newPosition.y = Mathf.Lerp(lerpStartPostion.y, targetLocalPosition.y, freeSpeedCurve.Evaluate(yPercentageComplete));
        transform.localPosition = newPosition;

        if (xPercentageComplete >= 1 && yPercentageComplete >= 1)
        {
            isMoving = false;
        }
    }

    public void CollisionMove(Vector3 from)
    {
        Vector3 direction = from - transform.localPosition;

        float xDir = 0;
        float yDir = 0;

        if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
        {
            xDir = Mathf.Sign(direction.x) * -1;
            yDir = 0;

            float newX = targetGrid.GetClosestUnit(transform.localPosition).x + xDir;
            if (newX < 0 || newX >= targetGrid.xUnits)
            {
                xDir = 0;
                yDir = Mathf.Sign(direction.x) * -1;
                float newY = targetGrid.GetClosestUnit(transform.localPosition).y + yDir;
                if (newY < 0 || newY >= targetGrid.yUnits)
                {
                    yDir = -yDir;
                }
            }
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            yDir = Mathf.Sign(direction.y) * -1;
            xDir = 0;

            float newY = targetGrid.GetClosestUnit(transform.localPosition).y + yDir;
            if (newY < 0 || newY >= targetGrid.yUnits)
            {
                yDir = 0;
                xDir = Mathf.Sign(direction.y) * -1;
                float newX = targetGrid.GetClosestUnit(transform.localPosition).x + xDir;
                if (newX < 0 || newX >= targetGrid.xUnits)
                {
                    xDir = -xDir;
                }
            }
        }
        Vector2 newDirection = new Vector2(xDir, yDir);
        StartCoroutine(ForceMove(.25f, newDirection));
    }

    IEnumerator ForceMove(float time, Vector2 direction)
    {
        isHit = true;
        lerpStartPostion = transform.localPosition;

        Vector2 newPoint = direction + currentGridPosition;
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
