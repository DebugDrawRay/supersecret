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
    }

    float CurrentSpeed()
    {
        accelTime += Time.deltaTime;
        float time = accelerationCurve.Evaluate(accelTime / maxAccelTime);
        return maxSpeed * time;
        
    }
    void ResetSpeed()
    {
        accelTime = minSpeed;
    }

    public float GetNormalizedSpeed()
    {
        return accelerationCurve.Evaluate(accelTime / maxAccelTime);
    }
}