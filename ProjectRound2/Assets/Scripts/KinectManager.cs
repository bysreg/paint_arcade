using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect.Monitor;
using Kinect.Shape;
using PaintArcade.Generic;

namespace Kinect {
	public class KinectManager : MonoBehaviour {

		public enum PlayerStatus {
			Hold,
			Operate
		}


		public bool SimulateWithKeyBoard;

		private SkeletonWrapper SW;
		public int player;
		public PlayerHand RightHand;
		public PlayerHand LeftHand;

		private ContextTracker contextTracker;
		private PlayerStatus playerStatus = PlayerStatus.Hold;
		private float canvasHeightToWidthRatio;
		private float skeletonDrawHeight;
		private float skeletonDrawWidth;
		private Vector3 skeletonDrawCenter;
		private float canvasWidth;
		private float canvasHeight;
		private GameObject canvas;

		private RightHandMonitor rightHandMonitor;
		private LeftHandMonitor leftHandMonitor;


		// Use this for initialization
		void Start () {

			if (player < 0 || player >= 2) {
				Debug.Log ("Invalid tracking ID.");
				this.enabled = false;
				return ;
			}

			SW = GameObject.FindObjectOfType<SkeletonWrapper> ();

			if (SW == null && !SimulateWithKeyBoard) {
				Debug.Log ("Can't find skeleton wrapper.");
				this.enabled = false;
				return;
			}

			canvas = GameObject.FindGameObjectWithTag("canvas");

			if (canvas == null) {
				Debug.Log ("Cannot find canvas");
				this.enabled = false;
				return;
			}

			contextTracker = gameObject.AddComponent<ContextTracker>();
			contextTracker.AssignSkeletonWrapper (SW, player);

			rightHandMonitor = gameObject.AddComponent<RightHandMonitor>();
			leftHandMonitor = gameObject.AddComponent<LeftHandMonitor>();
		
			canvasWidth = canvas.collider.bounds.size.x;
			canvasHeight = canvas.collider.bounds.size.y;
			canvasHeightToWidthRatio = canvasHeight / canvasWidth;

			skeletonDrawHeight = 0.3f;
			skeletonDrawWidth = 0.3f;

		}
		
		// Update is called once per frame
		void Update () {
			if(SimulateWithKeyBoard) {
				HandleKeyBoardIssue(); 
				return;
			}

			if (contextTracker.ReadSkeleton ()) {
				rightHandMonitor.Process(contextTracker.GetData());
				leftHandMonitor.Process(contextTracker.GetData());
				UpdateSkeletonDrawArea(contextTracker.GetData());
				syncLeftHand();
				syncRightHand();
			}

		}

		void syncLeftHand() {
			if(leftHandMonitor.GetHandState() == HandMonitor.HandState.Hold) {
				playerStatus = PlayerStatus.Operate;
				LeftHand.isHandDown = false;
			} else if(leftHandMonitor.GetHandState() == HandMonitor.HandState.Operate) {
				playerStatus = PlayerStatus.Hold;
				LeftHand.isHandDown = true;
			}

			LeftHand.UpdateOutLook ();

			Vector3 pos = leftHandMonitor.GetHandPosition();
			pos.z = canvas.transform.position.z - 0.05f;
			
			LeftHand.prevIsHandDown = LeftHand.isHandDown;
			LeftHand.prevPos = LeftHand.pos;
			Vector3 adjustedPos = PaintPositionFromSkeletonPosition (pos, 3.2f, 0.45f);
			adjustedPos.x += LeftHand.collider.bounds.size.x * ((adjustedPos.x > 0)?-0.5f:0.5f);
			LeftHand.UpdatePosition(adjustedPos);
		}


		void syncRightHand() {
	
			Vector3 pos = rightHandMonitor.GetHandPosition();
			pos.z = canvas.transform.position.z - 0.05f;

			RightHand.prevIsHandDown = RightHand.isHandDown;
			RightHand.prevPos = RightHand.pos;
			Vector3 adjustedPos = PaintPositionFromSkeletonPosition(pos, -1.7f, 0.45f);
			adjustedPos.x += RightHand.collider.bounds.size.x * ((adjustedPos.x > 0)?-0.5f:0.5f);
			RightHand.UpdatePosition(adjustedPos);

			if(playerStatus == PlayerStatus.Hold) {
				RightHand.isHandDown = false;
				RightHand.UpdateOutLook();
			} else if(playerStatus == PlayerStatus.Operate) {
				RightHand.isHandDown = true;
				RightHand.UpdateOutLook();
			}

			/*
			if(rightHandMonitor.GetHandState() == HandMonitor.HandState.Hold) {
				RightHand.isHandDown = false;
				RightHand.UpdateOutLook();
			} else if(rightHandMonitor.GetHandState() == HandMonitor.HandState.Operate) {
				RightHand.isHandDown = true;
				RightHand.UpdateOutLook();
			}
			*/
		}


		Vector3 PaintPositionFromSkeletonPosition(Vector3 skeletonPosition, float shiftX, float shiftY) {

			float ratioX = (skeletonPosition.x - skeletonDrawCenter.x) / (skeletonDrawWidth*.5f);
			float ratioY = (skeletonPosition.y - skeletonDrawCenter.y) / (skeletonDrawHeight*.5f);

			ratioX += shiftX; // dont let hand go across body, sensor problem, shaky

			ratioY += shiftY; // from realtime test

			if(ratioX > 1f) ratioX = 1f;
			if(ratioY > 1f) ratioY = 1f;
			if(ratioX < -1f) ratioX = -1f;
			if(ratioY < -1f) ratioY = -1f;


			float x = canvas.transform.position.x + ratioX * canvasWidth*.5f * Consts.kinectToCanvasScale; 
			float y = canvas.transform.position.y + ratioY * canvasHeight*.5f * Consts.kinectToCanvasScale; 

			return new Vector3 (x, y, skeletonPosition.z);
	
		}

		void UpdateSkeletonDrawArea(Dictionary <int, List <ContextPoint>> data) {
			if (data == null || data.Count == 0) return;
			
			int maxIndex = -1;
			
			for (int i = 0; i < (int)Kinect.NuiSkeletonPositionIndex.Count; i ++) {
				if(data.ContainsKey(i)) {
					maxIndex = data[i].Count-1; 
					break;
				}
			}
			
			if(maxIndex < 0) return;

			//Debug.Log(data[(int)Kinect.NuiSkeletonPositionIndex.HandRight][maxIndex].Position.x + " " + data[(int)Kinect.NuiSkeletonPositionIndex.HandRight][maxIndex].Position.y);

			int rightShoulderIndex = (int)Kinect.NuiSkeletonPositionIndex.ShoulderRight;
			int centerShoulderIndex = (int)Kinect.NuiSkeletonPositionIndex.ShoulderCenter;
			int centerHipIndex = (int)Kinect.NuiSkeletonPositionIndex.HipCenter;

			Vector3 hipPosition = data[centerHipIndex][maxIndex].Position;
			Vector3 centerShoulderPosition = data[centerShoulderIndex][maxIndex].Position;
			Vector3 rightShoulderPosition = data[rightShoulderIndex][maxIndex].Position;

			skeletonDrawHeight = Vector3.Distance (hipPosition, centerShoulderPosition);
			skeletonDrawWidth = Mathf.Sin((60 * Mathf.PI)/180) * skeletonDrawHeight;

			skeletonDrawCenter = centerShoulderPosition;

			if(skeletonDrawHeight < 0.3f)
				skeletonDrawHeight = 0.3f;

			if(skeletonDrawWidth < 0.3f)
				skeletonDrawWidth = 0.3f;

			//Debug.Log ("skeletonDrawCenter: " + skeletonDrawCenter);
			//Debug.Log ("skeletonDrawHeight: " + skeletonDrawHeight);
			//Debug.Log ("skeletonDrawWidth: " + skeletonDrawWidth);


		}

		private void HandleKeyBoardIssue(){
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
		}
	}
}


;
