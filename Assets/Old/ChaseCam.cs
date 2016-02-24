using UnityEngine;
using System.Collections;

public class ChaseCam : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y + 20, player.transform.position.z - 50);
        transform.position = newPos;
    }
}
