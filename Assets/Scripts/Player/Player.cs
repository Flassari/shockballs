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
	private float movementSpeed = 5f;
	[SerializeField]
	private float pushbackForce = 10f;
	[SerializeField]
	private float shockWavePower = 5f;
	[SerializeField]
	private float scaleMassMultiplier = 2f;
	[SerializeField]
	private float speedMassFactorPercent = 50f;
	[SerializeField]
	private float shockWaveMassCost = 2f;
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
		capsuleCollider = GetComponent<CapsuleCollider>();
		Scale(mass);
	}

	public void Init(int playerNumber, Material material)
	{
		this.playerNumber = playerNumber;
		gameObject.layer = LayerMask.NameToLayer("Player" + playerNumber.ToString());
		graphics.GetComponentInChildren<MeshRenderer>().material = material;
		color = GameLogic.instance.playerColors[playerNumber - 1];
		GetComponent<PlayerInput>().Init(playerNumber, this);
		UIManager.instance.playerUIs[playerNumber - 1].sliderFill.color = color;
	}

	void Update()
	{
		var pos = transform.position;
		var margin = 100f;
		var isOutOfBounds = pos.y < -10f || pos.x < -margin || pos.x > margin || pos.z < -margin || pos.z > margin;
		if ((Mathf.Floor (mass) <= 0f) || isOutOfBounds)
		{
			Debug.Log ("Player " + playerNumber + " is out of bounds");
			Die ();
		}

		Scale(mass);
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

			var massSpeedFactor = 1f + ((speedMassFactorPercent / mass) - (speedMassFactorPercent/2f))/100f;
			transform.position += movementSpeed * massSpeedFactor * delta * Time.deltaTime;

			transform.LookAt(transform.position + delta, transform.up);

			graphics.transform.localPosition = new Vector3(0f, Mathf.PingPong(Time.time / 5f, .5f));
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
		if (mass > shockWaveMassCost) {
			ReleaseShockWave (chargeTime);
		}
	}

	void GetHit(float damage, Vector3 pushDirection)
	{
		// Reduce mass and scale the player
		ChangeMass(-damage);
		if (mass < 0f)
		{
			Die ();
			return;
		}

		Vector3 force = pushDirection * pushbackForce;
		rb.AddForce(force, ForceMode.Impulse);

		int collectableAmount = Random.Range(2, 5);
		SpawnCollectables(damage, collectableAmount, pushDirection);
	}

	void SpawnCollectables(float totalMass, float count, Vector3 direction)
	{
		float angle = (Mathf.PI / 5f);
		float startingAngle = Vector3.Angle(transform.position - direction, transform.position + Vector3.forward) - angle * (count / 2f);
		float radius = capsuleCollider.radius + 2f;
		for(int i = 0; i < count; i++)
		{
			float x = transform.position.x + radius * Mathf.Cos(startingAngle + i * angle);
			float z = transform.position.z + radius * Mathf.Sin(startingAngle + i * angle);
			Vector3 collSpawnPos = new Vector3(x, transform.position.y + .1f, z);
			GameObject collectable = Instantiate(collectablePrefab, collSpawnPos, Quaternion.identity);
			Collectable coll = collectable.GetComponent<Collectable>();
			GameLogic.instance.AddCollectable(collectable);
			coll.mass = totalMass / count;
		}
		
	}

	void ChangeMass(float delta)
	{
		Debug.Log(gameObject.name + " mass changed from " + mass + " to " + (mass + delta));
		mass += delta;
	}

	void Scale(float mass)
	{
		float scale = (0.10f + mass / 10f) * scaleMassMultiplier;
		graphics.transform.localScale = new Vector3(scale, scale, scale);
		capsuleCollider.radius = scale / 2f;
	}

	void ReleaseShockWave(float time)
	{
		GameObject shockWaveObj = Instantiate(shockWavePrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
		shockWaveObj.layer = gameObject.layer;
		ShockWave shockWave = shockWaveObj.GetComponent<ShockWave>();
		shockWave.owner = this;
		shockWave.startRadius += capsuleCollider.radius;
		//shockWave.propagationSpeed *= (1 + time);
		shockWave.power = shockWavePower;
		ChangeMass(-shockWaveMassCost);
		shockWave.color = color;
	}

	void Die()
	{
		// animations etc. here
		var droppedMass = Mathf.Max(1f, mass);
		SpawnCollectables(droppedMass, droppedMass, transform.forward);
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
				GetHit(segment.shockWave.power, segment.direction);
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
