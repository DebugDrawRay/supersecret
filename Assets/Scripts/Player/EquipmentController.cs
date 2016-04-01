using UnityEngine;
using System.Collections;
using Spine;

public class EquipmentController : MonoBehaviour
{
    public GameObject headAttachment;
    public PlayerAttachment equippedHead;

    public GameObject bodyAttachment;
    public PlayerAttachment equippedBody;

    public GameObject bikeAttachment;
    public PlayerAttachment equippedBike;

    public GameObject leftAttachment;
    public PlayerAttachment equippedLeft;

    public GameObject rightAttachment;
    public PlayerAttachment equippedRight;

    public void Init()
    {
        EquipAttachments();
    }

    public void EquipAttachments()
    {
        if (headAttachment)
        {
            string skin = equippedHead.headSkin.ToString();
            if(skin == "_default")
            {
                skin = "default";
            }
            headAttachment.GetComponent<SkeletonAnimation>().skeleton.SetSkin(skin);
            headAttachment.GetComponent<InteractionSource>().interactions = equippedHead.interactions;
            headAttachment.GetComponent<Stats>().collection = equippedHead.collection;
        }
        if (bodyAttachment)
        {
            string skin = equippedBody.bodySkin.ToString();
            if (skin == "_default")
            {
                skin = "default";
            }
            bodyAttachment.GetComponent<SkeletonAnimation>().skeleton.SetSkin(skin);
            bodyAttachment.GetComponent<InteractionSource>().interactions = equippedBody.interactions;
            bodyAttachment.GetComponent<Stats>().collection = equippedBody.collection;
        }
        if (bikeAttachment)
        {
            string skin = equippedLeft.bikeSkin.ToString();
            if (skin == "_default")
            {
                skin = "default";
            }
            bikeAttachment.GetComponent<SkeletonAnimation>().skeleton.SetSkin(skin);
            bikeAttachment.GetComponent<InteractionSource>().interactions = equippedBike.interactions;
            bikeAttachment.GetComponent<Stats>().collection = equippedBike.collection;
        }
        if (leftAttachment)
        {
            string skin = equippedLeft.leftSkin.ToString();
            if (skin == "_default")
            {
                skin = "default";
            }
            leftAttachment.GetComponent<SkeletonAnimation>().skeleton.SetSkin(skin);
            leftAttachment.GetComponent<InteractionSource>().interactions = equippedLeft.interactions;
            leftAttachment.GetComponent<Stats>().collection = equippedLeft.collection;
        }
        if (rightAttachment)
        {
            string skin = equippedRight.rightSkin.ToString();
            if (skin == "_default")
            {
                skin = "default";
            }
            rightAttachment.GetComponent<SkeletonAnimation>().skeleton.SetSkin(skin);
            rightAttachment.GetComponent<InteractionSource>().interactions = equippedRight.interactions;
            rightAttachment.GetComponent<Stats>().collection = new StatsCollection(equippedRight.collection);
        }
    }

    void OnTriggerEnter(Collider hit)
    {
        Enemy isEnemy = hit.GetComponent<Enemy>();
        
        if(isEnemy)
        {
            Stats enemy = isEnemy.GetComponent<Stats>();

            if (enemy)
            {
                Vector2 dirVect = isEnemy.transform.localPosition -transform.localPosition;
                float y = dirVect.y;
                float x = dirVect.x;

                if (Mathf.Abs(y) > Mathf.Abs(x))
                {
                    if (y > 0)
                    {
                        PlayerAttachment attach = equippedHead;
                        Stats stats = headAttachment.GetComponent<Stats>();
                        Interact(enemy, attach.interactions);
                        Interact(stats, enemy.GetComponent<InteractionSource>().interactions);
                    }
                    else if (y < 0)
                    {
                        PlayerAttachment attach = equippedBody;
                        Stats stats = bodyAttachment.GetComponent<Stats>();
                        Interact(enemy, attach.interactions);
                        Interact(stats, enemy.GetComponent<InteractionSource>().interactions);
                    }
                }
                else if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    if (x > 0)
                    {
                        PlayerAttachment attach = equippedRight;
                        Stats stats = rightAttachment.GetComponent<Stats>();
                        Interact(enemy, attach.interactions);
                        Interact(stats, enemy.GetComponent<InteractionSource>().interactions);
                    }
                    else if (x < 0)
                    {
                        PlayerAttachment attach = equippedLeft;
                        Stats stats = leftAttachment.GetComponent<Stats>();
                        Interact(enemy, attach.interactions);
                        Interact(stats, enemy.GetComponent<InteractionSource>().interactions);
                    }
                }
            }
        }
    }

    void Interact(Stats target, InteractionProperty[] interactions)
    {
        for (int i = 0; i < interactions.Length; i++)
        {
            InteractionProperty inter = interactions[i];

            if (inter.permanent)
            {
                target.ModStat(inter.statToAffect, inter.affectAmount);
            }
            else
            {
                target.ModStat(inter.statToAffect, inter.affectAmount, inter.modTime);
            }
        }
    }

    void Update()
    {
        if(rightAttachment.GetComponent<Stats>().isDead)
        {
            Destroy(rightAttachment);
        }
    }
}
