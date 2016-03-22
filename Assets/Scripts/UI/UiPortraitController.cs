using UnityEngine;
using System.Collections;
using Spine;
public class UiPortraitController : MonoBehaviour
{
    public SkeletonGraphic anim;

    void Start()
    {
        anim.AnimationState.SetAnimation(0, "trundle_shake", true);
        anim.AnimationState.SetAnimation(1, "biomes_rotate", true);
    }
}
