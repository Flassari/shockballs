using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
	public float mass;

	void Update()
	{
		Vector3 velocity = Vector3.zero; 

		if (IsOutsideGround()) {
		}

		transform.position += Vector3.zero;
	}

	bool IsOutsideGround()
	{	
		// TBD: do actual raycasting underneath to check?
		var pos = transform.position;
		return pos.x > 10f;
	}
}
