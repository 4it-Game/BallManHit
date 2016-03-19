using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerMannager : NetworkBehaviour {

	[SyncVar]
	private bool _isDead = false;
	public bool isDead
	{
		get{ return _isDead;}
		protected set {_isDead = value;}
	}

	[SerializeField]
	private int maxHealth = 100;

	[SyncVar]
	private int currentHealth;

	[SerializeField]
	private Behaviour[] disableOnDeath;
	private bool[] wasEnabled;

	public void Setup()
	{
		wasEnabled = new bool[disableOnDeath.Length];
		for (int i = 0; i < wasEnabled.Length; i++) 
		{
			wasEnabled [i] = disableOnDeath [i].enabled;
		}

		SetDefault ();
	}
	/*
	void Update(){
		if (!isLocalPlayer) {
			return;
		}

		if (Input.GetKeyDown(KeyCode.K)) {
			RpcTakeDamage (99999);
		}
	}
	*/

	[ClientRpc]
	public void RpcTakeDamage(int _amount){
		if (isDead) {
			return;
		}

		currentHealth -= _amount;

		Debug.Log (transform.name + " now has " + currentHealth + " health");
	
		if (currentHealth <= 0) {
			Die ();
		}
	}

	private void Die(){
		isDead = true;

		//DISABLE COMPONETS
		for (int i = 0; i < disableOnDeath.Length; i++) {
			disableOnDeath [i].enabled = false;
		}

		Collider _col = GetComponent<Collider> ();
		if (_col != null) {
			_col.enabled = false;
		}

		Debug.Log (transform.name + " is DEAD!");

		//CALL RESPAWN
		StartCoroutine(Respawn());
	}

	private IEnumerator Respawn(){
		yield return new WaitForSeconds (GameMannager.instance.matchSettings.respwanTime);

		SetDefault ();

		Transform _spwanPoint = NetworkManager.singleton.GetStartPosition ();
		transform.position = _spwanPoint.position;
		transform.rotation = _spwanPoint.rotation;

		Debug.Log (transform.name + " Respawnd");
	}

	public void SetDefault()
	{	
		isDead = false;

		currentHealth = maxHealth;

		for (int i = 0; i < disableOnDeath.Length; i++) 
		{
			disableOnDeath [i].enabled = wasEnabled[i];
		}

		Collider _col = GetComponent<Collider> ();
		if (_col != null) {
			_col.enabled = true;
		}
	}
}
