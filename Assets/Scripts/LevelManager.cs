using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	public delegate void LevelLoadedDelegate();
	public static LevelLoadedDelegate OnLevelLoaded;

	public static LevelManager current;

	public List<Transform> spawnPoints;

	void Awake()
	{
		current = this;

		if (OnLevelLoaded != null)
			OnLevelLoaded();
	}
}
