using UnityEngine;
using DG.Tweening;

public class QuadCam : MonoBehaviour
{
    [Header("Camera Movement Properties")]
    public float tiltLimit;
    public float tiltSpeed;

    public float hoverHeight;
    public float hoverHeightDeviation;
    public float chaseDistance;

    public float moveSpeed;
    public float moveAcceleration;

    private Vector3 currentPosition;

    private Camera cam;
    private LevelMovement levelMove;
    private PlayerActions input;
    private Transform targetPoint;

    [Header("Camera Fov Control")]
    public float minFov;
    public float maxFov;
    public float fovSmooth;

    [Header("Camera Reaction Shake Properties")]
    public float duration;
    public float strength;
    public int vibrado;
    public float randomness;

    private Tween currentTween;

    public TransparencySortMode sort;
    void Awake()
    {
        PlayerEventManager.CollisionReaction += CameraShake;
        //EventManager.TopSpeedEvent += MaxFov;
        cam = GetComponent<Camera>();
        cam.transparencySortMode = sort;
    }

    public void Init(Transform parent, Transform chaseTarget)
    {
        transform.SetParent(parent);
        targetPoint = chaseTarget;

        Vector3 newPos = targetPoint.position;
        newPos.z = newPos.z - chaseDistance;
        newPos.y = newPos.y + hoverHeight;

        transform.position = newPos;

        levelMove = Grid.instance.GetComponent<LevelMovement>();
    }

    void Update()
    {
        if (targetPoint.gameObject != null)
        {
            Vector3 newPos = targetPoint.position;
            newPos.z = newPos.z - chaseDistance;
            newPos.y = newPos.y + hoverHeight;

            transform.position = Vector3.Lerp(transform.position, newPos, moveAcceleration);

            TiltEngine(newPos);
            CurrentFov();
        }

    }

    float CalculateHoverHeight(Vector3 target)
    {
        float deltaZ = target.z - transform.position.z;
        float newDeviation = hoverHeightDeviation * (deltaZ / target.z);
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

    public void CameraShake()
    {
        currentTween = transform.DOShakePosition(duration, strength, vibrado, randomness);
        maxFov = 60;
    }

    void MaxFov()
    {
        maxFov = 90;
    }

    void CurrentFov()
    {
        if (levelMove)
        {
            float deltaFov = maxFov - minFov;
            float targetFov = minFov + (deltaFov * levelMove.GetNormalizedSpeed());
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, fovSmooth);
        }
    }
}
