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
        }
        if (bodyAttachment)
        {
            string skin = equippedBody.bodySkin.ToString();
            if (skin == "_default")
            {
                skin = "default";
            }
            bodyAttachment.GetComponent<SkeletonAnimation>().skeleton.SetSkin(skin);
        }
        if (bikeAttachment)
        {
            string skin = equippedLeft.bikeSkin.ToString();
            if (skin == "_default")
            {
                skin = "default";
            }
            bikeAttachment.GetComponent<SkeletonAnimation>().skeleton.SetSkin(skin);
        }
        if (leftAttachment)
        {
            string skin = equippedLeft.leftSkin.ToString();
            if (skin == "_default")
            {
                skin = "default";
            }
            leftAttachment.GetComponent<SkeletonAnimation>().skeleton.SetSkin(skin);
        }
        if (rightAttachment)
        {
            string skin = equippedRight.rightSkin.ToString();
            if (skin == "_default")
            {
                skin = "default";
            }
            rightAttachment.GetComponent<SkeletonAnimation>().skeleton.SetSkin(skin);
        }
    }

}
