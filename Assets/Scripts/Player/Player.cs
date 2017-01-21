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
	private PlayerState currentState = PlayerState.Alive;
	[SerializeField]
	private GameObject shockWavePrefab;
	[SerializeField]
	private GameObject collectablePrefab;

	private Rigidbody rb;
	private float chargeStartTime = 0f;
	private float mass;
	
	public int PlayerNumber { get { return playerNumber; } }
	public float Mass { get { return mass; } }

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		mass = originalMass;
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
		if (Mathf.Floor (mass) == 0f)
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
		if (this.IsAlive ())
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
		mass -= damage;
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
			coll.mass = damage / 5f;
		}

		Scale(mass);
	}

	void Scale(float mass)
	{
		float scale = 1f + mass / 100f;
		transform.localScale = new Vector3(scale, scale, scale);
	}

	void ReleaseShockWave(float time)
	{
		GameObject shockWaveObj = Instantiate(shockWavePrefab, transform.position, Quaternion.identity);
		shockWaveObj.layer = gameObject.layer;
		ShockWave shockWave = shockWaveObj.GetComponent<ShockWave>();
		shockWave.owner = this;
		//shockWave.propagationSpeed *= (1 + time);
		shockWave.power = 5f;
	}

	void Die()
	{
		// animations etc. here
		currentState = PlayerState.Dead;
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer("Death"))
		{
			currentState = PlayerState.Dead;
		}

		// Debug.Log("player " + playerNumber + " collided with " + col.gameObject.name + " of layer " + LayerMask.LayerToName(col.gameObject.layer));
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
			mass += collectable.mass;
			Destroy(collectable.gameObject);
		}
	}
}
