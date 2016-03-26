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
        Stunned,
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
    public float stunTime;
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
        PlayerEventManager.StunReaction += StunEvent;
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
            case State.Stunned:
                break;
            case State.LevelComplete:
                break;
        }
    }
    void RunActions()
    {
        if(movement)
        {
            float x = input.Move.X;
            float y = input.Move.Y;
            if (x > .2f)
            {
                movement.Move(1, 0);
            }
            else if (x < -.2f)
            {
                movement.Move(-1, 0);
            }
            else if (y > .2f)
            {
                movement.Move(0, 1);
            }
            else if (y < -.2f)
            {
                movement.Move(0, -1);
            }
        }
    }

    public void DeathEvent()
    {
        Debug.Log("trigger");
        currentState = State.Dead;
    }

    void StunEvent()
    {
        StartCoroutine(Stunned());
    }

    IEnumerator Stunned()
    {
        currentState = State.Stunned;
        for (float i = 0; i <= stunTime; i += Time.deltaTime)
        {
            yield return null;
        }
        currentState = State.InGame;
    }

    IEnumerator Invul()
    {
        stats.invulnerable = true;
        for(float i = 0; i <= invulTime; i += Time.deltaTime)
        {
            yield return null;
        }
        stats.invulnerable = false;
    }

    void OnTriggerEnter(Collider hit)
    {
        Enemy isEnemy = hit.GetComponent<Enemy>();
        if(isEnemy)
        {
            PlayerEventManager.TriggerStun();
            ContestSpace(isEnemy.GetComponent<Stats>());
            PlayerEventManager.TriggerCollision(hit.transform.localPosition);

        }

    }

    public void ContestSpace(Stats challenger)
    {
        float attack = stats.speed + stats.agility + stats.weight + stats.distanceTraveled;
        float defense = challenger.speed + challenger.agility + challenger.weight + challenger.distanceTraveled;

        if (attack < defense)
        {
            Debug.Log(name + " loses!");
            if (challenger.distanceTraveled > challenger.minRequiredDistanceTraveled)
            {
            }
        }
        else
        {
            Debug.Log(name + " wins!");
            if(stats.distanceTraveled < stats.minRequiredDistanceTraveled)
            {
            }
        }
    }
}
