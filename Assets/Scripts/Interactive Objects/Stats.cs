using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour
{
    public float health;
    private float defaultHealth;

    public float speed;
    private float defaultSpeed;

    public float agility;
    private float defaultAgility;

    public float power;
    private float defaultPower;

    public float luck;
    private float defaultLuck;

    public float weight;
    private float defaultWeight;

    private Timer currentTimer;

    private bool invul;
    public float invulTime;

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
        defaultHealth = health;
        defaultSpeed = speed;
        defaultAgility = agility;
        defaultPower = power;
        defaultLuck = luck;
        defaultWeight = weight;

        EventManager.CollisionReaction += Invulnerable;
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
        if (!invul)
        {
            switch (stat)
            {
                case "health":
                    health = defaultHealth + value;
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
        }
    }

    public void ModStat(string stat, float value, float time)
    {
        if (!invul)
        {
            switch (stat)
            {
                case "health":
                    health = defaultHealth + value;
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
        }
    }

    void ResetStats()
    {
        health = defaultHealth;
        speed = defaultSpeed;
        agility = defaultAgility;
        power = defaultPower;
        luck = defaultLuck;
    }

    void Invulnerable()
    {
        StartCoroutine(InvulnerableRoutine());
    }

    IEnumerator InvulnerableRoutine()
    {
        invul = true;

        for(float i = 0; i <= invulTime; i += Time.deltaTime)
        {
            yield return null;
        }

        invul = false;
    }
}
