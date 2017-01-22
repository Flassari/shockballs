using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
	public float spawnTimeout;
	public float fuseTimeout;
	public float shockwavePower;
	public Color shockwaveColor;
	public Bomb bombPrefab;
	public Transform spawnPointsContainer;

	private float lastSpawn;

	private List<Transform> availableLocations;

	protected void Start()
	{
		lastSpawn = Time.time;

		availableLocations = new List<Transform>();
		foreach (Transform spawnPoint in spawnPointsContainer)
		{
			availableLocations.Add(spawnPoint);
		}
	}

	protected void Update()
	{
		if (Time.time > lastSpawn + spawnTimeout && availableLocations.Count > 0)
		{
			SpawnBomb();

			lastSpawn = Time.time;
		}
	}

	private void SpawnBomb()
	{
		Transform spawnPoint = availableLocations[Random.Range(0, availableLocations.Count)];
		availableLocations.Remove(spawnPoint);

		Bomb bomb = CreateBombAt(spawnPoint.position.x, spawnPoint.position.z);
		bomb.OnExplode += (Bomb b) => {
			
			availableLocations.Add(spawnPoint);
		};
	}

	Bomb CreateBombAt(float x, float z)
	{
		Bomb bomb = Instantiate(bombPrefab);

		Vector3 pos = new Vector3(x, 0f, z);

		bomb.transform.position = pos;
		bomb.fuseTimeout = fuseTimeout;
		bomb.shockwaveColor = shockwaveColor;
		bomb.shockwavePower = shockwavePower;

		return bomb;
	}

}
