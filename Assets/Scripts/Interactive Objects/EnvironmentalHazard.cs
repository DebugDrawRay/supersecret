using UnityEngine;
using System.Collections;

public class EnvironmentalHazard : MonoBehaviour
{
	public string collisionSound;

    void OnTriggerEnter(Collider hit)
    {
        PlayerController isPlayer = hit.GetComponent<PlayerController>();

		AkSoundEngine.PostEvent(collisionSound, this.gameObject);
        
		if(isPlayer)
        {
            Destroy(gameObject);
        }
    }
}
