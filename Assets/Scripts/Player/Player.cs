using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	enum PlayerState { Dead, Alive }

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

	private Rigidbody rb;
	private float chargeStartTime = 0f;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	public void Init(int playerNumber)
	{
		this.playerNumber = playerNumber;
		gameObject.layer = LayerMask.NameToLayer("Player" + playerNumber.ToString());
		GetComponent<PlayerInput>().Init(playerNumber, this);
	}

	void Update()
	{
		//
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

	void ReleaseShockWave(float time)
	{
		GameObject shockWaveObj = Instantiate(shockWavePrefab, transform.position, Quaternion.identity);
		shockWaveObj.layer = gameObject.layer;
		ShockWave shockWave = shockWaveObj.GetComponent<ShockWave>();
		shockWave.propagationSpeed *= (1 + time);
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
			col.gameObject.SetActive(false);
			Vector3 force = transform.position - col.transform.position;
			rb.AddForce(force * pushbackForce, ForceMode.Impulse);
		}
	}
}
