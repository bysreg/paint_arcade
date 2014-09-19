using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Kinect.GestureSegement {

	abstract public class GestureSegement : MonoBehaviour {
		
		public SkeletonWrapper TrackedSkeleton;
		public int TrackingID;
		private int windowSize;
		private NuiSkeletonPositionIndex boneIndex;
		private List<Vector3> points;

		protected void Awake() {
			points = new List<Vector3>();
		}

		public abstract void SetIndex();
		
		
		void Update () {
			Add ();
		}
		
		protected void Add () {
			points.Add(TrackedSkeleton.bonePos[this.TrackingID, (int)boneIndex]);
			DetectGesture();
		}

		public abstract bool DetectGesture();
		
	}


}