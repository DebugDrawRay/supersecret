using UnityEngine;
using System.Collections;

public class ManeuveringEnemy : Enemy
{
    public enum state
    {
        Inactive,
        Maneuvering,
        Chasing
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

    void Start()
    {
        inactiveTime = Random.Range(minInactiveTime, maxInactiveTime);
        maneuverTime = Random.Range(minManeuverTime, maxManeuverTime);
        chaseTime = Random.Range(minChaseTime, maxChaseTime);

        initialized = true;
    }
    void Update()
    {
        if (initialized)
        {
            RunStates();
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
                if (maneuverTime <= 0)
                {
                    maneuverTime = Random.Range(minManeuverTime, maxManeuverTime);
                    currentState = SelectState();
                }
                break;
            case state.Chasing:
                chaseTime -= Time.deltaTime;
                if (chaseTime <= 0)
                {
                    chaseTime = Random.Range(minChaseTime, maxChaseTime);
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
}
