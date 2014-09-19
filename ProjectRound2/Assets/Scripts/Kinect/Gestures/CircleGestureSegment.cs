using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect.Shape;

public class CircleGestureSegment{

	private int windowSize;
	private List<Vector3> points;
	private int minPointsForDetection;
	private float startPointDstanceTolerance = 0.1f;
	private float maximumCenterVariance = 0.1f;
	private float minimumAvgDiameter = 0.5f;
	private Vector3 avgCenter;
	// Use this for initialization

	public CircleGestureSegment() {
		points = new List<Vector3>();
		windowSize = 40;
		minPointsForDetection = 20;
	}

	public Circle AddandDetect(Vector3 point) {
		points.Add (point);
		if (points.Count > windowSize) {
			//Debug.Log("CircleGestureSegement Full");
			points.RemoveAt(0);
		}
		return Detect();
	}

	Circle Detect() {
		if (points.Count < minPointsForDetection)
			return new Circle(0, Vector3.zero);

		int startPointIndex = 0;
		int endPointIndex = FindEndPointIndex ();

		if (endPointIndex != -1) {
			return DetectCircle(startPointIndex, endPointIndex);
		}

		return new Circle(0, Vector3.zero);
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

	Circle DetectCircle(int startIndex, int endIndex) {
		if (endIndex % 2 == 0) {
			endIndex--;
		}

		List<Circle> circles = FindCircles(startIndex, endIndex);
		float avgDiameter = AverageDiameterFromCircles (circles);
		//Debug.Log ("Avg diameter: " + avgDiameter);
		if (avgDiameter > minimumAvgDiameter) {
			float variance = CenterVarianceFromCircles(circles);
			//Debug.Log("Variance: " + variance);

			if(variance <= maximumCenterVariance) {
				points.Clear();
				Circle circle = new Circle(avgDiameter, avgCenter);
				return circle;
			}
		}

		return new Circle(0, Vector3.zero);
	}



	List<Circle> FindCircles(int startIndex, int endIndex) {
		List<Circle> circles = new List<Circle>();

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
			circles.Add(new Circle(maxDistance, (points[startIndex] + points[farestIndex])*.5f));
		}
		return circles;
	}


	float CenterVarianceFromCircles(List<Circle> circles) {
		Vector3 avgCenter = AverageCenterFromCircles (circles);
		float squareDifferenceSumX = 0f;
		float squareDifferenceSumY = 0f;

		foreach (Circle circle in circles) {
			Vector3 center = circle.center;
			squareDifferenceSumX += Mathf.Pow((center.x-avgCenter.x), 2);
			squareDifferenceSumY += Mathf.Pow((center.y-avgCenter.y), 2);
		}

		float varianceX = squareDifferenceSumX/circles.Count;
		float varianceY = squareDifferenceSumY/circles.Count;

		return Mathf.Sqrt(Mathf.Pow(varianceX, 2)+Mathf.Pow(varianceY, 2));

	}

	Vector3 AverageCenterFromCircles(List<Circle> circles) {
		avgCenter = Vector3.zero;
		foreach (Circle circle in circles) {
			avgCenter += circle.center;
		}
		avgCenter /= circles.Count;
		return avgCenter;
	}

	float AverageDiameterFromCircles(List<Circle> circles) {
		float avg = 0f;
		foreach (Circle circle in circles) {
			avg += circle.diameter;
		}
		return avg / circles.Count;
	}


}
