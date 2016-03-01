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

    void ModStat(string stat, float value, float time)
    {
        switch(stat)
        {
            case "health":
                health += value;
                currentTimer = new Timer(time);
                break;
            case "speed":
                speed += value;
                currentTimer = new Timer(time);
                break;
            case "agility":
                agility += value;
                currentTimer = new Timer(time);
                break;
            case "power":
                power += value;
                currentTimer = new Timer(time);
                break;
            case "luck":
                luck += value;
                currentTimer = new Timer(time);
                break;
            default:
                Debug.LogError("No stat of name " + stat + " found, check spelling?");
                break;
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
}
