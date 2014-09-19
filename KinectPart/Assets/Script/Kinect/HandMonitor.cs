using UnityEngine;
using System.Collections;

public class HandMonitor : MonoBehaviour {

	public SkeletonWrapper SW;
	public int player;

	private HandState handState;
	public static float validOperateDistanceScale = 1.4f;
	public static float validOperateDegree = 115f;
	private CircleGestureSegment circleGestureSegment;

	public enum HandState{
		Hold,
		Operate
	}

	public HandState getHandState() {
		return handState;
	}

	// Use this for initialization
	void Start () {
		handState = HandState.Hold;
		circleGestureSegment = new CircleGestureSegment ();
		circleGestureSegment.Init ();
	}
	
	// Update is called once per frame
	void Update() {
		if(player == -1)
			return;

		if (SW.pollSkeleton()) {
			circleGestureSegment.AddandDetect(SW.bonePos [player, (int)Kinect.NuiSkeletonPositionIndex.WristRight]);
			//CheckAndUpdateState();
		}
	}

	void CheckAndUpdateState() {
		Vector3 elbowToWrist = SW.bonePos [player, (int)Kinect.NuiSkeletonPositionIndex.WristRight] - SW.bonePos [player, (int)Kinect.NuiSkeletonPositionIndex.ElbowRight];
		Vector3 elbowToShoulder = SW.bonePos [player, (int)Kinect.NuiSkeletonPositionIndex.ShoulderRight] - SW.bonePos [player, (int)Kinect.NuiSkeletonPositionIndex.ElbowRight];
		float elbowAngle = Vector3.Angle (elbowToWrist, elbowToShoulder);
		//Debug.Log (elbowAngle);

		float headToNeckDistance = Vector3.Distance (SW.bonePos [player, (int)Kinect.NuiSkeletonPositionIndex.Head], SW.bonePos [player, (int)Kinect.NuiSkeletonPositionIndex.ShoulderCenter]);
		float wristToNeckDistanceZ = SW.bonePos [player, (int)Kinect.NuiSkeletonPositionIndex.WristRight].z - SW.bonePos [player, (int)Kinect.NuiSkeletonPositionIndex.ShoulderCenter].z;
		//Debug.Log (headToNeckDistance + " " +wristToNeckDistanceZ);

		if (wristToNeckDistanceZ > validOperateDistanceScale * headToNeckDistance) {
			if(elbowAngle > validOperateDegree) {
				Debug.Log("Drawing");
			} else {
				Debug.Log("angle fail");
			}
		} else {
			Debug.Log("distance fail");
		}
	}

	void SetHoldState() {
		handState = HandState.Hold;
	}

	void SetOperateState() {
		handState = HandState.Operate;
	}


}
