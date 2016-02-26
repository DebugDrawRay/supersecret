using UnityEngine;
using System.Collections;

public class LevelMovement : MonoBehaviour
{
    public float maxSpeed;
    public Vector3 movementDirection;

    public bool isMoving;

    private Rigidbody rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isMoving)
        {
            rigid.velocity = movementDirection * maxSpeed;
        }
    }

}