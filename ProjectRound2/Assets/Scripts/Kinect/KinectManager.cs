﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect.Monitor;
using Kinect.Shape;
using PaintArcade.Generic;

namespace Kinect {
	public class KinectManager : MonoBehaviour {


		public bool SimulateWithKeyBoard;

		public SkeletonWrapper SW;
		public int player;
		public PlayerHand RightHand;
		public GameObject PaintBoard;
		public GameObject Canvas;


		private ContextTracker contextTracker;
		private float canvasHeightToWidthRatio;
		private float skeletonDrawHeight;
		private float skeletonDrawWidth;
		private Vector3 skeletonDrawCenter;
		private float canvasWidth;
		private float canvasHeight;

		private RightHandMonitor rightHandMonitor;



		// Use this for initialization
		void Start () {

			if (player < 0 || player >= 2) {
				Debug.Log ("Invalid tracking ID.");
				this.enabled = false;
				return ;
			}
			
			if (SW == null) {
				Debug.Log ("Can't find skeleton wrapper.");
				this.enabled = false;
				return;
			}

			if (Canvas == null) {
				Debug.Log ("Cannot find canvas");
				this.enabled = false;
				return;
			}

			rightHandMonitor = gameObject.AddComponent<RightHandMonitor>();

			contextTracker = gameObject.AddComponent<ContextTracker>();
			contextTracker.AssignSkeletonWrapper (SW, player);

			canvasWidth = Canvas.collider.bounds.size.x;
			canvasHeight = Canvas.collider.bounds.size.y;
			canvasHeightToWidthRatio = canvasHeight / canvasWidth;
		}
		
		// Update is called once per frame
		void Update () {
			if (contextTracker.ReadSkeleton ()) {
				rightHandMonitor.Process(contextTracker.GetData());
				UpdateSkeletonDrawArea(contextTracker.GetData());
				
				if (SimulateWithKeyBoard) {
					HandleKeyBoardIssue(); 
				} else {
					syncRightHand();
				}
			}

		}

		void syncRightHand() {

			Vector3 pos = rightHandMonitor.GetHandPosition();
			pos.z = Canvas.transform.position.z;

			RightHand.transform.position = PaintPositionFromSkeletonPosition(pos);
			RightHand.prevIsHandDown = RightHand.isHandDown;
			RightHand.prevPos = RightHand.pos;
			RightHand.UpdatePosition(RightHand.transform.position);

			if(rightHandMonitor.GetHandState() == HandMonitor.HandState.Hold) {
				RightHand.isHandDown = false;
				RightHand.UpdateOutLook();
			} else if(rightHandMonitor.GetHandState() == HandMonitor.HandState.Operate) {
				RightHand.isHandDown = true;
				RightHand.UpdateOutLook();
			}
		}


		Vector3 PaintPositionFromSkeletonPosition(Vector3 skeletonPosition) {

			float ratioX = (skeletonPosition.x - skeletonDrawCenter.x) / (skeletonDrawWidth*.5f);
			float ratioY = (skeletonPosition.y - skeletonDrawCenter.y) / (skeletonDrawHeight*.5f);

			ratioX -= 1.5f; // dont let hand go across body, sensor problem, shaky

			if(ratioX > 1f) ratioX = 1f;
			if(ratioY > 1f) ratioY = 1f;
			if(ratioX < -1f) ratioX = -1f;
			if(ratioY < -1f) ratioY = -1f;

			//Debug.Log (ratioX + ", " + ratioY);


			float x = Canvas.transform.position.x + ratioX * canvasWidth*.5f * Consts.kinectToCanvasScale; 
			float y = Canvas.transform.position.y + ratioY * canvasHeight*.5f * Consts.kinectToCanvasScale; 

			return new Vector3 (x, y, skeletonPosition.z);
	
		}

		void UpdateSkeletonDrawArea(Dictionary <int, List <ContextPoint>> data) {
			int maxIndex = data[0].Count-1;
			if (maxIndex <= 0)
				return;


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
