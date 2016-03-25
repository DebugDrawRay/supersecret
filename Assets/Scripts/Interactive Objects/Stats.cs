using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour
{
    public enum stat
    {
        health = 0,
        maxHealth,
        speed,
        agility,
        power,
        luck
    }

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

    public float distanceTraveled;
    public float minRequiredDistanceTraveled;

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

    public void ModStat(stat stat, float value)
    {
        if (!invulnerable)
        {
            value = Mathf.Clamp(value, -1, 1);
            switch (stat)
            {
                case stat.health:
                    currentHealth += value;
                    break;
                case stat.maxHealth:
                    maxHealth = defaultMaxHealth + value;
                    break;
                case stat.speed:
                    speed = defaultSpeed + value;
                    break;
                case stat.agility:
                    agility = defaultAgility + value;
                    break;
                case stat.power:
                    power = defaultPower + value;
                    break;
                case stat.luck:
                    luck = defaultLuck + value;
                    break;
                default:
                    break;
            }
            CheckStats();
        }
    }

    public void ModStat(stat stat, float value, float time)
    {
        if (!invulnerable)
        {
            value = Mathf.Clamp(value, -1, 1);
            switch (stat)
            {
                case stat.maxHealth:
                    maxHealth = defaultMaxHealth + value;
                    currentTimer = new Timer(time);
                    break;
                case stat.speed:
                    speed = defaultSpeed + value;
                    currentTimer = new Timer(time);
                    break;
                case stat.agility:
                    agility = defaultAgility + value;
                    currentTimer = new Timer(time);
                    break;
                case stat.power:
                    power = defaultPower + value;
                    currentTimer = new Timer(time);
                    break;
                case stat.luck:
                    luck = defaultLuck + value;
                    currentTimer = new Timer(time);
                    break;
                default:
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
