using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Kinect.Button {
	public class ToolButtonManager : MonoBehaviour {
		public PlayerHand LeftHand;
		public PlayerHand RightHand;

		private ToolButton [] buttons;
		private PlayerHand[] mouseHands;
		private bool simulateWithMouse; // need to fetch it from GameController

			// Use this for initialization
		void Start () {
			buttons = FindObjectsOfType(typeof(ToolButton)) as ToolButton[];
			foreach (ToolButton button in buttons) {
				button.onButtonSelected += HandleOnButtonSelected;
			}

			GameController gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
			simulateWithMouse = gameController.simulateWithMouse;
			if (gameController.simulateWithMouse) 
			{
				mouseHands = gameController.GetPlayerHands();
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

			LeftHand.UpdateOutLook ();
			RightHand.UpdateOutLook ();
		}
		
		// Update is called once per frame
		void Update () {
			foreach (ToolButton button in buttons) {
				button.UpdateWithPlayerHands(LeftHand, RightHand);
				if(simulateWithMouse)
				{
					button.UpdateWithPlayerHands(mouseHands[0], mouseHands[1]);
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
						if (handID == 2) {
							LeftHand.color = button.DrawColor;
							RightHand.color = button.DrawColor;
						} else if (handID == 1) {
							RightHand.color = button.DrawColor;
						} else if (handID == 0) {
							LeftHand.color = button.DrawColor;
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
						if (handID == 2) {
							LeftHand.tool = button.EToolID;
							RightHand.tool = button.EToolID;
						} else if (handID == 1) {
							RightHand.tool = button.EToolID;
						} else if (handID == 0) {
							LeftHand.tool = button.EToolID;
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
			foreach (ToolButton button in buttons) {
				button.UnselectButton();
			}

			Debug.Log("Select Continue");
		}
	}


}


