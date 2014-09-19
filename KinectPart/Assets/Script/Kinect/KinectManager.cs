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
		// Use this for initialization
		void Start () {

			//TODO: pass data rather than offer SW object
			leftHandMonitor = gameObject.AddComponent<LeftHandMonitor>();
			rightHandMonitor = gameObject.AddComponent<RightHandMonitor>();
			leftHandMonitor.SW = SW;
			rightHandMonitor.SW = SW;
			leftHandMonitor.player = player;
			rightHandMonitor.player = player;

		}
		
		// Update is called once per frame
		void Update () {
			if(player == -1)
				return;

			Dictionary<string, ShapeClass> leftHandResultDict = leftHandMonitor.Process();
			Dictionary<string, ShapeClass> rightHandResultDict = rightHandMonitor.Process();
			handleLeftHandResult(leftHandResultDict);
			handleRightHandResult(rightHandResultDict);
			syncLeftHand();
			syncRightHand();
		}

		void syncLeftHand() {
			Vector3 pos = leftHandMonitor.GetHandPosition();
			pos.z = PaintBoard.transform.position.z;
			LeftHandObject.transform.position = pos;
			if(leftHandMonitor.GetHandState() == HandMonitor.HandState.Hold) {
				LeftHandObject.renderer.material.mainTexture = red;
			} else if(leftHandMonitor.GetHandState() == HandMonitor.HandState.Operate) {
				LeftHandObject.renderer.material.mainTexture = green;
			}
		}

		void syncRightHand() {
			Vector3 pos = rightHandMonitor.GetHandPosition();
			pos.z = PaintBoard.transform.position.z;
			RightHandObject.transform.position = pos;

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
	}
}


