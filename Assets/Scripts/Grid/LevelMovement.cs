using UnityEngine;

public class LevelMovement : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;
    public float maxAccelTime;

    public Vector3 movementDirection;
    public AnimationCurve accelerationCurve;

    private float accelTime;
    public bool isMoving;

    private Rigidbody rigid;

    private bool topSpeed;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        EventManager.CollisionReaction += ResetSpeed;
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rigid.velocity = movementDirection * CurrentSpeed();
        }

        if (GetNormalizedSpeed() >= 1 && !topSpeed)
        {
            EventManager.TopSpeed();
            topSpeed = true;
        }
    }

    float CurrentSpeed()
    {
        accelTime += Time.deltaTime;
        float time = accelerationCurve.Evaluate(accelTime / maxAccelTime);
        return maxSpeed * time;
        
    }
    void ResetSpeed()
    {
        topSpeed = false;
        accelTime = minSpeed;
    }

    public float GetNormalizedSpeed()
    {
        return accelerationCurve.Evaluate(accelTime / maxAccelTime);
    }
}