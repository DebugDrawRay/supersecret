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
    public PlayerAnimationController anim;
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
        PlayerEventManager.EnemyHit += StunEvent;
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

        movement.Init(equipment.bikeAttachment.GetComponent<Stats>(), grid, anim);

        SetupInput();

        anim.Init();

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
            if (x > .5f)
            {
                movement.Move(1, 0);
            }
            else if (x < -.5f)
            {
                movement.Move(-1, 0);
            }
            else if (y > .5f)
            {
                movement.Move(0, 1);
            }
            else if (y < -.5f)
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
        InteractionSource isInteraction = hit.GetComponent<InteractionSource>();
        EnvironmentalHazard isEnviromental = hit.GetComponent<EnvironmentalHazard>();
        Enemy isEnemy = hit.GetComponent<Enemy>();

        if(isEnemy)
        {
            PlayerEventManager.TriggerEnemyCollision(isEnemy.transform.localPosition);
        }

        if(isEnviromental)
        {
            PlayerEventManager.TriggerObjectCollision();

            Stats hasStats = isInteraction.GetComponent<Stats>();
            if(hasStats)
            {
                //ContestSpace(hasStats);
            }
        }
    }

    public void ContestSpace(Stats challenger)
    {
        float attack = stats.collection.speed + stats.collection.agility + stats.collection.weight + stats.distanceTraveled;
        float defense = challenger.collection.speed + challenger.collection.agility + challenger.collection.weight + challenger.distanceTraveled;

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
