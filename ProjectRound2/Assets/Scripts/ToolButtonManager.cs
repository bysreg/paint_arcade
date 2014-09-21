using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Kinect.Button {
	public class ToolButtonManager : MonoBehaviour {
		public PlayerHand LeftHand;
		public PlayerHand RightHand;
		private ToolButton [] buttons;
			// Use this for initialization
		void Start () {
			buttons = FindObjectsOfType(typeof(ToolButton)) as ToolButton[];
			foreach (ToolButton button in buttons) {
				button.onButtonSelected += HandleOnButtonSelected;
			}
		}

		void HandleOnButtonSelected(int id, int handID, ToolType type) {
			if (type == ToolType.Colour) {
				HandleColorButtonSelected(id, handID);
			} else if(type == ToolType.Shape) {
				HandleShapeButtonSelected(id, handID);
			}

		}
		
		// Update is called once per frame
		void Update () {
			foreach (ToolButton button in buttons) {
				button.UpdateWithPlayerHands(LeftHand, RightHand);
			}
		}

		void UnselectButtons() {
			foreach (ToolButton button in buttons) {
				button.UnselectButton();
			}
		}

		void HandleColorButtonSelected(int id, int handID) {
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
			foreach (ToolButton button in buttons) {
				if(button.id != id) {
					if(button.toolType == ToolType.Shape) {
						button.UnselectButton();
					}
				}
				else {
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
		}
	}


}


