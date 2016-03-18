using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

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

	void Shoot(){
		RaycastHit hit;
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, wepon.range ,mask)) {
			Debug.Log("We Hit" + hit.collider.name);
		}
	}
}
