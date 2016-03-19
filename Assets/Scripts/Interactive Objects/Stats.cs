using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour
{
    [Range(0,1)]
    public float maxHealth;
    private float defaultMaxHealth;

    private float currentHealth;

    [Range(0,1)]
    public float speed;
    private float defaultSpeed;

    [Range(0,1)]
    public float agility;
    private float defaultAgility;

    [Range(0,1)]
    public float power;
    private float defaultPower;

    [Range(0,1)]
    public float luck;
    private float defaultLuck;

    [Range(0,1)]
    public float weight;
    private float defaultWeight;

    private Timer currentTimer;

    public bool invulnerable;

    public class Timer
    {
        float currentTime;

        public Timer(float time)
        {
            currentTime = time;
        }

        public bool Run()
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    void Awake()
    {
        currentHealth = maxHealth;
        defaultMaxHealth = maxHealth;
        defaultSpeed = speed;
        defaultAgility = agility;
        defaultPower = power;
        defaultLuck = luck;
        defaultWeight = weight;
    }

    void Update()
    {
        if (currentTimer != null)
        {
            if (currentTimer.Run())
            {
                ResetStats();
                currentTimer = null;
            }
        }
    }

    public void ModStat(string stat, float value)
    {
        if (!invulnerable)
        {
            value = Mathf.Clamp(value, -1, 1);
            switch (stat)
            {
                case "health":
                    currentHealth += value;
                    break;
                case "maxhealth":
                    maxHealth = defaultMaxHealth + value;
                    break;
                case "speed":
                    speed = defaultSpeed + value;
                    break;
                case "agility":
                    agility = defaultAgility + value;
                    break;
                case "power":
                    power = defaultPower + value;
                    break;
                case "luck":
                    luck = defaultLuck + value;
                    break;
                default:
                    Debug.LogError("No stat of name " + stat + " found, check spelling?");
                    break;
            }
            CheckStats();
        }
    }

    public void ModStat(string stat, float value, float time)
    {
        if (!invulnerable)
        {
            value = Mathf.Clamp(value, -1, 1);
            switch (stat)
            {
                case "maxhealth":
                    maxHealth = defaultMaxHealth + value;
                    currentTimer = new Timer(time);
                    break;
                case "speed":
                    speed = defaultSpeed + value;
                    currentTimer = new Timer(time);
                    break;
                case "agility":
                    agility = defaultAgility + value;
                    currentTimer = new Timer(time);
                    break;
                case "power":
                    power = defaultPower + value;
                    currentTimer = new Timer(time);
                    break;
                case "luck":
                    luck = defaultLuck + value;
                    currentTimer = new Timer(time);
                    break;
                default:
                    Debug.LogError("No stat of name " + stat + " found, check spelling?");
                    break;
            }
            CheckStats();
        }
    }

    void CheckStats()
    {
        PlayerController isPlayer = GetComponent<PlayerController>();
        Enemy isEnemy = GetComponent<Enemy>();
        if (isPlayer)
        {
            PlayerEventManager.TriggerCollision();

            if (currentHealth <= 0)
            {
                PlayerEventManager.PlayerDeath();
            }
        }
    }

    void ResetStats()
    {
        maxHealth = defaultMaxHealth;
        speed = defaultSpeed;
        agility = defaultAgility;
        power = defaultPower;
        luck = defaultLuck;
    }

}
