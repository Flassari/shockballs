using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Random Sound", fileName = "Random Sound")]
public class RandomSound : ScriptableObject, ISound
{
	public SoundData[] sounds;

	public void Play(Vector3 position, float volume)
	{
		if (sounds.Length == 0) return;

		sounds[Random.Range(0, sounds.Length)].Play(position, volume);
	}
}
