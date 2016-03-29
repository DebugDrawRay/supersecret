using UnityEngine;
using System.Collections;

[System.Serializable]
public class InteractionProperty
{
    public Stats.stat statToAffect;

    [Range(-1, 1)]
    public float affectAmount;

    public bool permanent;
    public float modTime;

}
