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

	public void Init(int playerNumber, Player player)
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
		float x;
		float y;

		bool shootIsDown = false;
		bool shootIsUp = false;

		switch(playerNumber)
		{
			case 1:
				x = Input.GetAxis("p1 left x");
				y = Input.GetAxis("p1 left y");
				shootIsDown = Input.GetKeyDown(KeyCode.Joystick1Button4);
				shootIsUp = Input.GetKeyUp(KeyCode.Joystick1Button4);
				break;
			case 2:
				x = Input.GetAxis("p1 right x");
				y = Input.GetAxis("p1 right y");
				shootIsDown = Input.GetKeyDown(KeyCode.Joystick1Button5);
				shootIsUp = Input.GetKeyUp(KeyCode.Joystick1Button5);
				break;
			case 3:
				x = Input.GetAxis("p2 left x");
				y = Input.GetAxis("p2 left y");
				shootIsDown = Input.GetKeyDown(KeyCode.Joystick2Button4);
				shootIsUp = Input.GetKeyUp(KeyCode.Joystick2Button4);
				break;
			case 4:
				x = Input.GetAxis("p2 right x");
				y = Input.GetAxis("p2 right y");
				shootIsDown = Input.GetKeyDown(KeyCode.Joystick2Button5);
				shootIsUp = Input.GetKeyUp(KeyCode.Joystick2Button5);
				break;
			default:
				return;
		}
		// float x = Input.GetAxis("p" + playerNumber.ToString().ToLower() + " left x");
		// float y = Input.GetAxis("p" + playerNumber.ToString().ToLower() + " left y");

		player.Move(x, y);

		//if (Input.GetKeyDown(GetJoystickButton(1)))
		if (shootIsDown)
		{
			// Debug.Log("player " + playerNumber + " pressed button " + GetJoystickButton(1).ToString());
			player.StartCharging();
		}

		if (shootIsUp)
		{
			// Debug.Log("player " + playerNumber + " pressed button " + GetJoystickButton(1).ToString());
			player.StopCharging();
		}
	}

}
