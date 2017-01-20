using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
	public GameObject quadPrefab;
	public float radius = 3;
	public int segmentCount = 8;

	private List<GameObject> segments = new List<GameObject>();

	protected void Update()
	{
		bool isDirty = segmentCount != segments.Count;

		while (segmentCount > segments.Count) {
			GameObject newSegment = (GameObject)Instantiate(quadPrefab, transform);
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

			Debug.DrawLine(transform.position, new Vector3(x, 0, z), Color.red);

			//float lenght = 2.0f * Mathf.Cos(((float)((360.0f / (segmentCount * 2))) / 2) * (Mathf.PI / 180.0f));

			GameObject segment = segments[i];

			segment.transform.position = new Vector3(x, 0, z);
			segment.transform.rotation = Quaternion.Euler(0, yRotation, 0);
			segment.transform.localScale = new Vector3(1, 1, length);
		}
	}

	/*protected void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, radius);
	}*/

}
