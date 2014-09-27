using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Kinect.Button {
	public class MenuButtonManager : MonoBehaviour {
		public PlayerHand RightHand;

		private MenuButton [] buttons;

		void Start () {
			buttons = FindObjectsOfType(typeof(MenuButton)) as MenuButton[];
		}
		
		// Update is called once per frame
		void Update () {
			foreach (MenuButton button in buttons) {
				button.UpdateWithPlayerHand(RightHand);
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


