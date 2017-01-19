using System;
using UnityEngine;
using System.Collections;

public class TinyTween
{
	public static IEnumerator Tween(float delay, float duration, Action<float> onTween,
		Func<float, float> easing = null, Action onComplete = null)
	{
		yield return new WaitForSeconds(delay);
		yield return Tween(duration, onTween, easing, onComplete);
	}

	public static IEnumerator Tween(float duration, Action<float> onTween,
		Func<float, float> easing = null, Action onComplete = null)
	{
		if (easing == null) easing = EaseNone;

		float startTime = Time.time;
		float endTime = startTime + duration;

		while (Time.time < endTime)
		{
			float scale = easing((Time.time - startTime) / duration);
			onTween(scale);
			yield return null;
		}
		onTween(1);
		if (onComplete != null) onComplete();
	}

	public static float EaseNone(float t) { return t; }
	public static float EaseIn(float t) { return t * t; }
	public static float EaseOut(float t) { return 1 - EaseIn(1 - t); }
	public static float EaseInOutBezier(float t) { return t * t* (3.0f - 2.0f * t); }
	// Just add more as needed.
}
