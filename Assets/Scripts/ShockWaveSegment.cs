using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveSegment : MonoBehaviour
{
	public ShockWave shockWave;
	public Vector3 direction;
	public Renderer renderer;

	public Material material { get; private set; }

	protected void Awake()
	{
		material = renderer.material;
	}

	public void SetColor(Color color)
	{
		material.SetColor("_TintColor", color);
	}
}
