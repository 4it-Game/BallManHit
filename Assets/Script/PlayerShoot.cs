using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

	private const string PLAYER_TAG = "Player";

	public PlayerWeponScript wepon;

	[SerializeField]
	private Camera cam;

	[SerializeField]
	private LayerMask mask;

	void Start(){
		if (cam == null) {
			Debug.LogError ("Player Shoot: No Camera ref");
			this.enabled = true;
		}
	}

	void Update(){
		if (Input.GetButtonDown("Fire1")) {
			Shoot ();
		}
	}

	[Client]
	void Shoot(){
		RaycastHit hit;
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, wepon.range ,mask)) {
			if (hit.collider.tag == PLAYER_TAG) {
				CmdPlayerShot (hit.collider.name, wepon.damage);
			}
		}
	}

	[Command]
	void CmdPlayerShot(string _playerID, int _damage){
		Debug.Log (_playerID + " has been shot");

		PlayerMannager _player = GameMannager.GetPlayer (_playerID);
		_player.RpcTakeDamage (_damage);
	}
}
