using UnityEngine;
using System.Collections;


namespace Kinect.Monitor {
	public abstract class HandMonitor : MonoBehaviour {
		
		public SkeletonWrapper SW;
		public int player;
		public static float validOperateDistanceScale = 1.4f;
		public static float validOperateDegree = 115f;
		public Vector3 handPosition;

		protected HandState handState;
		protected CircleGestureSegment circleGestureSegment;
		protected int wristIndex;
		protected int elbowIndex;
		protected int shoulderIndex;
		
		
		public enum HandState{
			Hold,
			Operate
		}
		
		public HandState getHandState() {
			return handState;
		}
		
		// Use this for initialization
		void Start () {
			SetElbowIndex ();
			SetShoulderIndex ();
			SetWristIndex ();
			handState = HandState.Hold;
			circleGestureSegment = new CircleGestureSegment();
		}
		
		public abstract void SetWristIndex();
		public abstract void SetElbowIndex();
		public abstract void SetShoulderIndex();

		public void Process() {
			CheckAndUpdateState();
			UpdateHandData();
			if (SW.pollSkeleton()) {
				circleGestureSegment.AddandDetect(SW.bonePos [player, (int)Kinect.NuiSkeletonPositionIndex.WristRight]);
			}
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
			handPosition = SW.bonePos [player, wristIndex];
		}
	}
}

