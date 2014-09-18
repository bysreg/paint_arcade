using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Kinect.GestureSegement {

	public class GestureSegement : MonoBehaviour {
		
		public SkeletonWrapper TrackedSkeleton;
		public int TrackingID;
		private int windowSize;
		private NuiSkeletonPositionIndex boneIndex;
		private List<Vector3> points;

		protected void Awake() {
			points = new List<Vector3>();
			SetIndex();
		}

		virtual void SetIndex();
		
		
		void Update () {
			for (int i = 0; i < (int)Kinect.NuiSkeletonPositionIndex.Count; i ++) {
				Add (i);
			}
		}
		
		protected void Add () {
			points.Add(TrackedSkeleton.bonePos[this.TrackingID, boneIndex]);
			DetectGesture();
		}

		virtual bool DetectGesture();
		
	}


}