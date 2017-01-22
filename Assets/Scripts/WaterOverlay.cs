using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterOverlay : MonoBehaviour
{
	public float speed = 1;
	private Material material;

	void Start () {
		material = GetComponent<Renderer>().material;
		
	}

	void Update () {
		material.SetTextureOffset("_MainTex", new Vector2(Time.time * speed, 0));
	}
}
