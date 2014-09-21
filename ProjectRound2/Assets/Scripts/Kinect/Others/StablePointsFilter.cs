using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Kinect {
	public class StablePointsFilter {
		
		private int windowSize;
		private int minPointsForDetection;

		private List<Vector3> points;
		private List<Vector3> velocitys;

		private float maximumPredictionDifference = 0.2f;

		public StablePointsFilter() {
			points = new List<Vector3>();
			velocitys = new List<Vector3>();
	
			windowSize = 40;
			minPointsForDetection = 30;
		}
		
		public bool CheckPointValidation(Vector3 point, Vector3 velocity) {

			if (points.Count < minPointsForDetection) {
				velocitys.Add(velocity);
				points.Add(point);
				return false;
			}

			if (Validation (point, velocity)) {
				points.Add (point);
				velocitys.Add(velocity);
				if (points.Count > windowSize) {
					points.RemoveAt(0);
					velocitys.RemoveAt(0);
				}

				return true;
			}



			return false;
		}

		bool Validation(Vector3 point, Vector3 velocity) {
			bool result = true;
			result = result && InvalidIfVelocityLow (velocity);
			result = result && InvalidIfMovementLow (point);
			result = result && InvalidIfDifferentWithPrediction (point, velocity);

			return result;
		}

		bool InvalidIfVelocityLow(Vector3 velocity) {
			velocity.z = 0;
			float velocityValue = velocity.magnitude;
			
			if (velocityValue < 0.02f) {
				return false;
			}
			return true;
		}

		bool InvalidIfMovementLow(Vector3 point) {
			float movement = Vector3.Distance (point, points [points.Count - 1]);
			if (movement < 0.01f) {
				return false;
			}

			return true;
		}

		bool InvalidIfDifferentWithPrediction(Vector3 point, Vector3 velocity) {
			int pointsLength = points.Count;
			int velocitysLength = velocitys.Count;
			
			Vector3 movement = points [pointsLength - 1] - points [0];
			Vector3 avgSpeed = movement / (pointsLength * Time.deltaTime);

			velocity.z = 0;
			float velocityValue = velocity.magnitude;
			
			Vector3 predictPos = points [pointsLength - 1] + avgSpeed * Time.deltaTime;
			
			float difference = Vector3.Distance (point, predictPos);
			
			if (difference > maximumPredictionDifference && velocityValue < 1.0f) {
				points.Add(predictPos);
				velocitys.Add(avgSpeed);
				points.RemoveAt(0);
				velocitys.RemoveAt(0);
				return false;
			}

			return true;
		}

		public Vector3 SmoothPoint(Vector3 point) {
			int count = points.Count;
			Vector3 smoothPoint = point * 40;
			int times = 40;
			if (count > 10) {
				for(int i=0;i<9;i++) {
					smoothPoint += points[count-1-i]*(9-i);
					times += 9 - i;
				}
				return smoothPoint/times;
			}
			return point;
		}

		public Vector3 ExponentialMovingAveragePoint(Vector3 point) {
			float[] datax = new float[points.Count];
			float[] datay = new float[points.Count];

			for (int i=0; i<points.Count; i++) {
				datax[i] = points[i].x;
				datay[i] = points[i].y;
			}
			float x = ExponentialMovingAverage(datax, 0.9f);
			float y = ExponentialMovingAverage(datax, 0.9f);

			return new Vector3 (x, y, point.z);

		}

		public float ExponentialMovingAverage( float[] data, float baseValue )
		{
			float numerator = 0;
			float denominator = 0;
			float sum = 0;

			for (int i=0; i< data.Length; i++) {
				sum += data[i];
			}

			float average = sum / data.Length;

			for ( int i = 0; i < data.Length; ++i )
			{
				numerator += data[i] * Mathf.Pow( baseValue, data.Length - i - 1 );
				denominator += Mathf.Pow( baseValue, data.Length - i - 1 );
			}
			
			numerator += average * Mathf.Pow( baseValue, data.Length );
			denominator += Mathf.Pow( baseValue, data.Length );
			
			return numerator / denominator;
		}
		
	}
}

