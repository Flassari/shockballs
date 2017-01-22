using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
	public float mass;

	private bool isOnGroundOneWayFlag = false;
	private static float groundCheckInterval = 0.5f;
	private float groundCheckTimer = groundCheckInterval;

	void Start()
	{
		transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
	}

	void Update()
	{
		if (!isOnGroundOneWayFlag)
		{
			if (IsOutsideGround())
			{
				Vector3 vel = -transform.position.normalized;
				transform.position += vel * Time.deltaTime;
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
