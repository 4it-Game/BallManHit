using UnityEngine;
using System.Collections.Generic;

public class GameMannager : MonoBehaviour {

	public static GameMannager instance;

	public MatchSettings matchSettings;

	void Awake(){
		if (instance != null) {
			Debug.LogError ("More than one GameMannager in scene.");
		} 
		else 
		{
			instance = this;
		}
	}

	#region Player Tracking

	private const string PLAYER_ID_PREFIX = "Player";

	private static Dictionary<string, PlayerMannager> players = new Dictionary<string, PlayerMannager>();

	public static void RegisterPlayer(string _netID, PlayerMannager _player)
	{
		string _playerID = PLAYER_ID_PREFIX + _netID;
		players.Add (_playerID, _player);
		_player.transform.name = _playerID;
	}

	public static void UnRegisterPlayer(string _playerID){
		players.Remove (_playerID);
	}

	public static PlayerMannager GetPlayer(string _playerID){
		return players[_playerID];
	}

	void OnGUI(){
		GUILayout.BeginArea (new Rect(20,150,200,500));
		GUILayout.BeginVertical ();

		foreach(string _playerID in players.Keys){
			GUILayout.Label (_playerID + " - " + players[_playerID].transform.name);
		}

		GUILayout.EndVertical ();
		GUILayout.EndArea ();
	}

	#endregion
}
