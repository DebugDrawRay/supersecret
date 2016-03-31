using UnityEngine;
using System.Collections;

public class LaserBlast : MonoBehaviour {

	private PlayerActions input;

	uint switchGroup = 3893417221U;
	uint currentSwitch;
	string currentSwitchString;

	// Use this for initialization
	void Awake () {
		input = PlayerActions.BindKeyboardAndJoystick();
	}
	
	// Update is called once per frame
	void Update () {

		if (input.Debug_MusicWeapon.WasPressed) 
		{

			AkSoundEngine.GetSwitch(switchGroup, GameController.instance.gameObject, out currentSwitch);
			Debug.Log(AkSoundEngine.GetSwitch(switchGroup, GameController.instance.gameObject, out currentSwitch));
			Debug.Log(currentSwitch);

			switch(currentSwitch)
			{
			case 930712164U:
				currentSwitchString = "Off";
				break;
			case 3982605422U:
				currentSwitchString = "Laser";
				break;
			}

			Debug.Log(currentSwitchString);
			if (currentSwitchString == "Off")
			{
				AkSoundEngine.SetSwitch("Weapon","Laser",GameController.instance.gameObject);
			} 
			else 
			{
				AkSoundEngine.SetSwitch("Weapon","Off",GameController.instance.gameObject);
			}
		}
	
	}

	void beatSync(AkEventCallbackMsg in_callbackInfo) {
		AkCallbackManager.AkMusicSyncCallbackInfo AkEventCallbackInfo = (AkCallbackManager.AkMusicSyncCallbackInfo)in_callbackInfo.info;
		Debug.Log("Beat.");

	}
}
