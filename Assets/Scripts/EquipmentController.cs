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
            MeshRenderer render = headAttachment.GetComponent<MeshRenderer>();
            SkeletonAnimation anim = headAttachment.GetComponent<SkeletonAnimation>();

            if (render)
            {
                render.materials = equippedHead.spriteMaterials;
            }
            else
            {
                Debug.LogError("No mesh renderer found");
            }

            InteractionSource interact = headAttachment.GetComponent<InteractionSource>();
            if (interact)
            {

            }
            else
            {
                Debug.LogError("No interaction source component found");
            }
        }
        if (bodyAttachment)
        {
            MeshRenderer render = bodyAttachment.GetComponent<MeshRenderer>();
            if (render)
            {
                render.materials = equippedBody.spriteMaterials;
            }
            else
            {
                Debug.LogError("No mesh renderer found");
            }

            InteractionSource interact = bodyAttachment.GetComponent<InteractionSource>();
            if (interact)
            {

            }
            else
            {
                Debug.LogError("No interaction source component found");
            }
        }
        if (bikeAttachment)
        {
            MeshRenderer render = bikeAttachment.GetComponent<MeshRenderer>();
            if (render)
            {
                render.materials = equippedBike.spriteMaterials;
            }
            else
            {
                Debug.LogError("No mesh renderer found");
            }

            InteractionSource interact = bikeAttachment.GetComponent<InteractionSource>();
            if (interact)
            {

            }
            else
            {
                Debug.LogError("No interaction source component found");
            }
        }
        if (leftAttachment)
        {
            MeshRenderer render = leftAttachment.GetComponent<MeshRenderer>();
            if (render)
            {
                render.materials = equippedLeft.spriteMaterials;
            }
            else
            {
                Debug.LogError("No mesh renderer found");
            }

            InteractionSource interact = leftAttachment.GetComponent<InteractionSource>();
            if (interact)
            {

            }
            else
            {
                Debug.LogError("No interaction source component found");
            }
        }
        if (rightAttachment)
        {
            MeshRenderer render = rightAttachment.GetComponent<MeshRenderer>();
            if (render)
            {
                render.materials = equippedRight.spriteMaterials;
            }
            else
            {
                Debug.LogError("No mesh renderer found");
            }

            InteractionSource interact = rightAttachment.GetComponent<InteractionSource>();
            if (interact)
            {

            }
            else
            {
                Debug.LogError("No interaction source component found");
            }
        }
    }

}
