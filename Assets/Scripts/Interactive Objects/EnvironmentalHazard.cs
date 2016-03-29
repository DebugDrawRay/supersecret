using UnityEngine;
using System.Collections;

public class EnvironmentalHazard : MonoBehaviour
{
	public string collisionSound;

    void OnTriggerEnter(Collider hit)
    {
        PlayerController isPlayer = hit.GetComponent<PlayerController>();

		if(isPlayer)
        {
            Destroy(gameObject);
        }
        if(collisionSound != "")
        {
            AkSoundEngine.PostEvent(collisionSound, gameObject);
        }
    }
}
