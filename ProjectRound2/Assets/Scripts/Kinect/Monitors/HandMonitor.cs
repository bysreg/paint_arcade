using UnityEngine;
using System.Collections;
using Kinect.Shape;
using System.Collections.Generic;
using PaintArcade.Generic;

namespace Kinect.Monitor {
	public abstract class HandMonitor : MonoBehaviour {
	
		protected Vector3 handPosition = Vector3.zero;
		protected HandState handState;
		protected int wristIndex;
		protected int elbowIndex;
		protected int shoulderIndex;
		protected int handIndex;

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
			SetHandIndex ();
			handState = HandState.Hold;
		}
		
		public abstract void SetWristIndex();
		public abstract void SetElbowIndex();
		public abstract void SetShoulderIndex();
		public abstract void SetHandIndex();

		public void Process(Dictionary <int, List <ContextPoint>> data) {
			int maxIndex = data[0].Count-1;
			if (maxIndex <= 0)
				return;
			
			Vector3 wristPosition = data [wristIndex] [maxIndex].Position;
			Vector3 handPos = data [handIndex] [maxIndex].Position;
			Vector3 headPosition = data [(int)Kinect.NuiSkeletonPositionIndex.Head] [maxIndex].Position;
			Vector3 neckPosition = data [(int)Kinect.NuiSkeletonPositionIndex.ShoulderCenter] [maxIndex].Position;

			float headToNeckDistance = Vector3.Distance (headPosition, neckPosition);
			float wristToNeckDistance = Vector3.Distance(wristPosition, neckPosition);
			
			//Debug.Log("wrist to neck: " + wristToNeckDistance);
			//Debug.Log("head to neck: " + headToNeckDistance);
			
			if (wristToNeckDistance > Consts.ValidOperateDistanceScale * headToNeckDistance) {
				SetOperateState();
			} else {
				SetHoldState();
				SetOperateState();//TODO: del

			}
		
			handPosition = wristPosition;
		}
		
		void SetHoldState() {
			handState = HandState.Hold;
		}
		
		void SetOperateState() {
			handState = HandState.Operate;
		}

	}
}

