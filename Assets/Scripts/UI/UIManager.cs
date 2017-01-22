using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum UIState { MainMenu, Instructions, Pause, Playing, EndGame }

public class UIManager : MonoBehaviour
{
	public static UIManager instance;

	public PlayerUI[] playerUIs;
	public Text gameOverText;
	public GameObject gameOverAnyKeyTexts;
	public Image winBackgroundImage;
	public Text[] deathTexts;

	[SerializeField]
	private GameObject mainMenuObject;
	[SerializeField]
	private GameObject instructionsObject;
	[SerializeField]
	private GameObject pauseObject;
	[SerializeField]
	private GameObject playingObject;
	[SerializeField]
	private GameObject endGameObject;
	[SerializeField]
	private UIState currentState;
	[SerializeField]
	private GameObject playerUIPrefab;

	void Awake()
	{
		if (instance != null)
			Destroy(this.gameObject);
		else
			instance = this;

		playerUIs = new PlayerUI[4];

		ChangeState(UIState.MainMenu);
	}

	void Update()
	{
		if (Input.anyKeyDown)
		{
			if (currentState == UIState.MainMenu)
				ChangeState(UIState.Instructions);
			if (currentState == UIState.Instructions)
				GameLogic.instance.StartGame();
			if (currentState == UIState.EndGame)
				GameLogic.instance.RestartGame();
		}
	}

	public void ChangeState(UIState newState)
	{
		mainMenuObject.SetActive(false);
		pauseObject.SetActive(false);
		playingObject.SetActive(false);
		endGameObject.SetActive(false);
		instructionsObject.SetActive(false);

		// Exit
		switch(currentState)
		{
			case UIState.MainMenu:
				break;
			case UIState.Pause:
				break;
			case UIState.Playing:
				break;
			case UIState.EndGame:
				break;
		}

		currentState = newState;

		// Enter
		switch(currentState)
		{
			case UIState.MainMenu:
				mainMenuObject.SetActive(true);
				break;
			case UIState.Instructions:
				instructionsObject.SetActive(true);
				break;
			case UIState.Pause:
				pauseObject.SetActive(true);
				break;
			case UIState.Playing:
				playingObject.SetActive(true);
				break;
			case UIState.EndGame:
				endGameObject.SetActive(true);
				break;
		}
	}

	public void CreatePlayerUI(int playerNumber)
	{
		if (playerUIs[playerNumber - 1] != null)
			return;
			
		GameObject uiObj = Instantiate(playerUIPrefab, playingObject.transform);
		uiObj.name = "Player" + playerNumber + "UI";
		RectTransform uiRectTransform = uiObj.GetComponent<RectTransform>();
		uiRectTransform.localPosition = Vector3.zero;
		uiRectTransform.localScale = Vector3.one;
		switch(playerNumber)
		{
			case 1:
				uiRectTransform.anchorMin = new Vector2(0, 1);
				uiRectTransform.anchorMax = new Vector2(0, 1);
				uiRectTransform.pivot = new Vector2(0, 1);
				break;
			case 2:
				uiRectTransform.anchorMin = new Vector2(1, 1);
				uiRectTransform.anchorMax = new Vector2(1, 1);
				uiRectTransform.pivot = new Vector2(1, 1);
				break;
			case 3:
				uiRectTransform.anchorMin = new Vector2(0, 0);
				uiRectTransform.anchorMax = new Vector2(0, 0);
				uiRectTransform.pivot = new Vector2(0, 0);
				break;
			case 4:
				uiRectTransform.anchorMin = new Vector2(1, 0);
				uiRectTransform.anchorMax = new Vector2(1, 0);
				uiRectTransform.pivot = new Vector2(1, 0);
				break;
			default:
				return;
		}

		uiRectTransform.anchoredPosition = Vector2.zero;
		playerUIs[playerNumber - 1] = uiObj.GetComponent<PlayerUI>();
	}
}	
