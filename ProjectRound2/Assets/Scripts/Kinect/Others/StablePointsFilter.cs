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
			bool result = false;

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

			Vector3 vtmp = velocity;
			vtmp.z = 0;
			float velocityValue = vtmp.magnitude;

			if (velocityValue < 0.2f) {
				return false;
			}

			int pointsLength = points.Count;
			int velocitysLength = velocitys.Count;

			Vector3 movement = points [pointsLength - 1] - points [0];
			Vector3 avgSpeed = movement / (pointsLength * Time.deltaTime);

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

