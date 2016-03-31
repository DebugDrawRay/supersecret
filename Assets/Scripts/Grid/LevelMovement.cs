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
        PlayerEventManager.HitReaction += ResetSpeed;
        AkSoundEngine.PostEvent("TB_engineStart", gameObject);
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rigid.velocity = movementDirection * CurrentSpeed();
            AkSoundEngine.SetRTPCValue("TB_Speed", GetNormalizedSpeed());
        }
    }

    void Update()
    {
        if (GetNormalizedSpeed() >= 1 && !topSpeed)
        {
            //PlayerEventManager.TopSpeed();
            topSpeed = true;
        }
    }

    float CurrentSpeed()
    {
        accelTime += Time.fixedDeltaTime;
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