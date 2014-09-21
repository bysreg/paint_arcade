using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect.Monitor;
using Kinect.Shape;

namespace Kinect {
	public class KinectManager : MonoBehaviour {

		//public delegate void OnGesttureDetected(GameObject g);
		//public event OnGesttureDetected OnClick;

		public bool SimulateWithKeyBoard;
		public SkeletonWrapper SW;
		public int player;
		private LeftHandMonitor leftHandMonitor;
		private RightHandMonitor rightHandMonitor;
		public PlayerHand LeftHand;
		public PlayerHand RightHand;
		public GameObject PaintBoard;
		public bool UseStablePointFilter;

		private GameObject canvas;
		private float kinectToCanvasScale = 1.75f;
		private float canvasHeightToWidthRatio;
		private float skeletonDrawHeight;
		private float skeletonDrawWidth;
		private float canvasWidth;
		private float canvasHeight;
		private Vector3 skeletonDrawCenterLeft;
		private Vector3 skeletonDrawCenterRight;
		private bool dataReady = false;

		// Use this for initialization
		void Start () {

			//TODO: pass data rather than offer SW object
			leftHandMonitor = gameObject.AddComponent<LeftHandMonitor>();
			rightHandMonitor = gameObject.AddComponent<RightHandMonitor>();
			leftHandMonitor.UseStablePointFilter = this.UseStablePointFilter;
			rightHandMonitor.UseStablePointFilter = this.UseStablePointFilter;
			leftHandMonitor.minimumMoveDistance = LeftHand.collider.bounds.size.x / 8f;
			rightHandMonitor.minimumMoveDistance = RightHand.collider.bounds.size.x / 8f;
			leftHandMonitor.SW = SW;
			rightHandMonitor.SW = SW;
			leftHandMonitor.player = player;
			rightHandMonitor.player = player;

			canvas = GameObject.FindGameObjectWithTag ("canvas");
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
			if (SimulateWithKeyBoard) {
				if(Input.GetKey(KeyCode.W)) {
					Vector3 pos = RightHand.transform.position;
					pos.y += 0.1f;
					RightHand.transform.position = pos;
				}
				if(Input.GetKey(KeyCode.S)) {
					Vector3 pos = RightHand.transform.position;
					pos.y -= 0.1f;
					RightHand.transform.position = pos;
				}
				if(Input.GetKey(KeyCode.A)) {
					Vector3 pos = RightHand.transform.position;
					pos.x -= 0.1f;
					RightHand.transform.position = pos;
				}
				if(Input.GetKey(KeyCode.D)) {
					Vector3 pos = RightHand.transform.position;
					pos.x += 0.1f;
					RightHand.transform.position = pos;
				}

				if(Input.GetKey(KeyCode.O)) {
					RightHand.isHandDown = true;
					RightHand.UpdateOutLook();
				} 

				if(Input.GetKey(KeyCode.P)) {
					RightHand.isHandDown = false;
					RightHand.UpdateOutLook();
				} 

			} else {
				syncLeftHand();
				syncRightHand();
			}

		}

		void syncLeftHand() {
			if (!dataReady) {
				return;
			}
			Vector3 pos = leftHandMonitor.GetHandPosition();
			pos.z = PaintBoard.transform.position.z;
			LeftHand.transform.position = PaintPositionFromSkeletonPosition(pos, 0);
			LeftHand.prevIsHandDown = LeftHand.isHandDown;
			LeftHand.prevPos = LeftHand.pos;
			LeftHand.pos = pos;

			if(leftHandMonitor.GetHandState() == HandMonitor.HandState.Hold) {
				LeftHand.isHandDown = false;
				LeftHand.UpdateOutLook();
			} else if(leftHandMonitor.GetHandState() == HandMonitor.HandState.Operate) {
				LeftHand.isHandDown = true;
				LeftHand.UpdateOutLook();
			}
		}

		void syncRightHand() {
			if (!dataReady) {
				return;
			}
			Vector3 pos = rightHandMonitor.GetHandPosition();
			pos.z = PaintBoard.transform.position.z;

			//Debug.Log ("hand: "+ pos);

			RightHand.transform.position = PaintPositionFromSkeletonPosition(pos, 1);
			RightHand.prevIsHandDown = RightHand.isHandDown;
			RightHand.prevPos = RightHand.pos;
			RightHand.pos = pos;

			if(rightHandMonitor.GetHandState() == HandMonitor.HandState.Hold) {
				RightHand.isHandDown = false;
				RightHand.UpdateOutLook();
			} else if(rightHandMonitor.GetHandState() == HandMonitor.HandState.Operate) {
				RightHand.isHandDown = true;
				RightHand.UpdateOutLook();
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

		Vector3 PaintPositionFromSkeletonPosition(Vector3 skeletonPosition, int hand) {
			if(hand == 0) {
				float ratioX = (skeletonPosition.x - skeletonDrawCenterLeft.x) / (skeletonDrawWidth*.5f);
				float ratioY = (skeletonPosition.y - skeletonDrawCenterLeft.y) / (skeletonDrawHeight*.5f);
				float x = canvas.transform.position.x + ratioX * canvasWidth*.5f*kinectToCanvasScale; 
				float y = canvas.transform.position.y + ratioY * canvasHeight*.5f*kinectToCanvasScale; 
				return new Vector3 (x, y, skeletonPosition.z);
			} else if (hand == 1) {
				float ratioX = (skeletonPosition.x - skeletonDrawCenterRight.x) / (skeletonDrawWidth*.5f);
				float ratioY = (skeletonPosition.y - skeletonDrawCenterRight.y) / (skeletonDrawHeight*.5f);
				//Debug.Log("ratiox:"+ratioX);
				float x = canvas.transform.position.x + ratioX * canvasWidth*.5f*kinectToCanvasScale; 
				float y = canvas.transform.position.y + ratioY * canvasHeight*.5f*kinectToCanvasScale; 
				return new Vector3 (x, y, skeletonPosition.z);
			}

			return Vector3.zero;
		}

		void UpdateSkeletonDrawArea() {
			int rightShoulderIndex = (int)Kinect.NuiSkeletonPositionIndex.ShoulderRight;
			int rightWristIndex = (int)Kinect.NuiSkeletonPositionIndex.WristRight;
			int leftShoulderIndex = (int)Kinect.NuiSkeletonPositionIndex.ShoulderLeft;
			int leftWristIndex = (int)Kinect.NuiSkeletonPositionIndex.WristLeft;
			int centerShoulderIndex = (int)Kinect.NuiSkeletonPositionIndex.ShoulderCenter;
			int centerHipIndex = (int)Kinect.NuiSkeletonPositionIndex.HipCenter;
			int leftElbowIndex = (int)Kinect.NuiSkeletonPositionIndex.ElbowLeft;
			int rightElbowIndex = (int)Kinect.NuiSkeletonPositionIndex.ElbowRight;

			float rightHeight =  Vector3.Distance(SW.bonePos[player, rightShoulderIndex], SW.bonePos[player, rightWristIndex]);
			float leftHeight = Vector3.Distance(SW.bonePos[player, leftShoulderIndex], SW.bonePos[player, leftWristIndex]);
			float centerHeight = Vector3.Distance(SW.bonePos[player, centerHipIndex], SW.bonePos[player, centerShoulderIndex]);

			float [] heights = {rightHeight, leftHeight, centerHeight};
			skeletonDrawHeight = Mathf.Max(heights);
			skeletonDrawWidth = Mathf.Sin((60 * Mathf.PI)/180) * skeletonDrawHeight;

			Vector3 rightCenter = SW.bonePos[player, rightShoulderIndex];
			rightCenter.x += skeletonDrawWidth*.5f;
			rightCenter.y += skeletonDrawHeight*.25f;
			skeletonDrawCenterRight = rightCenter;
			
			Vector3 leftCenter = SW.bonePos[player, leftShoulderIndex];
			leftCenter.x -= skeletonDrawWidth*.5f;
			leftCenter.y += skeletonDrawHeight*.25f;
			skeletonDrawCenterLeft = leftCenter;

			//Debug.Log ("center:"+rightCenter);
			//Debug.Log ("width: "+skeletonDrawWidth);

			if(skeletonDrawHeight !=0 && skeletonDrawWidth !=0)
				dataReady = true;
		}
	}
}


;
