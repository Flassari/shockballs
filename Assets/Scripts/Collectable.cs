﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
	public float mass;

	void Update()
	{
		transform.position += Vector3.zero;
	}
}