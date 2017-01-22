using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Random Sound", fileName = "Random Sound")]
public class RandomSound : ScriptableObject, ISound
{
	public SoundData[] sounds;

	public float Play(AudioSource source, Vector3 position, float volume)
	{
		if (sounds.Length == 0) return 0;

		return sounds[Random.Range(0, sounds.Length)].Play(source, position, volume);
	}
}
