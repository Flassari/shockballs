using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
	public float mass;

	void Update()
	{
		Vector3 vel = Vector3.zero; 

		if (IsOutsideGround())
		{
			Debug.Log ("float collectible to center");
			vel = -transform.position.normalized * 0.05f;
		}

		transform.position += vel;
	}

	bool IsOutsideGround()
	{	
		// TBD: do actual raycasting underneath to check?
		var pos = transform.position;
		var margin = 25f;
		return (pos.x < -margin || pos.x > margin || pos.z < -margin || pos.z > margin);
	}
}
