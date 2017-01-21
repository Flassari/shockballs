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
		if (col.CompareTag("ShockWave"))
		{
			ShockWaveCollider otherCollider = col.GetComponent<ShockWaveCollider>();
			if (otherCollider != null && otherCollider.segment.shockWave != this.segment.shockWave &&
				otherCollider.segment.shockWave.owner != this.segment.shockWave.owner)
			{
				segment.gameObject.SetActive(false);
				otherCollider.segment.gameObject.SetActive(false);
			}
		}

		if (col.CompareTag("Wall"))
		{
			segment.gameObject.SetActive(false);
		}

		if (col.CompareTag("Bomb"))
		{
			segment.gameObject.SetActive(false);
			Bomb bomb = col.transform.parent.GetComponent<Bomb>().Explode();
		}
	}
}
