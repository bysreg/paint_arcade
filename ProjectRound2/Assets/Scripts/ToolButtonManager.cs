using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Kinect.Button {
	public class ToolButtonManager : MonoBehaviour {
		public PlayerHand RightHand;

		private ToolButton [] buttons;
		private PlayerHand[] mouseHands;
		private bool simulateWithMouse; // need to fetch it from GameController

		private ToolButton brushButton;

			// Use this for initialization
		void Start () {
			buttons = FindObjectsOfType(typeof(ToolButton)) as ToolButton[];
			foreach (ToolButton button in buttons) {
				button.onButtonSelected += HandleOnButtonSelected;
				if(button.EToolID == PlayerHand.ETool.Brush) {
					brushButton = button;
				}
			}

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

		void HandleOnButtonSelected(int id, int handID, ToolType type) {
			if (type == ToolType.Colour) {
				HandleColorButtonSelected(id, handID);
			} else if(type == ToolType.Shape) {
				HandleShapeButtonSelected(id, handID);
			} else if(type == ToolType.System) {
				HandleSystemButtonSelected(id, handID);
			}

			RightHand.UpdateOutLook ();
		}
		
		// Update is called once per frame
		void Update () {
			foreach (ToolButton button in buttons) {
				button.UpdateWithPlayerHand(RightHand);
				if(simulateWithMouse)
				{
					button.UpdateWithPlayerHand(mouseHands[0]);
				}
			}
		}

		void UnselectButtons() {
			foreach (ToolButton button in buttons) {
				button.UnselectButton();
			}
		}

		void HandleColorButtonSelected(int id, int handID) {
			SoundManager.instance.PlayButtonSound (2);
			foreach (ToolButton button in buttons) {
				if(button.id != id) {
					if(button.toolType == ToolType.Colour) {
						button.UnselectButton();
					}
				}
				else {
					if(button.toolType == ToolType.Colour) {
						HandleShapeButtonSelected(brushButton.id, handID);
						brushButton.SelectButton();
						if (handID == 1) {
							RightHand.color = button.DrawColor;
							RightHand.color.a = 1;
							RightHand.tool = PlayerHand.ETool.Brush;
						} 
					}
					
				}
			}
		}

		void HandleShapeButtonSelected(int id, int handID) {
			PlayerHand.ETool t = PlayerHand.ETool.Brush;
			foreach (ToolButton button in buttons) {
				if(button.id != id) {
					if(button.toolType == ToolType.Shape) {
						button.UnselectButton();
					}
				} else {
					t = button.EToolID;
					if(button.toolType == ToolType.Shape) {
						if (handID == 1) {
							RightHand.tool = button.EToolID;
						} 
					}

				}
			}

			if (t == PlayerHand.ETool.Brush) {
				SoundManager.instance.PlayButtonSound (0);
			} else if (t == PlayerHand.ETool.Eraser){
				SoundManager.instance.PlayButtonSound (3);
			}
		}

		void HandleSystemButtonSelected(int id, int handID) {
			SceneEntry entry = GameObject.FindGameObjectWithTag ("scene_entry").GetComponent<SceneEntry>();
			entry.ProcessDoneButton ();
		}

		public ToolButton NearstestButton(Vector3 pos) {
			float minDistance = 999f;
			ToolButton button = buttons [0];
			foreach (ToolButton b in buttons) {
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


