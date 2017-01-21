﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { NotStarted, Started, Paused, Over }

public class GameLogic : MonoBehaviour
{
	GameState currentState = GameState.NotStarted;
	Player[] players = new Player[4];

	[SerializeField]
	private GameObject playerPrefab;
	[SerializeField]
	private Material[] playerMaterials;

	// Use this for initialization
	void Start ()
	{
		LevelManager.OnLevelLoaded += OnLevelLoaded;
	}

	void OnLevelLoaded()
	{
		Switch(currentState, GameState.Started);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (currentState != GameState.Paused && currentState != GameState.NotStarted)
		{
			// Check if game should end
			if (IsGameOverConditionReached ()) {
				Switch (currentState, GameState.Over);
			}

			// Update check on players
			foreach(var player in players)
			{
				RespawnPlayerIfNeeded(player);
				UIManager.instance.playerUIs[player.PlayerNumber - 1].playerMassSlider.value = player.Mass;
			}
		}
	}

	void CreatePlayer(int playerNumber, Vector3 position)
	{
		if (players[playerNumber - 1] != null)
			return;

		GameObject playerObj = Instantiate(playerPrefab, position, Quaternion.identity);
		playerObj.name = "Player" + playerNumber;
		Player player = playerObj.GetComponent<Player>();
		player.Init(playerNumber, playerMaterials[playerNumber - 1]);
		players[playerNumber - 1] = player;
		UIManager.instance.CreatePlayerUI(playerNumber);
		UIManager.instance.playerUIs[playerNumber - 1].playerNameText.text = "Player " + playerNumber;
	}

	bool IsGameOverConditionReached()
	{
		// return !players.Exists (p => p.Mass >= 100f);
		return false;
	}

	void RespawnPlayerIfNeeded(Player player)
	{
		if (!player.IsAlive())
		{
			// set player state to not charging
			// set size to default
			// set speed to 0
			// set position/spawn new object
			player.Respawn(LevelManager.current.spawnPoints[player.PlayerNumber - 1].position);
		}
	}

	void Switch(GameState oldState, GameState newState)
	{
		switch (newState)
		{
			case GameState.Started:
				// instantiate four players
				for(int i = 0; i < 4; i++)
				{
					CreatePlayer(i + 1, LevelManager.current.spawnPoints[i].position);
				}

				UIManager.instance.ChangeState(UIState.Playing);
				break;	
			case GameState.Paused:
				UIManager.instance.ChangeState(UIState.Pause);
				break;
			case GameState.Over:
				UIManager.instance.ChangeState(UIState.EndGame);
				break;
		}
		
		currentState = newState;
	}

	public void StartGame()
	{
		SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
	}
}

