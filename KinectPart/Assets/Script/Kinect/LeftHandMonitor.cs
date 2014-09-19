using UnityEngine;
using System.Collections;
using Kinect.Monitor;

public class LeftHandMonitor : HandMonitor {

	public override void SetWristIndex() {
		wristIndex = (int)Kinect.NuiSkeletonPositionIndex.WristLeft;
	}

	public override void SetShoulderIndex() {
		shoulderIndex = (int)Kinect.NuiSkeletonPositionIndex.ShoulderLeft;
	}

	public override void SetElbowIndex() {
		elbowIndex = (int)Kinect.NuiSkeletonPositionIndex.ElbowLeft;
	}
}
