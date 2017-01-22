using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
	public float startingSpawnTimeout;
	public float fuseTimeout;
	public float shockwavePower;
	public AnimationCurve maxBombCountCurve;
	public int maxGameTimeSeconds = 180;
	public Color shockwaveColor;
	public Bomb bombPrefab;
	public Transform spawnPointsContainer;

	public int startingMaxBombCount;
	private int maxBombCount;
	private float startingTime;
	private float lastSpawn;
	private int bombCount = 0;

	private List<Transform> availableLocations;

	protected void Start()
	{
		startingTime = Time.time;
		lastSpawn = Time.time;

		availableLocations = new List<Transform>();
		foreach (Transform spawnPoint in spawnPointsContainer)
		{
			availableLocations.Add(spawnPoint);
		}
	}

	protected void Update()
	{
		var normalizedTime = (Time.time - startingTime) / maxGameTimeSeconds;
		var scaling = maxBombCountCurve.Evaluate (normalizedTime);
		maxBombCount = startingMaxBombCount + Mathf.FloorToInt(10f * scaling);

		var spawnTimeout = startingSpawnTimeout *(0.5f - scaling);
		//Debug.Log("Normalized time: " + normalizedTime.ToString() + " scaling " + scaling + " max bomb count " + maxBombCount + " timeout " + spawnTimeout);
		if (Time.time > lastSpawn + spawnTimeout && availableLocations.Count > 0)
		{
			SpawnBomb();

			lastSpawn = Time.time;
		}
	}

	private void SpawnBomb()
	{
		if (bombCount >= maxBombCount)
			return;

		Transform spawnPoint = availableLocations[Random.Range(0, availableLocations.Count)];
		availableLocations.Remove(spawnPoint);

		bombCount++;

		Bomb bomb = CreateBombAt(spawnPoint.position.x, spawnPoint.position.z);
		bomb.OnExplode += (Bomb b) => {
			
			availableLocations.Add(spawnPoint);
			bombCount--;
		};
	}

	Bomb CreateBombAt(float x, float z)
	{
		Bomb bomb = Instantiate(bombPrefab);

		Vector3 pos = new Vector3(x, bombPrefab.transform.position.y, z);

		bomb.transform.position = pos;
		bomb.fuseTimeout = fuseTimeout;
		bomb.shockwaveColor = shockwaveColor;
		bomb.shockwavePower = shockwavePower;

		return bomb;
	}

}
