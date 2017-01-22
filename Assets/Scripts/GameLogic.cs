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
	private float gameEndTime = 0f;
	private string currentLevelName = "Level1";

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
			if (IsGameOverConditionReached () && currentState != GameState.Over) {
				Switch (currentState, GameState.Over);
			}

			// Update check on players
			foreach(var player in players)
			{
				RespawnPlayerIfNeeded(player);
				UIManager.instance.playerUIs[player.PlayerNumber - 1].playerMassSlider.value = player.Mass;
			}
		}

		if (currentState == GameState.Over)
		{
			if ((Time.time - gameEndTime) >= 3f)
			{
				UIManager.instance.gameOverAnyKeyTexts.SetActive(true);
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
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
				int winningPlayerIndex = -1;
				foreach(var player in players)
				{
					int playerIndex = player.PlayerNumber - 1;

					if (player.Mass >= 100f)
					{
						winningPlayerName = "Player " + player.PlayerNumber;
						winningPlayerIndex = playerIndex;
						player.particleSystem.Emit (100);
					}
					
					UIManager.instance.deathTexts[playerIndex].text = "Player " + 
						player.PlayerNumber + " deaths: " + playerDeathCounts[playerIndex];

					UIManager.instance.deathTexts[playerIndex].color = playerColors[playerIndex];

					player.gameObject.SetActive(false);
				}
				UIManager.instance.gameOverText.text = winningPlayerName + " wins";
				UIManager.instance.winBackgroundImage.color = playerColors[winningPlayerIndex];
				UIManager.instance.gameOverAnyKeyTexts.SetActive(false);
				gameEndTime = Time.time;
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
		SceneManager.LoadScene(currentLevelName, LoadSceneMode.Additive);
	}

	public void RestartGame()
	{
		if ((Time.time - gameEndTime) < 3f)
		{
			return;
		}

		if (collectableParent != null)
			Destroy(collectableParent);

		foreach(var bomb in FindObjectsOfType<Bomb>())
		{
			Destroy(bomb.gameObject);
		}

		playerDeathCounts = new int[4];
		
		Switch(currentState, GameState.NotStarted);
		StartCoroutine(RestartGameRoutine());
	}

	IEnumerator RestartGameRoutine()
	{
		var async = SceneManager.UnloadSceneAsync(currentLevelName);
		if (currentLevelName == "Level1")
			currentLevelName = "Level2";
		else
			currentLevelName = "Level1";
			
		while (!async.isDone)
		{
			yield return 0;
		}

		var loadAsync = SceneManager.LoadSceneAsync(currentLevelName, LoadSceneMode.Additive);

		while (!loadAsync.isDone)
		{
			yield return 0;
		}

		Switch(currentState, GameState.Started);

		foreach(var player in players)
		{
			player.gameObject.SetActive(true);
			UIManager.instance.playerUIs[player.PlayerNumber - 1].deathCountText.text = "Deaths: 0";
			player.Respawn(LevelManager.current.spawnPoints[player.PlayerNumber - 1].position);
		}
	}
}

