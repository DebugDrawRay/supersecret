using UnityEngine;
using System.Collections;

public class InteractionSource : MonoBehaviour
{
    public string targetStat;
    public float modValue;
    public float modTime;

    void OnTriggerEnter(Collider hit)
    {
        Stats hasStats = hit.GetComponent<Stats>();
        if (hasStats)
        {
            if (modTime > 0)
            {
                hasStats.ModStat(targetStat, modValue, modTime);
            }
            else
            {
                hasStats.ModStat(targetStat, modValue);
            }
            EventManager.TriggerCollision();

        }
    }
}
