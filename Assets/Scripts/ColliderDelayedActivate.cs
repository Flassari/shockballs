using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDelayedActivate : MonoBehaviour
{
	public Collider[] colliders;

	protected void Awake()
	{
		SetCollidersEnabled(false);
	}

	protected IEnumerator Start()
	{
		yield return null;
		SetCollidersEnabled(true);
	}

	#if UNITY_EDITOR
	protected void OnValidate()
	{
		SetCollidersEnabled(false);
	}
	#endif

	private void SetCollidersEnabled(bool enabled)
	{
		foreach (Collider collider in colliders)
		{
			if (collider == null) continue;

			collider.enabled = enabled;
		}
	}
}
