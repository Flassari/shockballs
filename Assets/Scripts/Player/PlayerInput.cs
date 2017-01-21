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

	private string axisX;
	private string axisY;
	private KeyCode fireButton;

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

		SetupControls();
	}

	void SetupControls()
	{
		if (playerNumber == 1) {
			axisX = "p1 left x";
			axisY = "p1 left y";
			fireButton = KeyCode.Joystick3Button4;
		} else if (playerNumber == 2) {
			axisX = "p1 left x";
			axisY = "p1 left y";
			fireButton = KeyCode.Joystick3Button5;
		} else {
			// right stick different on Mac: see http://wiki.unity3d.com/index.php?title=Xbox360Controller
			switch (SystemInfo.operatingSystemFamily)
			{
				case OperatingSystemFamily.Windows:
					if (playerNumber == 3) {
						axisX = "p2 4th axis";
						axisY = "p2 5th axis";
						fireButton = KeyCode.Joystick4Button4;
					} else {
						axisX = "p2 4th axis";
						axisY = "p2 5th axis";
						fireButton = KeyCode.Joystick4Button5;
					}
					break;
				case OperatingSystemFamily.MacOSX:
					if (playerNumber == 3) {
						axisX = "p2 3rd axis";
						axisY = "p2 4th axis";
						fireButton = KeyCode.Joystick4Button13;
					} else {
						axisX = "p2 3rd axis";
						axisY = "p2 4th axis";
						fireButton = KeyCode.Joystick4Button14;
					}
					break;
				case OperatingSystemFamily.Linux:
					if (playerNumber == 3) {
						axisX = "p2 4th axis";
						axisY = "p2 5th axis";
						fireButton = KeyCode.Joystick4Button4;
					} else {
						axisX = "p2 4th axis";
						axisY = "p2 5th axis";
						fireButton = KeyCode.Joystick4Button5;
					}
					break;
				default:
					break;
			}
		}
	}

	void Update ()
	{
		float x;
		float y;

		bool shootIsDown = false;
		bool shootIsUp = false;

		x = Input.GetAxis(axisX);
		y = Input.GetAxis(axisY);
		shootIsDown = Input.GetKeyDown(fireButton);
		shootIsUp = Input.GetKeyUp(fireButton);

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
