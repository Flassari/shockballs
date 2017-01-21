using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bomb : MonoBehaviour
{
	public ShockWave shockwavePrefab;
	public SoundData ExplodeSound;
	public event Action<Bomb> OnExplode;
	
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
		if (fuseTimeout <= 0) return;

		if (Time.time > spawnTime + fuseTimeout)
		{
			Explode();
		}
	}

	public void Explode()
	{
		ShockWave shockWave = Instantiate(shockwavePrefab);
		shockWave.transform.position = transform.position;
		shockWave.color = shockwaveColor;
		shockWave.power = shockwavePower;

		if (ExplodeSound != null)
		{
			ExplodeSound.Play(transform.position);
		}

		Destroy(gameObject);

		if (OnExplode != null)
		{
			OnExplode(this);
		}
	}
}
