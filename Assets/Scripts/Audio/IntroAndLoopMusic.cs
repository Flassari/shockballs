using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAndLoopMusic : MonoBehaviour
{
	public AudioSource intro;
	public AudioSource loop;

	void Start ()
	{
		intro.Play();
		loop.PlayDelayed(intro.clip.length);
	}
}
