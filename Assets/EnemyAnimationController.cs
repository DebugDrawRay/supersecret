using UnityEngine;
using System.Collections;
using Spine;
public class EnemyAnimationController : MonoBehaviour
{
    public SkeletonAnimation anim;

    [Header("Available Animations")]
    public string idleAnim;
    public string collisionAnimLeft;
    public string collisionAnimRight;
    [Header("Animation Properties")]
    public float universalTimeScale;
    void Start()
    {
        if(anim)
        {
            anim.timeScale = universalTimeScale;
            anim.state.SetAnimation(0, idleAnim, true);
        }
    }

    void OnTriggerEnter(Collider hit)
    {
        PlayerController isPlayer = hit.GetComponent<PlayerController>();
        if (anim && isPlayer)
        {
            float dir = transform.localPosition.x - hit.transform.localPosition.x;

            if(-dir < 0)
            {
                anim.state.SetAnimation(1, collisionAnimLeft, false);
            }
            else if (-dir > 0)
            {
                anim.state.SetAnimation(1, collisionAnimRight, false);
            }
        }
    }
}
