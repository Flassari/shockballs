using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	enum PlayerState { Dead, Spawning, Alive }

	[SerializeField]
	private GameObject graphics;
	[SerializeField]
	public float originalMass = 20f;
	[SerializeField]
	private int playerNumber;
	[SerializeField]
	private float minSpeed = 5;
	[SerializeField]
	private float maxSpeed = 10f;
	[SerializeField]
	private float pushbackForce = 10f;
	[SerializeField]
	private float shockWavePower = 5f;
	[SerializeField]
	private float shockWaveMassCost = 2f;
	[SerializeField]
	private float minScale = 1f;
	[SerializeField]
	private float maxScale = 10f;
	[SerializeField]
	private float minRigidbodyMass = .2f;
	[SerializeField]
	private PlayerState currentState = PlayerState.Spawning;
	[SerializeField]
	private GameObject shockWavePrefab;
	[SerializeField]
	private GameObject collectablePrefab;
	[SerializeField]
	private float mass;

	private float invulnerableDurationRemaining = 0f;
	private const float invulnerableDuration = 0.5f;
	private float flashFramesDuration = 0f;

	[SerializeField]
	private SoundData fallSound;
	[SerializeField]
	private SoundData hitSound;
	[SerializeField]
	private SoundData dieSound;

	[SerializeField]
	private SoundData collectSound;

	private Rigidbody rb;
	private float chargeStartTime = 0f;
	private Color color;
	private CapsuleCollider capsuleCollider;
	private Material defaultMaterial;

	[SerializeField]
	private Bomb bombPrefab;
	[SerializeField]
	private float bombFuseTimeout;
	[SerializeField]
	private float bombShockwavePower;
	[SerializeField]
	private float bombMassCost;
	[SerializeField]
	private Color bombShockwaveColor;

	public int PlayerNumber { get { return playerNumber; } }
	public float Mass { get { return mass; } }

	private Vector3 lastGroundPosition;
	private static float groundCheckInterval = 0.5f;
	private float groundCheckTimer = groundCheckInterval;
	private AudioSource audioSource;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
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
		defaultMaterial = gameObject.GetComponentInChildren<Renderer> ().material;
	}

	void Update()
	{
		var pos = transform.position;
		var margin = 100f;
		var isOutOfBounds = pos.y < -10f || pos.x < -margin || pos.x > margin || pos.z < -margin || pos.z > margin;
		if (isOutOfBounds)
		{
			Debug.Log ("Player " + playerNumber + " is out of bounds");
			if (fallSound != null)
			{
				// fallSound.Play(transform.position);
				audioSource.PlayOneShot((AudioClip)fallSound.sound);
			}
			Die ();
		}

		Scale(mass);

		groundCheckTimer -= Time.deltaTime;
		if (groundCheckTimer <= 0f)
		{
			CheckGroundBelow ();
			groundCheckTimer = groundCheckInterval;
		}

		if (currentState == PlayerState.Spawning)
		{
			// to be idioottivarma: stop sideways movement before you've landed on something
			var yVel = rb.velocity.y;
			rb.velocity.Set(0f, yVel, 0f);
		}

		if (currentState == PlayerState.Spawning || invulnerableDurationRemaining > 0f)
		{
			invulnerableDurationRemaining -= Time.deltaTime;
			flashFramesDuration -= Time.deltaTime;
			if (flashFramesDuration <= 0f)
			{
				gameObject.GetComponentInChildren<Renderer>().enabled = !gameObject.GetComponentInChildren<Renderer>().enabled;
				//gameObject.GetComponentInChildren<Renderer> ().material = null;
				//gameObject.GetComponentInChildren<Renderer> ().material.color = Color.white;
				flashFramesDuration = 0.1f;
			}
		} else
		{
			gameObject.GetComponentInChildren<Renderer> ().material = defaultMaterial;		
			gameObject.GetComponentInChildren<Renderer> ().enabled = true;	
		}


	}

	public bool IsAlive()
	{
		return (currentState == PlayerState.Alive);
	}

	public bool IsDead()
	{
		return (currentState == PlayerState.Dead);
	}

	public void Move(float x, float z)
	{
		if (IsAlive () && GameLogic.instance.CurrentState == GameState.Started)
		{
			float speed = Mathf.Lerp(maxSpeed, minSpeed, mass / 100f) * maxSpeed;
			Vector3 delta = new Vector3 (x, 0, z).normalized;

			if (delta == Vector3.zero)
				return;

			transform.position += delta * speed * Time.deltaTime;

			transform.LookAt(transform.position + delta, transform.up);

			graphics.transform.rotation *= Quaternion.Euler(Mathf.Rad2Deg * Mathf.PI * 2f * Time.deltaTime, 0f, 0f);

			graphics.transform.localPosition = new Vector3(0f, Mathf.PingPong(Time.time / 5f, .5f));
		}
	}

	public void Respawn(Vector3 position)
	{
		currentState = PlayerState.Spawning;
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
			Fire(chargeTime);
		}
	}

	void GetHit(float damage, Vector3 pushDirection)
	{

		if (mass <= 0) return;

		if (invulnerableDurationRemaining <= 0f)
		{
			// Reduce mass and scale the player
			ChangeMass (-damage);

			if (hitSound != null) {
				// Reduce mass and scale the player
				ChangeMass (-damage);

				StartInvul ();

				if (hitSound != null) {
					hitSound.Play (audioSource, transform.position);
				}

				Vector3 force = pushDirection * pushbackForce;
				rb.AddForce (force, ForceMode.Impulse);

				int collectableAmount = 7; //Random.Range(5, 8);
				SpawnCollectables (damage, collectableAmount, pushDirection);
			}
		}
	}

	void SpawnCollectables(float totalMass, float count, Vector3 direction)
	{
		float angle = (Mathf.PI / 5f);
		float startingAngle = Vector3.Angle(transform.position - direction, transform.position + Vector3.forward) - angle * (count / 2f);
		float radius = capsuleCollider.radius + 2f;
		for (int i = 0; i < count; i++)
		{
			float x = transform.position.x + radius * Mathf.Cos(startingAngle + i * angle);
			float z = transform.position.z + radius * Mathf.Sin(startingAngle + i * angle);
			Vector3 collSpawnPos = new Vector3(x, .5f, z);
			GameObject collectable = Instantiate(collectablePrefab, collSpawnPos, Quaternion.identity);
			Collectable coll = collectable.GetComponent<Collectable>();
			GameLogic.instance.AddCollectable(collectable);
			coll.mass = totalMass / count;
			radius += 1f - (2f * Random.value);
		}
	}

	void ChangeMass(float delta)
	{
		mass += delta;
		mass = Mathf.Clamp(mass, 0f, 100f);
		rb.mass = Mathf.Clamp(mass / 100f, minRigidbodyMass, 1f);
	}

	void Scale(float mass)
	{
		float scale = Mathf.Lerp(minScale, maxScale, mass / 100f);
		float graphicsScale = scale;

		float pulsingMinMass = 90f;
		if (mass > pulsingMinMass)
		{
			float pulseFrequency = 6 * Mathf.Pow((mass / pulsingMinMass), 2f);
			float scaleOscillatingFactor = 1.1f + 0.1f * Mathf.Sin (Time.time * pulseFrequency);
			graphicsScale = scale * scaleOscillatingFactor;
		}

		graphics.transform.localScale = new Vector3(graphicsScale, graphicsScale, graphicsScale);
		capsuleCollider.radius = scale / 2f;
	}

	void Fire(float time)
	{
		ReleaseShockWave();
		ChangeMass(-shockWaveMassCost);
	}

	void ReleaseShockWave()
	{
		GameObject shockWaveObj = Instantiate(shockWavePrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
		shockWaveObj.layer = gameObject.layer;
		ShockWave shockWave = shockWaveObj.GetComponent<ShockWave>();
		shockWave.owner = this;
		//shockWave.startRadius += capsuleCollider.radius;
		//shockWave.propagationSpeed *= (1 + time);
		shockWave.power = shockWavePower;
		shockWave.color = color;
	}

	public void AltFire()
	{
		if (mass > bombMassCost)
		{
			Debug.Log("Player " + playerNumber + " dropping bombs");
			ChangeMass (-bombMassCost);

			var hit = new RaycastHit();
			Physics.Raycast(transform.position, Vector3.down, out hit, capsuleCollider.height/2 + 1f);
			if (hit.collider != null && hit.collider.gameObject.tag == "Ground")
			{
				Bomb bomb = Instantiate (bombPrefab);

				Vector3 pos = new Vector3 (this.transform.position.x, 0f, this.transform.position.z);
				bomb.transform.position = pos;
				bomb.fuseTimeout = bombFuseTimeout;
				bomb.shockwaveColor = bombShockwaveColor;
				bomb.shockwavePower = bombShockwavePower;
			}
		}
	}

	void Die()
	{
		if (dieSound != null)
		{
			dieSound.Play(audioSource, transform.position);
		}
		// animations etc. here
		var droppedMass = Mathf.Ceil(Mathf.Max(1f, mass/2f));
		SpawnCollectables(droppedMass, droppedMass, transform.forward);
		currentState = PlayerState.Dead;
	}

	void OnTriggerEnter(Collider col)
	{
		if (currentState == PlayerState.Alive)
		{
			if (col.gameObject.tag == "ShockWave") {
				ShockWaveSegment segment = col.GetComponentInParent<ShockWaveSegment> ();
				if (segment != null) {
					col.gameObject.SetActive (false);
					GetHit (segment.shockWave.power, segment.direction);
				}
			}

			Collectable collectable = col.GetComponent<Collectable> ();
			if (collectable) {
				if (collectSound != null)
				{
					collectSound.Play(audioSource, transform.position);
				}
				ChangeMass (collectable.mass);
				Destroy (collectable.gameObject);
			}
		}
	}

	void OnCollisionEnter(Collision col)
	{
		//Debug.Log ("Collide enter");
		if (currentState == PlayerState.Spawning)
		{
			currentState = PlayerState.Alive;

			StartInvul();
			ReleaseShockWave ();
		}
	}

	void StartInvul()
	{
		invulnerableDurationRemaining = invulnerableDuration;
	}

	void CheckGroundBelow()
	{
		var hit = new RaycastHit();
		Physics.Raycast(transform.position, Vector3.down, out hit, capsuleCollider.height/2 + 1f);
		if (hit.collider != null && hit.collider.gameObject.tag == "Ground")
		{
			lastGroundPosition = hit.point;
		}
	}
}
