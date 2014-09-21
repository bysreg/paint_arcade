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
		public GameObject LeftHandObject;
		public GameObject RightHandObject;
		public GameObject PaintBoard;
		public Texture OperateTexture;
		public Texture HoldTexture;

		private GameObject canvas;
		private float kinectToCanvasScale = 1.5f;
		private float canvasHeightToWidthRatio;
		private float skeletonDrawHeight;
		private float skeletonDrawWidth;
		private float canvasWidth;
		private float canvasHeight;
		private Vector3 skeletonDrawCenterLeft;
		private Vector3 skeletonDrawCenterRight;

		// Use this for initialization
		void Start () {

			//TODO: pass data rather than offer SW object
			leftHandMonitor = gameObject.AddComponent<LeftHandMonitor>();
			rightHandMonitor = gameObject.AddComponent<RightHandMonitor>();
			leftHandMonitor.minimumMoveDistance = LeftHandObject.collider.bounds.size.x / 8f;
			rightHandMonitor.minimumMoveDistance = RightHandObject.collider.bounds.size.x / 8f;
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
					Vector3 pos = RightHandObject.transform.position;
					pos.y += 0.1f;
					RightHandObject.transform.position = pos;
				}
				if(Input.GetKey(KeyCode.S)) {
					Vector3 pos = RightHandObject.transform.position;
					pos.y -= 0.1f;
					RightHandObject.transform.position = pos;
				}
				if(Input.GetKey(KeyCode.A)) {
					Vector3 pos = RightHandObject.transform.position;
					pos.x -= 0.1f;
					RightHandObject.transform.position = pos;
				}
				if(Input.GetKey(KeyCode.D)) {
					Vector3 pos = RightHandObject.transform.position;
					pos.x += 0.1f;
					RightHandObject.transform.position = pos;
				}

				if(Input.GetKey(KeyCode.O)) {
					PlayerHand handData = RightHandObject.GetComponent<PlayerHand>();
					handData.isHandDown = true;
					RightHandObject.renderer.material.mainTexture = OperateTexture;
				} 

				if(Input.GetKey(KeyCode.P)) {
					PlayerHand handData = RightHandObject.GetComponent<PlayerHand>();
					handData.isHandDown = false;
					RightHandObject.renderer.material.mainTexture = HoldTexture;
				} 

			} else {
				syncLeftHand();
				syncRightHand();
			}

		}

		void syncLeftHand() {
			PlayerHand handData = LeftHandObject.GetComponent<PlayerHand>();
			Vector3 pos = leftHandMonitor.GetHandPosition()*kinectToCanvasScale;
			pos.z = PaintBoard.transform.position.z;
			LeftHandObject.transform.position = PaintPositionFromSkeletonPosition(pos, 0);
			handData.prevIsHandDown = handData.isHandDown;
			handData.prevPos = handData.pos;
			handData.pos = pos;

			if(leftHandMonitor.GetHandState() == HandMonitor.HandState.Hold) {
				LeftHandObject.renderer.material.mainTexture = HoldTexture;
				handData.isHandDown = false;
			} else if(leftHandMonitor.GetHandState() == HandMonitor.HandState.Operate) {
				LeftHandObject.renderer.material.mainTexture = OperateTexture;
				handData.isHandDown = true;
			}
		}

		void syncRightHand() {
			PlayerHand handData = RightHandObject.GetComponent<PlayerHand>();
			Vector3 pos = rightHandMonitor.GetHandPosition()*kinectToCanvasScale;
			pos.z = PaintBoard.transform.position.z;
			RightHandObject.transform.position = PaintPositionFromSkeletonPosition(pos, 1);
			handData.prevIsHandDown = handData.isHandDown;
			handData.prevPos = handData.pos;
			handData.pos = pos;

			if(rightHandMonitor.GetHandState() == HandMonitor.HandState.Hold) {
				RightHandObject.renderer.material.mainTexture = HoldTexture;
				handData.isHandDown = false;
			} else if(rightHandMonitor.GetHandState() == HandMonitor.HandState.Operate) {
				RightHandObject.renderer.material.mainTexture = OperateTexture;	
				handData.isHandDown = true;
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
				float ratioX = (skeletonPosition.x - skeletonDrawCenterLeft.x) / skeletonDrawWidth;
				float ratioY = (skeletonPosition.y - skeletonDrawCenterLeft.y) / skeletonDrawHeight;
				float x = canvas.transform.position.x + ratioX * canvasWidth; 
				float y = canvas.transform.position.y + ratioY * canvasHeight; 
				return new Vector3 (x, y, skeletonPosition.z);
			} else if (hand == 1) {
				float ratioX = (skeletonPosition.x - skeletonDrawCenterRight.x) / skeletonDrawWidth;
				float ratioY = (skeletonPosition.y - skeletonDrawCenterRight.y) / skeletonDrawHeight;
				float x = canvas.transform.position.x + ratioX * canvasWidth; 
				float y = canvas.transform.position.y + ratioY * canvasHeight; 
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


			float rightHeight =  Vector3.Distance(SW.bonePos[player, rightShoulderIndex], SW.bonePos[player, rightWristIndex]);
			float leftHeight = Vector3.Distance(SW.bonePos[player, leftShoulderIndex], SW.bonePos[player, leftWristIndex]);
			float centerHeight = Vector3.Distance(SW.bonePos[player, centerHipIndex], SW.bonePos[player, centerShoulderIndex]);

			float [] heights = {rightHeight, leftHeight, centerHeight};
			skeletonDrawHeight = Mathf.Max(heights);
			skeletonDrawWidth = Mathf.Sin((30 * Mathf.PI)/180) * skeletonDrawHeight;

			Vector3 rightCenter = SW.bonePos[player, rightShoulderIndex];
			rightCenter.x += skeletonDrawWidth*.5f;
			rightCenter.y += skeletonDrawHeight*.5f;
			skeletonDrawCenterRight = rightCenter;
			
			Vector3 leftCenter = SW.bonePos[player, leftShoulderIndex];
			leftCenter.x += skeletonDrawWidth*.5f;
			leftCenter.y += skeletonDrawHeight*.5f;
			skeletonDrawCenterLeft = leftCenter;

		}
	}
}


;
