using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
	public Player owner;
	public ShockWaveSegment segmentPrefab;
	public float startRadius = 1;
	public int segmentCount = 8;
	public float maxLifeTime = 5f;
	public float propagationSpeed = 1f;
	public float power = 0f;
	public float fadeoutTime = 0.3f;

	public Color color;

	private List<ShockWaveSegment> segments = new List<ShockWaveSegment>();
	private float lifeTime = 0f;
	private float radius = 0f;
	private float alpha = 1;

	public float Power { get { return power; } }

	void Start()
	{
		radius = startRadius;

		for (int i = 0; i < segmentCount; i++)
		{
			ShockWaveSegment segment = Instantiate(segmentPrefab, transform);
			segment.shockWave = this;
			segment.gameObject.layer = gameObject.layer;
			segments.Add(segment);
			segment.SetColor(color);
		}
	}

	protected void Update()
	{
		lifeTime += Time.deltaTime;
		if (lifeTime > maxLifeTime)
		{
			Destroy(this.gameObject);
			return;
		}

		radius += Time.deltaTime * propagationSpeed;

		Recalculate();
	}

	private void Recalculate()
	{
		for (int i = 0; i < segmentCount; i++) {
			
			float radianAngle = ((float)i / segmentCount) * (Mathf.PI * 2);

			float yRotation = 360 - radianAngle * (180.0f / Mathf.PI);

			float angleSpan = ((Mathf.PI * 2) / segmentCount);

			float innerRadius = radius * Mathf.Cos(angleSpan / 2);

			float length = 2.0f * Mathf.Sin(angleSpan / 2) * radius;

			float x = Mathf.Cos(radianAngle) * innerRadius;
			float z = Mathf.Sin(radianAngle) * innerRadius;

			ShockWaveSegment segment = segments[i];
			Vector3 dir = new Vector3(x, 0, z);
			segment.direction = dir.normalized;

			segment.transform.localPosition = dir;
			segment.transform.rotation = Quaternion.Euler(0, yRotation, 0);
			segment.transform.localScale = new Vector3(segmentPrefab.transform.localScale.x, segmentPrefab.transform.localScale.y, length);

			if (lifeTime + fadeoutTime > maxLifeTime)
			{
				color.a = (maxLifeTime - lifeTime) / fadeoutTime;
				segment.SetColor(color);
			}
		}
	}
}
