using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect.Monitor;
using Kinect.Shape;

namespace Kinect.Monitor {

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

		public override void SetHandIndex() {
			handIndex = (int)Kinect.NuiSkeletonPositionIndex.HandLeft;
		}
	}
}