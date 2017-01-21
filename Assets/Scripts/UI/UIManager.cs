using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum UIState { MainMenu, Pause, Playing, EndGame }

public class UIManager : MonoBehaviour
{
	public static UIManager instance;


	[SerializeField]
	private GameObject mainMenuObject;
	[SerializeField]
	private GameObject pauseObject;
	[SerializeField]
	private GameObject playingObject;
	[SerializeField]
	private GameObject endGameObject;
	[SerializeField]
	private UIState currentState;

	void Awake()
	{
		if (instance != null)
			Destroy(this.gameObject);
		else
			instance = this;

		ChangeState(UIState.MainMenu);
	}

	public void ChangeState(UIState newState)
	{
		mainMenuObject.SetActive(false);
		pauseObject.SetActive(false);
		playingObject.SetActive(false);
		endGameObject.SetActive(false);

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
}	
