using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAnimation : MonoBehaviour
{
	[SerializeField]
	private AnimationClip splashAnim;
	[SerializeField]
	private float randomStart = 10f;
	[SerializeField]
	private float randomEnd = 40f;
	private Animation anim;

	private float nextSplashTime = 0f;

	void Start ()
	{
		anim = GetComponent<Animation>();
		nextSplashTime = Time.time + Random.Range(randomStart, randomEnd);
	}
	
	void Update ()
	{
		if (Time.time > nextSplashTime)
		{
			anim.Play(splashAnim.name);
			nextSplashTime = Time.time + Random.Range(randomStart, randomEnd);
		}
	}
}
