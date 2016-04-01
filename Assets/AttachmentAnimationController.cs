using UnityEngine;
using System.Collections;
using Spine;

public class AttachmentAnimationController : MonoBehaviour
{
    public SkeletonAnimation anim;
    public Stats stat;

    void Start()
    {
        if (anim)
        {
        }
    }

    void Update()
    {
        if (anim)
        {
            float value = stat.collection.currentHealth;
            float steps = value / 5;

            if(value <= value - (steps * 4))
            {
                anim.state.SetAnimation(21, "damage_5", true);
            }
            else if (value <= value - (steps * 3))
            {
                anim.state.SetAnimation(21, "damage_4", true);
            }
            else if (value <= value - (steps * 2))
            {
                anim.state.SetAnimation(21, "damage_3", true);
            }
            else if (value <= value - (steps))
            {
                anim.state.SetAnimation(21, "damage_2", true);
            }
        }
    }
}