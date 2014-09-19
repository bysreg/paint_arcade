using UnityEngine;
using System.Collections;
using Kinect.Monitor;

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

			leftHandMonitor.Process();
			rightHandMonitor.Process();
			syncLeftHand();
			syncRightHand();

		}

		void syncLeftHand() {
			Vector3 pos = leftHandMonitor.handPosition;
			pos.z = PaintBoard.transform.position.z;
			LeftHandObject.transform.position = pos;
		}

		void syncRightHand() {
			Vector3 pos = rightHandMonitor.handPosition;
			pos.z = PaintBoard.transform.position.z;
			RightHandObject.transform.position = pos;		
		}
	}
}


