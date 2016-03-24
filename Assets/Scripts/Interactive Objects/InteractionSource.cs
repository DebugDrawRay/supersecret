using UnityEngine;

public class InteractionSource : MonoBehaviour
{
    [SerializeField]
    public InteractionProperty[] interactions;

    void OnTriggerEnter(Collider hit)
    {
        Stats thisStats = transform.parent.GetComponent<Stats>();
        Stats hasStats = hit.GetComponent<Stats>();
        if (hasStats)
        {
            for (int i = 0; i < interactions.Length; i++)
            {
                InteractionProperty inter = interactions[i];

                if (inter.permanent)
                {
                    hasStats.ModStat(inter.statToAffect, inter.affectAmount);
                }
                else
                {
                    hasStats.ModStat(inter.statToAffect, inter.affectAmount, inter.modTime);
                }
            }
            hasStats.ContestSpace(hasStats);

        }
    }

}
