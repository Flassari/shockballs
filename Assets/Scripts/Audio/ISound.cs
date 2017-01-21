using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ISound
{
	void Play(Vector3 position, float volume);
}

[Serializable]
public class SoundData
{
	public UnityEngine.Object sound;
	public float volume = 1.0f;

	public virtual void Play(Vector3 position, float volume = 1)
	{
		if (sound is AudioClip) {
			AudioSource.PlayClipAtPoint((AudioClip)sound, position, volume * this.volume);
		}
		else if (sound is ISound) {
			((ISound)sound).Play(position, volume);
		}
		else {
			throw new UnityException("Sound " + sound.name + " type " + sound.GetType() + " unsupported.");
		}
	}
}
