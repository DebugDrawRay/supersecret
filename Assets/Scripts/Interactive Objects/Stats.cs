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
    [SerializeField]
    public StatsCollection collection;

    private Timer currentTimer;

    public float distanceTraveled;
    public float minRequiredDistanceTraveled;
    public bool invulnerable;
    public bool isDead
    {
        get;
        private set;
    }
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

    void Start()
    {
        //collection.Init();
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
                    collection.currentHealth += value;
                    break;
                case stat.maxHealth:
                    collection.maxHealth = collection.defaultMaxHealth + value;
                    break;
                case stat.speed:
                    collection.speed = collection.defaultSpeed + value;
                    break;
                case stat.agility:
                    collection.agility = collection.defaultAgility + value;
                    break;
                case stat.power:
                    collection.power = collection.defaultPower + value;
                    break;
                case stat.luck:
                    collection.luck = collection.defaultLuck + value;
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
                    collection.maxHealth = collection.defaultMaxHealth + value;
                    currentTimer = new Timer(time);
                    break;
                case stat.speed:
                    collection.speed = collection.defaultSpeed + value;
                    currentTimer = new Timer(time);
                    break;
                case stat.agility:
                    collection.agility = collection.defaultAgility + value;
                    currentTimer = new Timer(time);
                    break;
                case stat.power:
                    collection.power = collection.defaultPower + value;
                    currentTimer = new Timer(time);
                    break;
                case stat.luck:
                    collection.luck = collection.defaultLuck + value;
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
        if (collection.currentHealth <= 0)
        {
            isDead = true;
        }
    }

    void ResetStats()
    {
        collection.maxHealth = collection.defaultMaxHealth;
        collection.speed = collection.defaultSpeed;
        collection.agility = collection.defaultAgility;
        collection.power = collection.defaultPower;
        collection.luck = collection.defaultLuck;
    }
}
