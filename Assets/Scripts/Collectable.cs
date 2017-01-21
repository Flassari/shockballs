using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
	public float mass;

	private bool isOnGroundOneWayFlag = false;
	private static float groundCheckInterval = 0.5f;
	private float groundCheckTimer = groundCheckInterval;

	void Update()
	{
		if (!isOnGroundOneWayFlag)
		{
			if (IsOutsideGround ())
			{
				Debug.Log ("float collectible to center");
				Vector3 vel = -transform.position.normalized * 0.05f;
				transform.position += vel;
			} else
			{
				isOnGroundOneWayFlag = true;
			}
		}
	}

	bool IsOutsideGround()
	{	
		// TBD: do actual raycasting underneath to check?
		var pos = transform.position;
		var margin = 25f;
		return (pos.x < -margin || pos.x > margin || pos.z < -margin || pos.z > margin);
	}
}
