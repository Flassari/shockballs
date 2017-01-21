using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
	public Player owner;
	public GameObject quadPrefab;
	public float startRadius = 1;
	public int segmentCount = 8;
	public float maxLifeTime = 5f;
	public float propagationSpeed = 1f;
	public float power = 0f;

	private List<GameObject> segments = new List<GameObject>();
	private float lifeTime = 0f;
	private float radius = 0f;

	public float Power { get { return power; } }

	void Start()
	{
		radius = startRadius;
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

		bool isDirty = segmentCount != segments.Count;

		while (segmentCount > segments.Count) {
			GameObject newSegment = (GameObject)Instantiate(quadPrefab, transform);
			newSegment.GetComponent<ShockWaveSegment>().shockWave = this;
			newSegment.layer = gameObject.layer;
			segments.Add(newSegment);
		}

		while (segmentCount < segments.Count) {
			GameObject removeSegment = segments[segments.Count - 1];
			segments.Remove(removeSegment);
			Destroy(removeSegment); // TODO: Pool these globally
		}

		//if (isDirty) {
			Recalculate();
		//}
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

			//float lenght = 2.0f * Mathf.Cos(((float)((360.0f / (segmentCount * 2))) / 2) * (Mathf.PI / 180.0f));

			GameObject segment = segments[i];

			segment.transform.localPosition = new Vector3(x, 0, z);
			segment.transform.rotation = Quaternion.Euler(0, yRotation, 0);
			segment.transform.localScale = new Vector3(1, 1, length);
		}
	}

	/*protected void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, radius);
	}*/

}
