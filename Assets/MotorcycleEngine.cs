using UnityEngine;
using InControl;

public class MotorcycleEngine : MonoBehaviour
{
    [Header("Movement Properties")]
    public float baseSpeed;

    [Header("Tilt Properties")]
    public float tiltRange;
    public float tiltSensitivity;
    public float minTiltForce;
    public AnimationCurve tiltCurve;
    public float recenterForce;

    [Header("Player Body")]
    public GameObject playerSprite;

    //input
    private PlayerActions input;

    //components
    private Rigidbody rigid;

    //test
    private float value;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void Start()
    {
        input = new PlayerActions();

        input.Up.AddDefaultBinding(InputControlType.LeftStickUp);
        input.Down.AddDefaultBinding(InputControlType.LeftStickDown);
        input.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
        input.Right.AddDefaultBinding(InputControlType.LeftStickRight);
    }

    void Update()
    {
        RunEngine();
    }

    void RunEngine()
    {
        Vector3 lateralSpeed = transform.right * baseSpeed;
        Vector3 forwardSpeed = transform.forward * baseSpeed * TiltForce(true);

        float direction = 1;
        float tiltAmount = 0;
        float clampZ = 0;

        float zRot = input.Move.X * tiltSensitivity * TiltForce(true);
        Vector3 spriteRot = new Vector3(0, 0, -zRot);
        Vector3 rot = new Vector3(0, zRot, 0);
        playerSprite.transform.Rotate(spriteRot);
        Vector3 eulers = playerSprite.transform.rotation.eulerAngles;

        if(eulers.z < 180)
        {
            clampZ = Mathf.Clamp(eulers.z, 0, tiltRange);
            direction = -1;
            tiltAmount = eulers.z / tiltRange;
        }
        else if(eulers.z > 180)
        {
            clampZ = Mathf.Clamp(eulers.z, 360 - tiltRange, 360);
            direction = 1;
            tiltAmount = (360 - eulers.z) / tiltRange;
        }

        if (input.Move.IsPressed)
        {
            float moveRot = input.Move.X * TiltForce(false);
            transform.Rotate(new Vector3(0, moveRot, 0) / 5);
            playerSprite.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, clampZ);
        }
        else
        {
            Quaternion zero = Quaternion.Euler(new Vector3(0, eulers.y, 0));
            playerSprite.transform.rotation = Quaternion.RotateTowards(playerSprite.transform.rotation, zero, recenterForce);
            
        }
        rigid.velocity = (direction * lateralSpeed * tiltAmount) + forwardSpeed;
    }

    float TiltForce(bool inverse)
    {
        Vector3 eulers = playerSprite.transform.rotation.eulerAngles;
        float inverseVal = 0;
        if(inverse)
        {
            inverseVal = 1;
        }
        if (eulers.z < 180)
        {
            return tiltCurve.Evaluate(inverseVal - (eulers.z / tiltRange)) + minTiltForce;
        }
        else if (eulers.z > 180)
        {
            return  tiltCurve.Evaluate(inverseVal - ((360 - eulers.z) / tiltRange)) + minTiltForce;
        }
        return 0;
    }
}
