using UnityEngine;
using System.Collections;

public class InteractionSource : MonoBehaviour
{
    public Stats.stats targetStat;

    private string[] availableStats = new string[]{"health", "maxHealth", "speed", "agility", "power", "luck"};
    private string stat;

    [Range(-1,1)]
    public float modValue;

    public float modTime;

    void Awake()
    {
        stat = availableStats[(int)targetStat];
    }
    void OnTriggerEnter(Collider hit)
    {
        Stats hasStats = hit.GetComponent<Stats>();
        if (hasStats)
        {
            if (modTime > 0)
            {
                hasStats.ModStat(stat, modValue, modTime);
            }
            else
            {
                hasStats.ModStat(stat, modValue);
            }
        }
    }
}
