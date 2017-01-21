using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Composite Sound", fileName = "Composite Sound")]
public class CompositeSound : ScriptableObject, ISound
{
	public SoundData[] sounds;

	public void Play(Vector3 position, float volume)
	{
		foreach (SoundData sound in sounds) {
			sound.Play(position, volume);
		}
	}
}
