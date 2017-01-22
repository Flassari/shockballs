using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Composite Sound", fileName = "Composite Sound")]
public class CompositeSound : ScriptableObject, ISound
{
	public SoundData[] sounds;

	public float Play(AudioSource source, Vector3 position, float volume)
	{
		float maxLength = 0;
		foreach (SoundData sound in sounds) {
			maxLength = Mathf.Max(maxLength, sound.Play(source, position, volume));
		}
		return maxLength;
	}
}
