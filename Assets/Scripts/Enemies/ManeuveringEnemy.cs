using UnityEngine;
using System.Collections;

public class ManeuveringEnemy : Enemy
{
    public enum state
    {
        Inactive,
        Maneuvering,
        Chasing,
        Stunned,
        Dead
    }
    [Header("States")]
    public state currentState;

    [Header("Behavior")]
    public float minInactiveTime;
    public float maxInactiveTime;
    private float inactiveTime;
    [Range(0,1)]
    public float chanceToInactive;

    [Space(10)]
    public float minManeuverTime;
    public float maxManeuverTime;
    private float maneuverTime;
    [Range(0, 1)]
    public float chanceToManeuver;

    [Space(10)]
    public float minChaseTime;
    public float maxChaseTime;
    private float chaseTime;
    [Range(0, 1)]
    public float chanceToChase;

    [Header("Collision")]
    public float stunTime;
    private float currentStunTime;

    void Start()
    {
        inactiveTime = Random.Range(minInactiveTime, maxInactiveTime);
        maneuverTime = Random.Range(minManeuverTime, maxManeuverTime);
        chaseTime = Random.Range(minChaseTime, maxChaseTime);

        currentStunTime = stunTime;

        Collision += TriggerStun;
    }

    void Update()
    {
        CheckGridPosition();
        if (initialized && enteredGrid)
        {
            RunStates();
        }
        if(stats.isDead)
        {
            Destroy(gameObject);
        }
    }

    void RunStates()
    {
        switch(currentState)
        {
            case state.Inactive:
                inactiveTime -= Time.deltaTime;
                if(inactiveTime <= 0)
                {
                    inactiveTime = Random.Range(minInactiveTime, maxInactiveTime);
                    currentState = SelectState();
                }
                break;
            case state.Maneuvering:
                maneuverTime -= Time.deltaTime;
                movement.MoveToRandomDestination();
                if (maneuverTime <= 0)
                {
                    maneuverTime = Random.Range(minManeuverTime, maxManeuverTime);
                    currentState = SelectState();
                }
                break;
            case state.Chasing:
                chaseTime -= Time.deltaTime;
                Vector2 targetPos = target.movement.currentGridPosition;
                movement.MoveToDestination(targetPos);
                if (chaseTime <= 0)
                {
                    chaseTime = Random.Range(minChaseTime, maxChaseTime);
                    currentState = SelectState();
                }
                break;
            case state.Stunned:
                currentStunTime -= Time.deltaTime;
                if(currentStunTime <= 0)
                {
                    currentStunTime = stunTime;
                    currentState = SelectState();
                }
                break;
        }
    }

    state SelectState()
    {
        float total = chanceToInactive + chanceToManeuver + chanceToChase;
        float value = Random.value * total;

        if(value >= chanceToInactive + chanceToManeuver)
        {
            return state.Chasing;
        }
        else if (value >= chanceToInactive && value < chanceToInactive + chanceToManeuver)
        {
            return state.Maneuvering;
        }
        else
        {
            return state.Inactive;
        }
    }
    
    void TriggerStun()
    {
        inactiveTime = Random.Range(minInactiveTime, maxInactiveTime);
        maneuverTime = Random.Range(minManeuverTime, maxManeuverTime);
        chaseTime = Random.Range(minChaseTime, maxChaseTime);
        currentStunTime = stunTime;
        currentState = state.Stunned;
    }
}
