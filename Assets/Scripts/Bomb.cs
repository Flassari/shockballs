using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bomb : MonoBehaviour
{
	public ShockWave shockwavePrefab;
	[SerializeField]
	private GameObject collectablePrefab;
	public GameObject ExplodeSoundPrefab;
	public GameObject explodePrefab;
	public event Action<Bomb> OnExplode;

	public float bounceHeight;
	public float bounceSpeed;

	private float originalY;

	[HideInInspector] public float fuseTimeout;
	[HideInInspector] public float shockwavePower;
	[HideInInspector] public Color shockwaveColor;

	private float spawnTime;
	private AudioSource audioSource;

	private bool isExploded;

	protected void Start()
	{
		audioSource = GetComponent<AudioSource>();
		spawnTime = Time.time;
		Vector3 prevRotation = transform.localRotation.eulerAngles;
		prevRotation.y = UnityEngine.Random.Range(0f, 360f);
		transform.localRotation = Quaternion.Euler(prevRotation);
		originalY = transform.localPosition.y;
	}

	protected void Update()
	{
		transform.localPosition = new Vector3(transform.localPosition.x, originalY + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight, transform.localPosition.z);
		
		if (fuseTimeout <= 0) return;

		if (Time.time > spawnTime + fuseTimeout)
		{
			Explode();
		}
	}

	public void Explode()
	{
		if (isExploded) return;

		isExploded = true;
		ShockWave shockWave = Instantiate(shockwavePrefab);
		shockWave.transform.position = transform.position;
		shockWave.color = shockwaveColor;
		shockWave.power = shockwavePower;

		Instantiate(ExplodeSoundPrefab, transform.position, Quaternion.identity);
		Instantiate(explodePrefab, transform.position, Quaternion.identity); 

		SpawnCollectables(20f, 7, Vector3.zero);
		Destroy(gameObject);

		if (OnExplode != null)
		{
			OnExplode(this);
		}
	}

	private 
	void SpawnCollectables(float totalMass, float count, Vector3 direction)
	{
		float angle = (Mathf.PI / 5f);
		float startingAngle = Vector3.Angle(transform.position - direction, transform.position + Vector3.forward) - angle * (count / 2f);
		float radius = 2f;
		for (int i = 0; i < count; i++)
		{
			float x = transform.position.x + radius * Mathf.Cos(startingAngle + i * angle);
			float z = transform.position.z + radius * Mathf.Sin(startingAngle + i * angle);
			Vector3 collSpawnPos = new Vector3(x, .5f, z);
			GameObject collectable = Instantiate(collectablePrefab, collSpawnPos, Quaternion.identity);
			Collectable coll = collectable.GetComponent<Collectable>();
			GameLogic.instance.AddCollectable(collectable);
			coll.mass = totalMass / count;
			radius += 1f - (2f * UnityEngine.Random.value);
		}
	}
}
