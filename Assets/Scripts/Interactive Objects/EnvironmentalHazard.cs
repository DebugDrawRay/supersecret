using UnityEngine;
using System.Collections;

public class EnvironmentalHazard : MonoBehaviour
{
    void OnTriggerEnter(Collider hit)
    {
        PlayerController isPlayer = hit.GetComponent<PlayerController>();
        if(isPlayer)
        {
            Destroy(gameObject);
        }
    }
}
