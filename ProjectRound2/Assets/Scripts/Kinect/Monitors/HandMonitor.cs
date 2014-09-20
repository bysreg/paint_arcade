﻿using UnityEngine;
using System.Collections;
using Kinect.Shape;
using System.Collections.Generic;

namespace Kinect.Monitor {
	public abstract class HandMonitor : MonoBehaviour {
		
		public SkeletonWrapper SW;
		public int player;
		public static float validOperateDistanceScale = 1.7f;
		public static float validOperateDegree = 90f;
		public float minimumMoveDistance;

		protected Vector3 handPosition;
		protected HandState handState;
		protected CircleGestureSegment circleGestureSegment;
		protected StablePointsFilter stablePointsFilter;
		protected int wristIndex;
		protected int elbowIndex;
		protected int shoulderIndex;
		protected Dictionary<string, ShapeClass> resultDict;
		protected Vector3 startPoint;
		protected Vector3 endPoint;

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
			resultDict = new Dictionary<string, ShapeClass>();

		}
		
		public abstract void SetWristIndex();
		public abstract void SetElbowIndex();
		public abstract void SetShoulderIndex();

		public Dictionary<string, ShapeClass> Process() {

			if (SW.pollSkeleton()) {
				if (stablePointsFilter.CheckPointValidation (SW.bonePos [player, wristIndex], SW.boneVel [player, wristIndex])) {
					CheckAndUpdateState();
					UpdateHandData();
					resultDict.Clear();
				}
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

			if (wristToNeckDistanceZ > validOperateDistanceScale * headToNeckDistance) {
				if(elbowAngle > validOperateDegree) {
					SetOperateState();
				} else {
					SetHoldState();
				}
			} else {			
				SetHoldState();
			}
		}
		
		void SetHoldState() {
			handState = HandState.Hold;
		}
		
		void SetOperateState() {
			handState = HandState.Operate;
		}
		
		void UpdateHandData() {
			float movement = Vector3.Distance (handPosition, SW.bonePos [player, wristIndex]);
			if (movement >= minimumMoveDistance) {
				handPosition = SW.bonePos [player, wristIndex];
			}
		}

	}
}
