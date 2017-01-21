using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveCollider : MonoBehaviour
{
	public ShockWaveSegment segment;

	void Awake()
	{
		segment = GetComponentInParent<ShockWaveSegment>();
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "ShockWave")
		{
			ShockWaveCollider otherCollider = col.GetComponent<ShockWaveCollider>();
			if (otherCollider != null && otherCollider.segment.shockWave != this.segment.shockWave)
			{
				segment.gameObject.SetActive(false);
				otherCollider.segment.gameObject.SetActive(false);
			}
		}
	}
}
