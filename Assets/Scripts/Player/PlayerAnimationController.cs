using UnityEngine;
using System.Collections;
using Spine;
public class PlayerAnimationController : MonoBehaviour
{
    [Header("Animation Components")]
    public PlayerController player;
    private SkeletonAnimation[] allAnim;
    public SkeletonAnimation bodyAnim;
    public SkeletonAnimation bikeAnim;
    public SkeletonAnimation headAnim;
    public SkeletonAnimation leftAnim;
    public SkeletonAnimation rightAnim;

    private bool initialized;

    //lean animation control
    private Vector3 leanDirection;
    private float leanAmount;

    private bool leanRight;
    private bool leanLeft;

    [Range(0,1)]
    public float lean;

    public bool inTopSpeed;
    void Start()
    {
        PlayerEventManager.HitReaction += CollisionAnim;

       // PlayerEventManager.TopSpeedEvent += TopSpeed;
    }

    public void Init()
    {
        allAnim = new SkeletonAnimation[5] { headAnim, bodyAnim, bikeAnim, leftAnim, rightAnim };
        for (int i = 0; i < allAnim.Length; ++i)
        {
            allAnim[i].state.SetAnimation(0, "idle", true);
        }
        initialized = true;
    }

    void CollisionAnim()
    {
        for (int i = 0; i < allAnim.Length; ++i)
        {
            allAnim[i].state.ClearTrack(2);
            allAnim[i].skeleton.SetToSetupPose();
            allAnim[i].state.SetAnimation(1, "collide_front", false);

            leanLeft = false;
            leanRight = false;
        }
    }

    public void StartInvulAnim()
    {
        StartCoroutine(InvulAnim(player.invulTime));
    }

    IEnumerator InvulAnim(float time)
    {
        for(float i = 0; i <= time; i += Time.deltaTime)
        {
            for (int j = 0; j < allAnim.Length; ++j)
            {
                allAnim[j].GetComponent<MeshRenderer>().enabled = !allAnim[j].GetComponent<MeshRenderer>().enabled;
            }
            yield return null;
        }
        for (int j = 0; j < allAnim.Length; ++j)
        {
            allAnim[j].GetComponent<MeshRenderer>().enabled = true;
        }
    }
    void TopSpeed()
    {
        inTopSpeed = true;
        for (int i = 0; i < allAnim.Length; ++i)
        {
            allAnim[i].state.ClearTracks();
            allAnim[i].state.SetAnimation(0, "idle_to_fast", true);
            allAnim[i].state.GetCurrent(0).time = 0;
            allAnim[i].state.GetCurrent(0).next = allAnim[i].state.SetAnimation(0, "fast", true);
        }
    }

    public void AnimateLean(Vector3 direction)
    {
        float xdir = direction.x;
        leanAmount = xdir / 8.5f;
        leanAmount = Mathf.Round(leanAmount * 1000) / 1000;

        string left = "turn_left";
        string right = "turn_right";

        if(inTopSpeed)
        {
            left = "turn_left_fast";
            right = "turn_right_fast";
        }
        if (leanAmount < 0)
        {
            if (!leanLeft)
            {
                for (int i = 0; i < allAnim.Length; ++i)
                {
                    leanLeft = true;
                    leanRight = false;
                    allAnim[i].state.SetAnimation(2, left, false);
                }
            }
        }
        if(leanAmount > 0)
        {
            if (!leanRight)
            {
                for (int i = 0; i < allAnim.Length; ++i)
                {
                    leanLeft = false;
                    leanRight = true;
                    allAnim[i].state.SetAnimation(2, right, false);
                }
            }
        }
        for (int i = 0; i < allAnim.Length; ++i)
        {
            if (allAnim[i].state.GetCurrent(2) != null)
            {
                float newTime = allAnim[i].skeleton.data.FindAnimation(right).duration * leanAmount;
                float currentTime = allAnim[i].state.GetCurrent(2).Time;
                allAnim[i].state.GetCurrent(2).Time = Mathf.Clamp(Mathf.Abs(newTime), 0, .99f);
                if(allAnim[i].state.GetCurrent(2).Time <= .02f)
                {
                    leanLeft = false;
                    leanRight = false;
                    allAnim[i].state.ClearTrack(2);
                    allAnim[i].skeleton.SetToSetupPose();
                }
            }
        }
    }
}
