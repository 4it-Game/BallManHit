using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerMannager))]
public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componetsToDisable;

	[SerializeField]
	string remoteLayerName = "RemotePlayer";

	Camera sceneCamera;

	void Start(){
		if (!isLocalPlayer) 
		{
			DisableComponets ();
			AssignRemoteLayer ();
		} 
		else 
		{
			sceneCamera = Camera.main;
			if (sceneCamera != null) 
			{
				sceneCamera.gameObject.SetActive (false);
			}
		}

		GetComponent<PlayerMannager> ().Setup ();
	}

	public override void OnStartClient(){
		base.OnStartClient ();

		string _netID = GetComponent<NetworkIdentity> ().netId.ToString();
		PlayerMannager _player = GetComponent<PlayerMannager> ();

		GameMannager.RegisterPlayer (_netID, _player);
	}

	void AssignRemoteLayer()
	{
		gameObject.layer = LayerMask.NameToLayer (remoteLayerName);
	}

	void DisableComponets()
	{
		for (int i = 0; i < componetsToDisable.Length; i++) {
			componetsToDisable [i].enabled = false;
		}
	}

	void OnDisable(){
		if (sceneCamera != null) {
			sceneCamera.gameObject.SetActive (true);
		}

		GameMannager.UnRegisterPlayer (transform.name);
	}
}
