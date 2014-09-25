using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Kinect.Button {
	public class MenuButtonManager : MonoBehaviour {
		public PlayerHand RightHand;

		private MenuButton [] buttons;
		private PlayerHand[] mouseHands;
		private bool simulateWithMouse; // need to fetch it from GameController

		void Start () {
			buttons = FindObjectsOfType(typeof(MenuButton)) as MenuButton[];
			GameObject gObj = GameObject.Find ("GameController");
			if(gObj != null) {
				GameController gameController = gObj.GetComponent<GameController> ();
				simulateWithMouse = gameController.simulateWithMouse;
				if (gameController.simulateWithMouse) {
					mouseHands = gameController.GetPlayerHands();
				}
			} else {
				Debug.Log("Cannot find game controller");
			}

		}
		
		// Update is called once per frame
		void Update () {
			foreach (MenuButton button in buttons) {
				if(simulateWithMouse){
					button.UpdateWithPlayerHand(mouseHands[0]);
				} else {
					button.UpdateWithPlayerHand(RightHand);
				}
			}
		}

		public MenuButton NearstestButton(Vector3 pos) {
			float minDistance = 999f;
			MenuButton button = buttons [0];
			foreach (MenuButton b in buttons) {
				float d = Vector3.Distance(pos, b.transform.position);
				if(d<minDistance) {
					minDistance = d;
					button = b;
				}
			}

			if (minDistance < .5f)
				return button;

			return null;
		}
	}


}


