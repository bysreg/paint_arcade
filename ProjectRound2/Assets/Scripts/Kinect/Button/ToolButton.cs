using UnityEngine;
using System.Collections;


namespace Kinect.Button {

	public enum ToolType {
		Colour,
		Shape,
		System
	}

	public delegate void OnButtonSelectedHandler(int id, int handID, ToolType type);
	
	public class ToolButton :MonoBehaviour {
		
		public enum ButtonStatus {
			Selected,
			Unselected,
			Hover
		}


		
		public Texture SelectedTexture;
		public Texture UnselectedTexture;
		public Texture HoverTexture;
		public int id;
		public OnButtonSelectedHandler onButtonSelected;
		public Color DrawColor;
		public ToolType toolType;
		public PlayerHand.ETool EToolID;
		
		private ButtonStatus buttonStatus = ButtonStatus.Unselected;
		private MeshRenderer renderer;
		private Rect validRect;
		private float timer;
		private float hoverTime;
		private int handID;
		
		void Start() {
			float width = transform.collider.bounds.size.x;
			float height = transform.collider.bounds.size.x;
			float x = transform.position.x - width * .5f * .95f;
			float y = transform.position.y - height * .5f * .95f;
			validRect = new Rect (x, y, width * .9f, height * .9f);
			renderer =  transform.GetComponent<MeshRenderer> ();
			timer = 0f;
			hoverTime = 1f;
		}
		
		void UpdateOutlook() {
			if (buttonStatus == ButtonStatus.Unselected) {
				renderer.material.mainTexture = UnselectedTexture;
			} else if(buttonStatus == ButtonStatus.Selected) {
				renderer.material.mainTexture = SelectedTexture;
			} else if(buttonStatus == ButtonStatus.Hover) {
				renderer.material.mainTexture = HoverTexture;
			}
		}

		void Update() {
			if(buttonStatus == ButtonStatus.Hover) {
				timer += Time.deltaTime;
				if(timer > hoverTime) {
					SelectButton();
					onButtonSelected(id, handID, toolType);
				}
			} else {
				timer = 0f;
			}
		}
		
		public void UpdateWithPlayerHands(PlayerHand leftHand, PlayerHand rightHand) {
			if (buttonStatus == ButtonStatus.Hover) {
				if(!validRect.Contains(rightHand.transform.position) && !validRect.Contains(leftHand.transform.position)) {
					buttonStatus = ButtonStatus.Unselected;
				}
			} else if(buttonStatus == ButtonStatus.Selected) {
			
			} else if(buttonStatus == ButtonStatus.Unselected) {
				if(validRect.Contains(rightHand.transform.position) && validRect.Contains(leftHand.transform.position)) {
					buttonStatus = ButtonStatus.Hover;
					handID = 2;
				} else if(validRect.Contains(rightHand.transform.position)) {
					buttonStatus = ButtonStatus.Hover;
					handID = 1;
				} else if(validRect.Contains(leftHand.transform.position)) {
					buttonStatus = ButtonStatus.Hover;
					handID = 0;
				}
			}

			UpdateOutlook();
		}

		public void SelectButton() {
			buttonStatus = ButtonStatus.Selected;
			UpdateOutlook();
		}

		public void UnselectButton() {
			buttonStatus = ButtonStatus.Unselected;
			UpdateOutlook();
		}
	}
}


