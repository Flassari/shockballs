using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { NotStarted, Started, Paused, Over }

public class GameLogic : MonoBehaviour
{
	static public GameLogic instance;

	public Color[] playerColors = new Color[4];
	
	GameState currentState = GameState.NotStarted;
	Player[] players = new Player[4];

	[SerializeField]
	private GameObject playerPrefab;
	[SerializeField]
	private Material[] playerMaterials;

	private GameObject collectableParent = null;
	private int[] playerDeathCounts = new int[4];

	public GameState CurrentState { get { return currentState; } }

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
			return;
		}

		instance = this;
	}

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
		players[playerNumber - 1] = player;
		UIManager.instance.CreatePlayerUI(playerNumber);
		UIManager.instance.playerUIs[playerNumber - 1].playerNameText.text = "Player " + playerNumber;
		player.Init(playerNumber, playerMaterials[playerNumber - 1]);
	}

	bool IsGameOverConditionReached()
	{
		foreach(var player in players)
		{
			if (player.Mass >= 100f)
				return true;
		}

		return false;
	}

	void RespawnPlayerIfNeeded(Player player)
	{
		if (player.IsDead())
		{
			// set player state to not charging
			// set size to default
			// set speed to 0
			// set position/spawn new object
			playerDeathCounts[player.PlayerNumber - 1]++;
			UIManager.instance.playerUIs[player.PlayerNumber - 1].deathCountText.text = "Deaths: " + playerDeathCounts[player.PlayerNumber - 1];
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
				string winningPlayerName = "";
				foreach(var player in players)
				{
					if (player.Mass >= 100f)
						winningPlayerName = player.gameObject.name;
				}
				UIManager.instance.gameOverText.text = winningPlayerName + " WON";
				break;
		}
		
		currentState = newState;
	}

	public void AddCollectable(GameObject collectable)
	{
		if (collectableParent == null)
			collectableParent = new GameObject("CollectableParent");
		
		collectable.transform.SetParent(collectableParent.transform, true);
	}

	public void StartGame()
	{
		SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
	}

	public void RestartGame()
	{
		if (collectableParent != null)
			Destroy(collectableParent);

		playerDeathCounts = new int[4];
		
		foreach(var player in players)
		{
			UIManager.instance.playerUIs[player.PlayerNumber - 1].deathCountText.text = "Deaths: 0";
			player.Respawn(LevelManager.current.spawnPoints[player.PlayerNumber - 1].position);
		}

		Switch(currentState, GameState.NotStarted);
		SceneManager.UnloadSceneAsync("Level1");
		StartGame();
	}
}

