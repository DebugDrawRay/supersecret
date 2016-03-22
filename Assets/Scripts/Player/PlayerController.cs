using UnityEngine;
using UnityEngine.SceneManagement;
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
    public EquipmentController equipment;

    [Header("Collision Properties")]
    public float invulTime;
    private bool invulnerable;

    //Health Control
    private float currentHealth;

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
        PlayerEventManager.CollisionReaction += InvulEvent;
        PlayerEventManager.DeathEvent += DeathEvent;
        InitializeInstance();
    }

    void InitializeInstance()
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
        equipment.Init();

        movement.Init(stats, grid, animation);

        SetupInput();

        animation.Init();

        SetupHealth();

        initialized = true;
    }

    void SetupHealth()
    {
        currentHealth = stats.maxHealth;
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
                Destroy(gameObject);
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

    public void DeathEvent()
    {
        Debug.Log("trigger");
        currentState = State.Dead;
    }

    void InvulEvent()
    {
        animation.StartInvulAnim(invulTime);
        StartCoroutine(Invul());
    }

    IEnumerator Invul()
    {
        invulnerable = true;
        stats.invulnerable = true;
        for(float i = 0; i <= invulTime; i += Time.deltaTime)
        {
            yield return null;
        }
        invulnerable = false;
        stats.invulnerable = false;
    }

    void OnTriggerEnter(Collider hit)
    {
        if (!invulnerable)
        {
            Enemy isEnemy = hit.GetComponent<Enemy>();
            if (isEnemy)
            {
                movement.CollisionMove(isEnemy.transform.localPosition);
            }
        }
    }
}
