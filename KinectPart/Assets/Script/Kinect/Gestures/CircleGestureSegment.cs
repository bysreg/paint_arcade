using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircleGestureSegment{

	private int windowSize;
	private List<Vector3> points;
	private int minPointsForDetection;
	private float startPointDstanceTolerance = 0.1f;

	// Use this for initialization
	public void Init () {
		points = new List<Vector3>();
		windowSize = 40;
		minPointsForDetection = 20;
	}

	public bool AddandDetect(Vector3 point) {
		points.Add (point);
		if (points.Count > windowSize) {
			Debug.Log("CircleGestureSegement Full");
			points.RemoveAt(0);
		}

		return Detect();

	}

	bool Detect() {
		if (points.Count < minPointsForDetection)
			return false;

		int startPointIndex = 0;
		int endPointIndex = FindEndPointIndex ();

		if (endPointIndex != -1) {
			DetectCircle(startPointIndex, endPointIndex);
		}


		Debug.Log("End");


		return false;
	}

	int FindEndPointIndex() {
		Vector3 startPoint = points [0];
		for (int i=1; i<points.Count; i++) {
			if(Vector3.Distance(startPoint, points[i]) < startPointDstanceTolerance && i> minPointsForDetection) {
				return i;
			}
		}

		return -1;
	}

	void DetectCircle(int startIndex, int endIndex) {
		if (endIndex % 2 == 0) {
			endIndex--;
		}

		List<Vector3> centerPoints = new List<Vector3>();

		for(int i=startIndex;i<=endIndex;i++) {
			float maxDistance = 0;
			int farestIndex = startIndex;
			for(int j=startIndex+1;j<=endIndex+startIndex;j++) {
				j %= endIndex+1;
				float distance = Mathf.Sqrt(Mathf.Pow(points[i].x-points[j].x,2)+Mathf.Pow(points[i].y-points[j].y,2));
				if(distance > maxDistance) {
					maxDistance = distance;
					farestIndex = j;
				}
			}
			centerPoints.Add((points[startIndex] + points[farestIndex])*.5f);
		}

		Vector3 avg = AverageFromList (centerPoints);

	}

	Vector3 AverageFromList(List<Vector3> list) {
		Vector3 avg = Vector3.zero;
		foreach (Vector3 point in list) {
			avg += point;
		}
		return avg / list.Count;
	}


}
