using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect.Monitor;
using Kinect.Shape;

namespace Kinect {
	public class KinectManager : MonoBehaviour {

		//public delegate void OnGesttureDetected(GameObject g);
		//public event OnGesttureDetected OnClick;

		public SkeletonWrapper SW;
		public int player;
		private LeftHandMonitor leftHandMonitor;
		private RightHandMonitor rightHandMonitor;
		public GameObject LeftHandObject;
		public GameObject RightHandObject;
		public GameObject PaintBoard;
		public Texture green;
		public Texture red;

		private float kinectToCanvasScale = 1f;
		private float canvasHeightToWidthRatio;

		private float skeletonDrawHeight;
		private float skeletonDrawWidth;
		private float canvasWidth;
		private float canvasHeight;
		private Vector3 skeletonDrawCenter;
	
		// Use this for initialization
		void Start () {

			//TODO: pass data rather than offer SW object
			leftHandMonitor = gameObject.AddComponent<LeftHandMonitor>();
			rightHandMonitor = gameObject.AddComponent<RightHandMonitor>();
			leftHandMonitor.SW = SW;
			rightHandMonitor.SW = SW;
			leftHandMonitor.player = player;
			rightHandMonitor.player = player;

			GameObject canvas = GameObject.FindGameObjectWithTag ("canvas");
			canvasWidth = canvas.collider.bounds.size.x;
			canvasHeight = canvas.collider.bounds.size.y;
			canvasHeightToWidthRatio = canvasHeight / canvasWidth;

		}
		
		// Update is called once per frame
		void Update () {
			if(player == -1)
				return;

			Dictionary<string, ShapeClass> leftHandResultDict = leftHandMonitor.Process();
			Dictionary<string, ShapeClass> rightHandResultDict = rightHandMonitor.Process();
			handleLeftHandResult(leftHandResultDict);
			handleRightHandResult(rightHandResultDict);
			UpdateSkeletonDrawArea();
			syncLeftHand();
			syncRightHand();
		}

		void syncLeftHand() {
			Vector3 pos = leftHandMonitor.GetHandPosition()*kinectToCanvasScale;
			pos.z = PaintBoard.transform.position.z;
			LeftHandObject.transform.position = PaintPositionFromSkeletonPosition(pos);
			if(leftHandMonitor.GetHandState() == HandMonitor.HandState.Hold) {
				LeftHandObject.renderer.material.mainTexture = red;
			} else if(leftHandMonitor.GetHandState() == HandMonitor.HandState.Operate) {
				LeftHandObject.renderer.material.mainTexture = green;
			}
		}

		void syncRightHand() {
			Vector3 pos = rightHandMonitor.GetHandPosition()*kinectToCanvasScale;
			pos.z = PaintBoard.transform.position.z;
			RightHandObject.transform.position = PaintPositionFromSkeletonPosition(pos);

			if(rightHandMonitor.GetHandState() == HandMonitor.HandState.Hold) {
				RightHandObject.renderer.material.mainTexture = red;
			} else if(rightHandMonitor.GetHandState() == HandMonitor.HandState.Operate) {
				RightHandObject.renderer.material.mainTexture = green;	
			}
		}

		void handleLeftHandResult(Dictionary<string, ShapeClass> result) {
			if (result == null) {
				return;
			}
			if(result.ContainsKey(Circle.identifier)) {
				Circle circle = (Circle)result[Circle.identifier];
				Debug.Log("Left hand create circle at Point " + circle.center.x + ", "+circle.center.y + " d: "+circle.diameter);
			}
			
		}

		void handleRightHandResult(Dictionary<string, ShapeClass> result) {
			if (result == null) {
				return;
			}

			if(result.ContainsKey(Circle.identifier)) {
				Circle circle = (Circle)result[Circle.identifier];
				Debug.Log("Right hand create circle at Point " + circle.center.x + ", "+circle.center.y + " d: "+circle.diameter);
			}
		}

		Vector3 PaintPositionFromSkeletonPosition(Vector3 skeletonPosition) {
			float ratioX = (skeletonPosition.x - skeletonDrawCenter.x) / skeletonDrawWidth;
			float x = skeletonDrawCenter.x + ratioX * canvasWidth; 

			float ratioY = (skeletonPosition.y - skeletonDrawCenter.y) / skeletonDrawHeight;
			float y = skeletonDrawCenter.y + ratioY * canvasHeight; 

			return new Vector3 (x, y, skeletonPosition.z);
		}

		void UpdateSkeletonDrawArea() {
			int shoulderCenterIndex = (int)Kinect.NuiSkeletonPositionIndex.ShoulderCenter;
			int hipCenterIndex = (int)Kinect.NuiSkeletonPositionIndex.HipCenter;
			skeletonDrawHeight = (SW.bonePos[player, shoulderCenterIndex].y - SW.bonePos[player, hipCenterIndex].y)*4f;
			skeletonDrawWidth = skeletonDrawHeight / canvasHeightToWidthRatio;
			skeletonDrawCenter = SW.bonePos[player, shoulderCenterIndex];

			Debug.Log ("x:"+SW.boneVel [player, (int)Kinect.NuiSkeletonPositionIndex.WristRight].x+"  y:" + SW.boneVel [player, (int)Kinect.NuiSkeletonPositionIndex.WristRight].y);

		}
	}
}


