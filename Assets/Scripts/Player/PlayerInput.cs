using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput: MonoBehaviour
{
	[SerializeField]
	private int playerNumber;

	Player player;

	private Rigidbody rb;
	private int button0Index = -1;
	private float chargeStartTime = 0f;

	public void init(int playerNumber, Player player)
	{
		this.playerNumber = playerNumber;
		this.player = player;
	}

	KeyCode GetJoystickButton(int index)
	{
		return (KeyCode)(button0Index + index);
	}

	void Start ()
	{
		button0Index = (int)KeyCode.JoystickButton0 + playerNumber * 20;
	}

	void Update ()
	{
		float x = Input.GetAxis("p" + playerNumber.ToString().ToLower() + " left x");
		float z = Input.GetAxis("p" + playerNumber.ToString().ToLower() + " left y");

		player.Move(x, z);

		if (Input.GetKeyDown(GetJoystickButton(1)))
		{
			// Debug.Log("player " + playerNumber + " pressed button " + GetJoystickButton(1).ToString());
			player.StartCharging();
		}

		if (Input.GetKeyUp(GetJoystickButton(1)))
		{
			// Debug.Log("player " + playerNumber + " pressed button " + GetJoystickButton(1).ToString());
			player.StopCharging();
		}
	}

}
