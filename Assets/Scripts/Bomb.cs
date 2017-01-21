using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	public ShockWave shockwavePrefab;
	
	[HideInInspector] public float fuseTimeout;
	[HideInInspector] public float shockwavePower;
	[HideInInspector] public Color shockwaveColor;

	private float spawnTime;

	protected void Start()
	{
		spawnTime = Time.time;
	}

	protected void Update()
	{
		if (Time.time > spawnTime + fuseTimeout)
		{
			ShockWave shockWave = Instantiate(shockwavePrefab);
			shockWave.transform.position = transform.position;
			shockWave.color = shockwaveColor;
			shockWave.power = shockwavePower;
			Destroy(gameObject);
		}
	}
}
