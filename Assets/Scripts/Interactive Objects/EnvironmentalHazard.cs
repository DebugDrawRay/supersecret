using UnityEngine;
using System.Collections;

public class EnvironmentalHazard : MonoBehaviour
{
    void OnTriggerEnter(Collider hit)
    {
        PlayerController isPlayer = hit.GetComponent<PlayerController>();
		AkSoundEngine.PostEvent("TB_collisionMetal", this.gameObject);
        if(isPlayer)
        {
            Destroy(gameObject);
        }
    }
}
