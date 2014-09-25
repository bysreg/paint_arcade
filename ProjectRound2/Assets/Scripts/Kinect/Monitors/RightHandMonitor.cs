using UnityEngine;
using System.Collections;
using Kinect.Monitor;

public class RightHandMonitor : HandMonitor {

	public override void SetWristIndex() {
		wristIndex = (int)Kinect.NuiSkeletonPositionIndex.WristRight;
	}

	public override void SetShoulderIndex() {
		shoulderIndex = (int)Kinect.NuiSkeletonPositionIndex.ShoulderRight;
	}

	public override void SetElbowIndex() {
		elbowIndex = (int)Kinect.NuiSkeletonPositionIndex.ElbowRight;
	}

	public override void SetHandIndex() {
		handIndex = (int)Kinect.NuiSkeletonPositionIndex.HandRight;
	}
}
