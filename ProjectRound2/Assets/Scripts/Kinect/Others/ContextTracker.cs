/// <summary>
/// Context tracker.
/// Part of the codes is from Kinect ToolBox project (http://kinecttoolbox.codeplex.com/)
/// Modified for Kinect Wrapper Package @ 2014.09, Chiu
/// Modified by Thomas for PaintArcade Project, Approved by original author Chiu.
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using PaintArcade.Generic;

namespace Kinect {
	public class ContextTracker : MonoBehaviour {

		private int windowSize = 40;
		private SkeletonWrapper trackedSkeleton;
		private int trackingID;
		private Dictionary <int, List <ContextPoint>> points;

		public bool AssignSkeletonWrapper(SkeletonWrapper sw, int trackingID) {
			this.trackingID = trackingID;
			trackedSkeleton = sw;

			if (trackingID < 0 || trackingID >= 2) {
				Debug.Log ("Invalid tracking ID.");
				this.enabled = false;
				return false;
			}
			
			if (trackedSkeleton == null) {
				Debug.Log ("Can't find skeleton wrapper.");
				this.enabled = false;
				return false;
			}

			return true;
		}

		public Dictionary <int, List <ContextPoint>> GetData() {
			return points;
		}


		public bool ReadSkeleton() {
			if (trackedSkeleton.pollSkeleton ()) {
				for (int i = 0; i < (int)Kinect.NuiSkeletonPositionIndex.Count; i ++) {
					Add (i);
				}
			}

			int maxIndex = points[0].Count-1;
			if (maxIndex < windowSize-1)
				return false;

			return true;
		}

		void Start () {
			points = new Dictionary<int, List<ContextPoint>> ();
		}

		private void Add (int bone, Vector3 position) {
			if (!points.ContainsKey (bone)) {
				points.Add (bone, new List<ContextPoint> ());
			}

			points[bone].Add (new ContextPoint (position, DateTime.Now));

			if (points[bone].Count > windowSize) {
				ContextPoint lastPoint = points[bone][windowSize];
				ContextPoint lastSecondPoint = points[bone][windowSize-1];
				lastPoint.Position = lastPoint.Position * Consts.KinectDrawingSmoothFactor + lastSecondPoint.Position * (1 - Consts.KinectDrawingSmoothFactor);
				points[bone][windowSize] = lastPoint;
				points[bone].RemoveAt(0);
			}
		}

		private void Add (int bone) {
			this.Add (bone, trackedSkeleton.bonePos[trackingID, bone]);
		}

	}
}