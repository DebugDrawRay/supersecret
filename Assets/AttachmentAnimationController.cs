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
            anim.state.SetAnimation(0, "cycle", true);
        }
    }

    void Update()
    {
        if (anim)
        {
            float value = anim.state.GetCurrent(0).endTime * stat.collection.currentHealth;
            anim.state.GetCurrent(0).time = value;
        }
    }
}