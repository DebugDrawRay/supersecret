using UnityEngine;

public class InteractionSource : MonoBehaviour
{
    [SerializeField]
    public InteractionProperty[] interactions;

    public string collisionSound;
    void OnTriggerEnter(Collider hit)
    {
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

            if (collisionSound != "")
            {
                AkSoundEngine.PostEvent(collisionSound, gameObject);

				if (gameObject.name == "Canyon_tree(Clone)")
				{
					Debug.Log("TREE!");
					AkSoundEngine.SetSwitch("Music", "canyonBoss", this.gameObject);
				}
            }
        }
    }

}
