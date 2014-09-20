using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Kinect {
	public class StablePointsFilter {
		
		private int windowSize;
		private int minPointsForDetection;

		private int windowSize2;
		private int minPointsForDetection2;

		private List<Vector3> points;
		private List<Vector3> velocitys;

		private float maximumPredictionDifference = 0.2f;

		public StablePointsFilter() {
			points = new List<Vector3>();
			velocitys = new List<Vector3>();
	
			windowSize = 40;
			windowSize2 = 10;
			minPointsForDetection = 30;
			minPointsForDetection2 = 10;
		}
		
		public bool CheckPointValidation(Vector3 point, Vector3 velocity) {

			if (velocitys.Count < minPointsForDetection2) {
				velocitys.Add(velocity);
			}

			if(points.Count < minPointsForDetection) {
				points.Add(point);
				velocitys.Add(velocity);
				return true;
			}

			if (Validation (point, velocity)) {
				points.Add (point);
				velocitys.Add(velocity);
				if (points.Count > windowSize) {
					points.RemoveAt(0);
				}
				
				if (velocitys.Count > windowSize2) {
					velocitys.RemoveAt(0);
				}

				return true;
			}



			return false;
		}

		bool Validation(Vector3 point, Vector3 velocity) {
			bool result = true;
			//result = result && InvalidIfVelocityLow (velocity);
			result = result && InvalidIfMovementLow (point);
			//result = result && InvalidIfDifferentWithPrediction (point, velocity);

			return result;
		}

		bool InvalidIfVelocityLow(Vector3 velocity) {
			velocity.z = 0;
			float velocityValue = velocity.magnitude;
			
			if (velocityValue < 0.2f) {
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
		
	}
}

