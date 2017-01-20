using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {
	
	enum GameState { NotStarted, Started, Paused, Over }

	GameState currentState = GameState.NotStarted;
	List<Player> players = new List<Player>();

	[SerializeField]
	private GameObject playerPrefab;


	// Use this for initialization
	void Start ()
	{
		Debug.Log("Start game logic loop");
		Switch(currentState, GameState.Started);
	}
	
	// Update is called once per frame
	void Update ()
	{
		Debug.Log ("Update game logic loop");
		if (currentState != GameState.Paused) {
			// Check if game should end
			if (IsGameOverConditionReached ()) {
				Switch (currentState, GameState.Over);
			}

			// Check if player should respawn
			players.ForEach (p => RespawnPlayerIfNeeded (p));
		}
	}

	void CreatePlayer(int playerNumber)
	{
		Vector3 v3 = new Vector3 (10, 10, 0 + 1*playerNumber);
		GameObject playerObj = Instantiate(playerPrefab, v3, Quaternion.identity);
		PlayerInput playerInput = playerObj.GetComponent<PlayerInput>();
		Player player = playerObj.GetComponent<Player>();
		playerInput.init(playerNumber, player);
	}

	bool IsGameOverConditionReached()
	{
		return !players.Exists (p => p.IsAlive ());
	}

	void RespawnPlayerIfNeeded(Player player)
	{
		if (player.IsAlive())
		{
			// set player state to not charging
			// set size to default
			// set speed to 0
			// set position/spawn new object
		}
	}

	void Switch(GameState oldState, GameState newState)
	{
		switch (newState)
		{
		case GameState.Started:
			// instantiate four players
			List<int> playercounts = new List<int> (new int[] { 1, 2, 3, 4 });
			playercounts.ForEach (
				i => CreatePlayer(i)
			);
			break;	
		case GameState.Paused:
			break;
		case GameState.Over:
			break;
		}
			
		currentState = newState;
	}


}

