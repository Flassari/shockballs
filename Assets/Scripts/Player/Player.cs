using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	enum PlayerState { Dead, Alive }

	[SerializeField]
	private GameObject graphics;
	[SerializeField]
	public float originalMass = 20f;
	[SerializeField]
	private int playerNumber;
	[SerializeField]
	private float movementSpeed = 1f;
	[SerializeField]
	private float pushbackForce = 10f;
	[SerializeField]
	private float shockWavePower = 5f;
	[SerializeField]
	private float scaleMassMultiplier = 2f;
	[SerializeField]
	private PlayerState currentState = PlayerState.Alive;
	[SerializeField]
	private GameObject shockWavePrefab;
	[SerializeField]
	private GameObject collectablePrefab;
	[SerializeField]
	private float mass;

	private Rigidbody rb;
	private float chargeStartTime = 0f;
	private Color color;
	private CapsuleCollider capsuleCollider;
	
	public int PlayerNumber { get { return playerNumber; } }
	public float Mass { get { return mass; } }

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		mass = originalMass;
		color = graphics.GetComponent<MeshRenderer>().material.color;
		capsuleCollider = GetComponent<CapsuleCollider>();
		Scale(mass);
	}

	public void Init(int playerNumber, Material material)
	{
		this.playerNumber = playerNumber;
		gameObject.layer = LayerMask.NameToLayer("Player" + playerNumber.ToString());
		graphics.GetComponent<MeshRenderer>().material = material;
		GetComponent<PlayerInput>().Init(playerNumber, this);
	}

	void Update()
	{
		if (Mathf.Floor (mass) <= 0f || transform.position.y < -10f)
		{
			currentState = PlayerState.Dead;
		}
	}

	public bool IsAlive()
	{
		return (currentState == PlayerState.Alive);
	}

	public void Move(float x, float z)
	{
		if (this.IsAlive () && GameLogic.instance.CurrentState == GameState.Started)
		{
			Vector3 delta = new Vector3 (x, 0, z);

			transform.position += delta * movementSpeed * Time.deltaTime;
		}
	}

	public void Respawn(Vector3 position)
	{
		currentState = PlayerState.Alive;
		transform.position = position;
		mass = originalMass;
		Scale(mass);
		rb.velocity = Vector3.zero;
	}

	public void StartCharging() 
	{
		// Debug.Log("player " + " started charging");
		chargeStartTime = Time.time;
	}

	public void StopCharging()
	{
		float chargeTime = Time.time - chargeStartTime;
		// Debug.Log("player " + " charged for " + chargeTime + " seconds");
		ReleaseShockWave(chargeTime);
	}

	void GetHit(float damage)
	{
		// Reduce mass and scale the player
		ChangeMass(-damage);
		if (mass < 0f)
		{
			Die ();
			return;
		}

		float angle = Mathf.PI * 2f / 5f;
		float radius = 4f;
		for(int i = 0; i < 5; i++)
		{
			float x = transform.position.x + radius * Mathf.Cos(angle * i);
			float z = transform.position.z + radius * Mathf.Sin(angle * i);
			Vector3 collSpawnPos = new Vector3(x, transform.position.y + .1f, z);
			GameObject collectable = Instantiate(collectablePrefab, collSpawnPos, Quaternion.identity);
			Collectable coll = collectable.GetComponent<Collectable>();
			GameLogic.instance.AddCollectable(collectable);
			coll.mass = damage / 5f;
		}
	}

	void ChangeMass(float delta)
	{
		mass += delta;
		Scale(mass);
	}

	void Scale(float mass)
	{
		float scale = (1f + mass / 100f) * scaleMassMultiplier;
		graphics.transform.localScale = new Vector3(scale, scale, scale);
		capsuleCollider.radius = scale / 2f;
	}

	void ReleaseShockWave(float time)
	{
		GameObject shockWaveObj = Instantiate(shockWavePrefab, transform.position, Quaternion.identity);
		shockWaveObj.layer = gameObject.layer;
		ShockWave shockWave = shockWaveObj.GetComponent<ShockWave>();
		shockWave.owner = this;
		//shockWave.propagationSpeed *= (1 + time);
		shockWave.power = shockWavePower;
		ChangeMass(-shockWave.power);
		shockWave.color = color;
	}

	void Die()
	{
		// animations etc. here
		currentState = PlayerState.Dead;
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "ShockWave")
		{
			ShockWaveSegment segment = col.GetComponentInParent<ShockWaveSegment>();
			if (segment != null)
			{
				col.gameObject.SetActive(false);
				Vector3 force = segment.direction * pushbackForce;
				rb.AddForce(force, ForceMode.Impulse);
				GetHit(segment.shockWave.power);
			}
		}

		Collectable collectable = col.GetComponent<Collectable>();
		if (collectable)
		{
			ChangeMass(collectable.mass);
			Destroy(collectable.gameObject);
		}
	}
}
