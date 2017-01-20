using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	enum PlayerState { Dead, Alive }

	[SerializeField]
	private int playerNumber;
	[SerializeField]
	private float movementSpeed = 1f;
	[SerializeField]
	private PlayerState currentState = PlayerState.Alive;
	[SerializeField]
	private GameObject shockWavePrefab;

	private Rigidbody rb;
	private int button0Index = -1;
	private float chargeStartTime = 0f;

	KeyCode GetJoystickButton(int index)
	{
		return (KeyCode)(button0Index + index);
	}

	void Start ()
	{
		rb = GetComponent<Rigidbody>();

		button0Index = (int)KeyCode.JoystickButton0 + playerNumber * 20;

		gameObject.layer = LayerMask.NameToLayer("Player" + playerNumber);
	}

	void Update ()
	{
		if (currentState == PlayerState.Alive)
		{
			float x = Input.GetAxis("p" + playerNumber.ToString().ToLower() + " left x");
			float z = Input.GetAxis("p" + playerNumber.ToString().ToLower() + " left y");

			Vector3 delta = new Vector3(x, 0, z);

			transform.position += delta * movementSpeed * Time.deltaTime;

			if (Input.GetKeyDown(GetJoystickButton(1)))
			{
				// Debug.Log("player " + playerNumber + " pressed button " + GetJoystickButton(1).ToString());
				Debug.Log("player " + playerNumber + " started charging");
				chargeStartTime = Time.time;
			}

			if (Input.GetKeyUp(GetJoystickButton(1)))
			{
				// Debug.Log("player " + playerNumber + " pressed button " + GetJoystickButton(1).ToString());
				float chargeTime = Time.time - chargeStartTime;
				Debug.Log("player " + playerNumber + " charged for " + chargeTime + " seconds");
				ReleaseShockWave(chargeTime);
			}
		}
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
	}

	void OnTriggerEnter(Collider col)
	{
		Debug.Log("OnTriggerEnter - " + gameObject.name + " collided with " + col.gameObject.name);

		if (LayerMask.LayerToName(col.gameObject.layer).Contains("Player") &&
			col.gameObject.layer != gameObject.layer)
		{
			col.gameObject.SetActive(false);
			Vector3 force = transform.position - col.transform.position;
			rb.AddForce(force * 5f, ForceMode.Impulse);
		}
	}
}
