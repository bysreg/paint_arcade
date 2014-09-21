using UnityEngine;
using System.Collections;
using Kinect.Shape;
using System.Collections.Generic;

namespace Kinect.Monitor {
	public abstract class HandMonitor : MonoBehaviour {
		
		public SkeletonWrapper SW;
		public int player;
		public static float validOperateDistanceScale = 2f;
		public static float validOperateDegree = 90f;
		public float minimumMoveDistance;
		public bool UseStablePointFilter;

		protected Vector3 handPosition;
		protected HandState handState;
		protected CircleGestureSegment circleGestureSegment;
		protected StablePointsFilter stablePointsFilter;
		protected StablePointsFilter2 stablePointsFilter2;
		protected int wristIndex;
		protected int elbowIndex;
		protected int shoulderIndex;
		protected Dictionary<string, ShapeClass> resultDict;
		protected Vector3 startPoint;
		protected Vector3 endPoint;

		protected int holdTimes = 0;
		protected int operateTimes = 0;
		protected float smoothFactor = 0.1f;

		public enum HandState{
			Hold,
			Operate
		}

		public HandState GetHandState() {
			return handState;
		}

		public Vector3 GetHandPosition() {
			return handPosition;
		}
		
		// Use this for initialization
		void Start () {
			SetElbowIndex ();
			SetShoulderIndex ();
			SetWristIndex ();
			handState = HandState.Hold;
			circleGestureSegment = new CircleGestureSegment();
			stablePointsFilter = new StablePointsFilter();
			stablePointsFilter2 = new StablePointsFilter2();

			resultDict = new Dictionary<string, ShapeClass>();

		}
		
		public abstract void SetWristIndex();
		public abstract void SetElbowIndex();
		public abstract void SetShoulderIndex();

		public Dictionary<string, ShapeClass> Process() {

			if (SW.pollSkeleton()) {

				if (!UseStablePointFilter ||stablePointsFilter.CheckPointValidation (SW.bonePos [player, wristIndex], SW.boneVel [player, wristIndex])) {
					CheckAndUpdateState();

					//Smooth
					Vector3 smoothedPos = (1-smoothFactor)*SW.bonePos [player, wristIndex] + handPosition*smoothFactor;
					UpdateHandData(smoothedPos, SW.boneVel [player, wristIndex]);

					//^

					//UpdateHandData(SW.bonePos [player, wristIndex], SW.boneVel [player, wristIndex]);
					resultDict.Clear();
				}



				/*
				if (!UseStablePointFilter || stablePointsFilter2.CheckPointValidation (SW.bonePos [player, wristIndex])) {
					CheckAndUpdateState();
					UpdateHandData(SW.bonePos [player, wristIndex]);
					resultDict.Clear();
				}
				*/
				//Circle Detection
				//Circle circle = circleGestureSegment.AddandDetect(SW.bonePos [player, wristIndex]);
				//if(circle.diameter != 0f) {
				//resultDict.Add(Circle.identifier, circle);
				//}
				//^
			}



			return resultDict;
		}

		void CheckAndUpdateState() {
			Vector3 elbowToWrist = SW.bonePos [player, wristIndex] - SW.bonePos [player, elbowIndex];
			Vector3 elbowToShoulder = SW.bonePos [player, shoulderIndex] - SW.bonePos [player, elbowIndex];
			float elbowAngle = Vector3.Angle (elbowToWrist, elbowToShoulder);
			float headToNeckDistance = Vector3.Distance (SW.bonePos [player, (int)Kinect.NuiSkeletonPositionIndex.Head], SW.bonePos [player, (int)Kinect.NuiSkeletonPositionIndex.ShoulderCenter]);
			float wristToNeckDistanceZ = SW.bonePos [player, wristIndex].z - SW.bonePos [player, (int)Kinect.NuiSkeletonPositionIndex.ShoulderCenter].z;
			float wristToNeckDistance = Vector3.Distance(SW.bonePos [player, wristIndex], SW.bonePos [player, (int)Kinect.NuiSkeletonPositionIndex.ShoulderCenter]);
			int requiredTimeToChangeStatus = 10;
			if (wristToNeckDistance > validOperateDistanceScale * headToNeckDistance) {
				if(elbowAngle > validOperateDegree) {
					operateTimes++;
					holdTimes = 0;
					if(operateTimes > requiredTimeToChangeStatus)
						SetOperateState();
				} else {
					holdTimes++;
					operateTimes = 0;
					if(holdTimes > requiredTimeToChangeStatus)
						SetHoldState();
				}
			} else {	
				holdTimes++;
				operateTimes = 0;
				if(holdTimes > requiredTimeToChangeStatus)
					SetHoldState();
			}
		}
		
		void SetHoldState() {
			handState = HandState.Hold;
		}
		
		void SetOperateState() {
			handState = HandState.Operate;
		}
		
		void UpdateHandData(Vector3 point, Vector3 velocity) {
			float movement = Vector3.Distance (handPosition, point);
			float speed = Mathf.Sqrt (Mathf.Pow (velocity.x, 2) + Mathf.Pow (velocity.y, 2));
			if (movement >= 0.006f && speed > 0.08f) {
				handPosition = point;
			}
		}

	}
}

