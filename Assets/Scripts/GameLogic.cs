using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

	enum PlayerState { Dead, Alive }
	enum GameState { NotStarted, Started, Paused, Over }

	// Player and players are placeholders 
	class Player
	{
		public PlayerState state;

		public bool IsAlive()
		{
			return this.state.Equals(PlayerState.Alive);
		}
	}
	
	GameState currentState = GameState.NotStarted;
	List<Player> players = new List<Player>();


	// Use this for initialization
	void Start ()
	{
		Switch(currentState, GameState.Started);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (currentState != GameState.Paused) {
			// Check if game should end
			if (IsGameOverConditionReached ()) {
				Switch (currentState, GameState.Over);
			}

			// Check if player should respawn
			players.ForEach (p => RespawnPlayerIfNeeded (p));
		}
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
			break;
		case GameState.Paused:
			break;
		case GameState.Over:
			break;
		}
			
		currentState = newState;
	}


}

