using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongScale : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float scale = Mathf.PingPong(Time.time / 5f, .1f);
		Vector3 delta = new Vector3(scale, scale);
		transform.localScale = Vector3.one + delta;
	}
}
