using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public enum State
    {
        Inactive,
        StartGame,
        InGame,
        Dead,
        LevelComplete
    }

    [Header("States")]
    public State currentState;

    [Header("Components")]
    public GridMovement movement;
    public Stats stats;

    //References
    private Grid gridSystem;

    //Input
    public PlayerActions input;

    private bool initialized;

    void SetupInput()
    {
        input = PlayerActions.BindKeyboardAndJoystick();
    }

    public void Init(Grid grid)
    {
        gridSystem = grid;
        transform.SetParent(gridSystem.transform);
        SetupInput();
        initialized = true;
    }

    void Update()
    {
        if(initialized)
        {
            RunStates();
        }
    }

    void RunStates()
    {
        switch(currentState)
        {
            case State.Inactive:
                break;
            case State.StartGame:
                break;
            case State.InGame:
                RunActions();
                break;
            case State.Dead:
                break;
            case State.LevelComplete:
                break;
        }
    }
    void RunActions()
    {
        if(movement)
        {
            movement.MovementListener(input.Move.Value);
        }
    }
}
