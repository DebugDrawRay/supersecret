using UnityEngine;
using Spine.Unity;
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
    public PlayerAnimationController animation;

    //References
    private Grid gridSystem;

    //Input
    public PlayerActions input;

    private bool initialized;

    public static PlayerController instance
    {
        get;
        private set;
    }

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    public void Init(Grid grid)
    {
        gridSystem = grid;
        transform.SetParent(gridSystem.transform);
        movement.Init(stats, grid, animation);
        SetupInput();
        animation.Init();
        initialized = true;
    }

    void SetupInput()
    {
        input = PlayerActions.BindKeyboardAndJoystick();
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
