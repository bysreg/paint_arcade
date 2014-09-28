using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PaintArcade.Generic;

namespace Kinect.Button {
	public class ToolButtonManager : MonoBehaviour {
		public PlayerHand RightHand;

		private ToolButton [] buttons;
		private ToolButton brushButton;
		private SizeButton sizeButton;

			// Use this for initialization
		void Start () {
			buttons = FindObjectsOfType(typeof(ToolButton)) as ToolButton[];
			foreach (ToolButton button in buttons) {
				button.onButtonSelected += HandleOnButtonSelected;
				if(button.EToolID == ETool.Brush && button.id == 0) {
					brushButton = button;
				}
				if(button.id == -1) {
					sizeButton = (SizeButton)button;
				}
			}

			SelectFromUserDefault ();
		}

		void SelectFromUserDefault() {
			if (buttons.Length == 0) {
				return;
			}

			ToolButton selectedColorButton = null;
			ToolButton selectedShapeButton = null;

			foreach (ToolButton button in buttons) {
				if(button.toolType == ToolType.Colour && button.id == Consts.SelectedColorButtonID) {
					selectedColorButton = button;
				}

				if(button.toolType == ToolType.Shape && button.id == Consts.SelectedShapeButtonID) {
					selectedShapeButton = button;
				}
			}
			if (selectedColorButton) {
				selectedColorButton.SelectButton();
				HandleColorButtonSelected(selectedColorButton.id, 1);
			}

			if (selectedShapeButton) {
				selectedShapeButton.SelectButton();
				HandleShapeButtonSelected(selectedShapeButton.id, 1);
			}

			if (sizeButton) {
				sizeButton.SelectButtonIndex(Consts.SelectedSizeButtonIndex);
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
							RightHand.tool = ETool.Brush;
							string brushFilePath = "Textures/Cursor/Brush_" + button.ColorName;
							RightHand.BrushOperateTexture = Resources.Load(brushFilePath, typeof(Texture2D)) as Texture2D;
						} 
					}
					
				}
			}
			Consts.SelectedColorButtonID = id; 

		}

		void HandleShapeButtonSelected(int id, int handID) {

			ETool t = ETool.Brush;
			if (id != -1) {

				foreach (ToolButton button in buttons) {
					if(button.id != id) {
						if(button.toolType == ToolType.Shape) {
							button.UnselectButton();
						}
					} else {
						if(button.toolType == ToolType.Shape) {
							t = button.EToolID;
							if (handID == 1) {
								RightHand.tool = button.EToolID;
							} 
						}
						
					}
				}
			}

			if (id == -1) {
				SoundManager.instance.PlayButtonSound (5);
			} else {
				Consts.SelectedShapeButtonID = id; 
			}


			if (t == ETool.Brush && id != -1) {
				SoundManager.instance.PlayButtonSound (0);
			} else if (t == ETool.Eraser){
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

			if (minDistance < .6f)
				return button;

			return null;
		}
	}


}


