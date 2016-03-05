using UnityEngine;
using DG.Tweening;

public class QuadCam : MonoBehaviour
{
    public float tiltLimit;
    public float tiltSpeed;

    public float hoverHeight;
    public float hoverHeightDeviation;
    public float chaseDistance;

    public float moveSpeed;
    public float moveAcceleration;

    private Vector3 currentPosition;

    private Rigidbody rigid;
    private PlayerActions input;
    private Transform targetPoint;

    [Header("Camera Reaction Shake Properties")]
    public float duration;
    public float strength;
    public int vibrado;
    public float randomness;

    private Tween currentTween;

    void Awake()
    {
        EventManager.CollisionReaction += CameraShake;
    }

    public void Init(Transform parent, Transform chaseTarget)
    {
        transform.SetParent(parent);
        targetPoint = chaseTarget;

        Vector3 newPos = targetPoint.position;
        newPos.z = newPos.z - chaseDistance;
        newPos.y = newPos.y + hoverHeight;

        transform.position = newPos;
    }

    void Update()
    {
        Vector3 newPos = targetPoint.position;
        newPos.z = newPos.z - chaseDistance;
        newPos.y = newPos.y + hoverHeight;

        transform.position = Vector3.Lerp(transform.position, newPos, moveAcceleration);


        TiltEngine(newPos);
    }

    float CalculateHoverHeight(Vector3 target)
    {
        float deltaZ = target.z - transform.position.z;
        float newDeviation = hoverHeightDeviation * (deltaZ / target.z);
        Debug.Log(newDeviation);
        float newHeight = hoverHeight + newDeviation;
        return newHeight;
    }

    void TiltEngine(Vector3 target)
    {
        float deltaX = (target.x - transform.position.x);
        float deltaZ = (target.z - transform.position.z);

        Vector3 direction = target;
        direction.x = deltaX;
        direction.z = deltaZ;
        direction = direction.normalized;
        Quaternion targetRot = Quaternion.Euler(tiltLimit * direction.z, 0, tiltLimit * -direction.x);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, tiltSpeed);
    }

    void CameraShake()
    {
        currentTween = transform.DOShakePosition(duration, strength, vibrado, randomness);
    }
}
