using UnityEngine;
using System.Collections;

public class SyncTest : MonoBehaviour {

	Camera mainCamera;
	QuadCam cameraScript;

	// Use this for initialization
	void Start () {
		mainCamera = FindObjectOfType<Camera>();
		cameraScript = mainCamera.GetComponent<QuadCam>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void beatSync(AkEventCallbackMsg in_callbackInfo) {
		AkCallbackManager.AkMusicSyncCallbackInfo AkEventCallbackInfo = (AkCallbackManager.AkMusicSyncCallbackInfo)in_callbackInfo.info;
		Debug.Log("Beat.");
		cameraScript.CameraShake();

	}

		
}
