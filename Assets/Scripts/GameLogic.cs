using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { NotStarted, Started, Paused, Over }

public class GameLogic : MonoBehaviour
{
	GameState currentState = GameState.NotStarted;
	List<Player> players = new List<Player>();

	[SerializeField]
	private GameObject playerPrefab;

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
		if (currentState != GameState.Paused && currentState != GameState.NotStarted) {
			// Check if game should end
			if (IsGameOverConditionReached ()) {
				Switch (currentState, GameState.Over);
			}

			// Check if player should respawn
			players.ForEach (p => RespawnPlayerIfNeeded (p));
		}
	}

	void CreatePlayer(int playerNumber, Vector3 position)
	{
		GameObject playerObj = Instantiate(playerPrefab, position, Quaternion.identity);
		playerObj.name = "Player" + playerNumber;
		Player player = playerObj.GetComponent<Player>();
		player.Init(playerNumber);
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
					i => CreatePlayer(i, LevelManager.current.spawnPoints[i - 1].position)
				);
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

