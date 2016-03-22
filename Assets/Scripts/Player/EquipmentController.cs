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
        }
        if (bodyAttachment)
        {
        }
        if (bikeAttachment)
        {
        }
        if (leftAttachment)
        {
        }
        if (rightAttachment)
        {
        }
    }

}
