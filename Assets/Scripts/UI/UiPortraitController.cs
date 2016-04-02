using UnityEngine;
using System.Collections;
using Spine;
public class UiPortraitController : MonoBehaviour
{
    public SkeletonGraphic anim;

    private bool fullSpeed;
    void Start()
    {
        StartPersistentAnims();
        AddEvents();
    }

    void StartPersistentAnims()
    {
        anim.AnimationState.SetAnimation(10, "biomes_bounce", true);
        anim.AnimationState.SetAnimation(9, "mini_trundle_idle", true);
        anim.AnimationState.SetAnimation(8, "tower_flash", true);
        anim.AnimationState.SetAnimation(7, "tower_sway", true);
        anim.AnimationState.SetAnimation(6, "trundle_shake", true);
        anim.AnimationState.SetAnimation(5, "biomes_rotate", true);
    }

    void AddEvents()
    {
        PlayerEventManager.EnemyHit += CollisionAnim;
        PlayerEventManager.ObjectCollision += CollisionAnim;
    }

    void Update()
    {
        UpdateMeters();
    }

    void UpdateMeters()
    {
        float speed = Grid.instance.GetComponent<LevelMovement>().GetNormalizedSpeed();
        if(speed > .9f && !fullSpeed)
        {
            anim.AnimationState.SetAnimation(6, "trundle_full_speed_start", false);
            anim.AnimationState.GetCurrent(6).next = anim.AnimationState.SetAnimation(6, "trundle_full_speed_sustain", true);
            fullSpeed = true;
        }
        else if(speed < .9f && fullSpeed)
        {

            fullSpeed = false;
        }
        anim.AnimationState.GetCurrent(5).timeScale = speed * 3;
    }

    void CollisionAnim()
    {
        anim.AnimationState.ClearTrack(6);
        anim.Skeleton.SetToSetupPose();
        anim.AnimationState.SetAnimation(4, "trundle_hit_left", false);
        anim.AnimationState.SetAnimation(6, "trundle_shake", true);
    }
}
