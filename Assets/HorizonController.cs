using UnityEngine;
using System.Collections;

public class HorizonController : MonoBehaviour
{
    private Transform target;
    private float zOffset;
    private float yOffset;
    private bool initialized;

    public void Init(Vector3 startPoint, Transform targetPoint)
    {
        transform.position = startPoint;
        zOffset = startPoint.z;
        yOffset = startPoint.y;
        target = targetPoint;
        initialized = true;
    }

    void Update()
    {
        if (initialized)
        {
            Vector3 local = target.position;
            local.z = local.z + zOffset;
            local.y = yOffset;
            transform.position = local;
        }
    }
}