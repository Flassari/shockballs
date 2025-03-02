﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ISound
{
	float Play(AudioSource source, Vector3 position, float volume);
}

[Serializable]
public class SoundData
{
	public UnityEngine.Object sound;
	public float volume = 1.0f;

	public virtual float Play(AudioSource source, Vector3 position, float volume = 1)
	{
		if (sound == null) return 0;

		if (sound is AudioClip) {
			source.PlayOneShot((AudioClip)sound, volume * this.volume);
			return ((AudioClip)sound).length;
		}
		else if (sound is ISound) {
			return ((ISound)sound).Play(source, position, volume);
		}
		else {
			throw new UnityException("Sound " + sound.name + " type " + sound.GetType() + " unsupported.");
		}
	}
}
