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

	protected void Start()
	{
		lastSpawn = Time.time;
	}

	protected void Update()
	{
		if (Time.time > lastSpawn + spawnTimeout)
		{
			Bomb bomb = Instantiate(bombPrefab);
			bomb.transform.position = spawnPointsContainer.GetChild(Random.Range(0, spawnPointsContainer.childCount)).transform.position;
			bomb.fuseTimeout = fuseTimeout;
			bomb.shockwaveColor = shockwaveColor;
			bomb.shockwavePower = shockwavePower;

			lastSpawn = Time.time;
		}
	}
}
