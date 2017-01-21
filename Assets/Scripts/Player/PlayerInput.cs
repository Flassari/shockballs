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
	private KeyCode fireButton1;
	private KeyCode fireButton2;

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
			fireButton1 = KeyCode.Joystick1Button4;
			fireButton2 = KeyCode.Joystick1Button13;
		} else if (playerNumber == 3) {
			axisX = "p2 left x";
			axisY = "p2 left y";
			fireButton1 = KeyCode.Joystick2Button4;
			fireButton2 = KeyCode.Joystick2Button13;
		} else {
			// right stick different on Mac: see http://wiki.unity3d.com/index.php?title=Xbox360Controller
			switch (SystemInfo.operatingSystemFamily)
			{
			case OperatingSystemFamily.Windows:
					Debug.Log ("Operating system is Windows");
					if (playerNumber == 2) {
						axisX = "p1 right x win";
						axisY = "p1 right y win";
						fireButton1 = KeyCode.Joystick1Button5;
						fireButton2 = KeyCode.Joystick1Button14;
					} else {
						axisX = "p2 right x win";
						axisY = "p2 right y win";
						fireButton1 = KeyCode.Joystick2Button5;
						fireButton2 = KeyCode.Joystick2Button14;
					}
					break;
			case OperatingSystemFamily.MacOSX:
					Debug.Log ("Operating system is Mac");
					if (playerNumber == 2) {
						axisX = "p1 right x mac";
						axisY = "p1 right y mac";
						fireButton1 = KeyCode.Joystick1Button5;
						fireButton2 = KeyCode.Joystick1Button14;
					} else {
						axisX = "p2 right x mac";
						axisY = "p2 right y mac";
						fireButton1 = KeyCode.Joystick2Button5;
						fireButton2 = KeyCode.Joystick2Button14;
					}
					break;
			case OperatingSystemFamily.Linux:
					Debug.Log ("Operating system is Linux");
					if (playerNumber == 2) {
						axisX = "p1 right x win";
						axisY = "p1 right y win";
						fireButton1 = KeyCode.Joystick1Button5;
						fireButton2 = KeyCode.Joystick1Button14;
					} else {
						axisX = "p2 right x win";
						axisY = "p2 right y win";
						fireButton1 = KeyCode.Joystick2Button5;
						fireButton2 = KeyCode.Joystick2Button14;
					}
					break;
				default:
					break;
			}
		}
	}

	void Update ()
	{
		if (player.IsAlive())
		{
			float x;
			float y;

			bool fireIsDown = false;
			bool fireIsUp = false;

			bool altFireIsDown = false;

			x = Input.GetAxis (axisX);
			y = Input.GetAxis (axisY);
			fireIsDown = Input.GetKeyDown (fireButton1) || Input.GetKeyDown (fireButton2);
			fireIsUp = Input.GetKeyUp (fireButton1) || Input.GetKeyUp (fireButton2);

			if (playerNumber == 1) {
				if (Input.GetKey (KeyCode.A))
					x = -1;
				if (Input.GetKey (KeyCode.D))
					x = 1;
				if (Input.GetKey (KeyCode.W))
					y = 1;
				if (Input.GetKey (KeyCode.S))
					y = -1;
				if (Input.GetKeyDown (KeyCode.LeftShift))
					fireIsDown = true;
				if (Input.GetKeyUp (KeyCode.LeftShift))
					fireIsUp = true;
				if (Input.GetKeyUp (KeyCode.LeftAlt))
					altFireIsDown = true;
			} else if (playerNumber == 2) {
				if (Input.GetKey (KeyCode.LeftArrow))
					x = -1;
				if (Input.GetKey (KeyCode.RightArrow))
					x = 1;
				if (Input.GetKey (KeyCode.UpArrow))
					y = 1;
				if (Input.GetKey (KeyCode.DownArrow))
					y = -1;
				if (Input.GetKeyDown (KeyCode.RightShift))
					fireIsDown = true;
				if (Input.GetKeyUp (KeyCode.RightShift))
					fireIsUp = true;
				if (Input.GetKeyUp (KeyCode.RightAlt))
					altFireIsDown = true;
			}

			// float x = Input.GetAxis("p" + playerNumber.ToString().ToLower() + " left x");
			// float y = Input.GetAxis("p" + playerNumber.ToString().ToLower() + " left y");

			player.Move (x, y);

			//if (Input.GetKeyDown(GetJoystickButton(1)))
			if (fireIsDown) {
				// Debug.Log("player " + playerNumber + " pressed button " + GetJoystickButton(1).ToString());
				player.StartCharging ();
			}

			if (fireIsUp) {
				// Debug.Log("player " + playerNumber + " pressed button " + GetJoystickButton(1).ToString());
				player.StopCharging ();
			}

			if (altFireIsDown) {
				player.AltFire();
			}
		}
	}

}
