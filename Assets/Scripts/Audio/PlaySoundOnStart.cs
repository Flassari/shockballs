using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnStart : MonoBehaviour
{
	public SoundData sound;

	protected IEnumerator Start () {
		float soundLength = sound.Play(GetComponent<AudioSource>(), transform.position);
		yield return new WaitForSeconds(soundLength);
		Destroy(gameObject);
	}
}
