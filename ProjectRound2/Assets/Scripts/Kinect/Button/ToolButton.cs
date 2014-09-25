using UnityEngine;
using System.Collections;


namespace Kinect.Button {

	public enum ToolType {
		Colour,
		Shape,
		System
	}

	public enum ButtonStatus {
		Selected,
		Unselected,
		Hover
	}

	public delegate void OnButtonSelectedHandler(int id, int handID, ToolType type);
	
	public class ToolButton :MonoBehaviour {

		public Texture SelectedTexture;
		public Texture UnselectedTexture;
		public Texture HoverTexture;
		public int id;
		public OnButtonSelectedHandler onButtonSelected;
		public Color DrawColor;
		public ToolType toolType;
		public PlayerHand.ETool EToolID;
		
		public ButtonStatus buttonStatus = ButtonStatus.Unselected;
		private MeshRenderer renderer;
		private Rect validRect;
		private float timer;
		private float hoverTime;
		private int handID;
		private Vector3 originalScale;
		
		void Start() {
			float width = transform.collider.bounds.size.x;
			float height = transform.collider.bounds.size.x;
			float x = transform.position.x - width * .5f * .95f;
			float y = transform.position.y - height * .5f * .95f;
			validRect = new Rect (x, y, width * .9f, height * .9f);
			renderer =  transform.GetComponent<MeshRenderer> ();
			timer = 0f;
			hoverTime = 1f;
			originalScale = transform.localScale;
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
					StartCoroutine(PlaySelectAnimation());
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

		public void UpdateWithPlayerHand(PlayerHand rightHand) {
			if (buttonStatus == ButtonStatus.Hover) {
				if(!validRect.Contains(rightHand.transform.position)) {
					buttonStatus = ButtonStatus.Unselected;
					rightHand.HideProgressBar();
				}
			} else if(buttonStatus == ButtonStatus.Selected) {
				
			} else if(buttonStatus == ButtonStatus.Unselected) {
				if(validRect.Contains(rightHand.transform.position)) {
					buttonStatus = ButtonStatus.Hover;
					handID = 1;
					rightHand.ShowProgressBar();
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

		IEnumerator PlaySelectAnimation() {
			float x = 0f;
			while(x<0.5f) {
				transform.localScale = Vector3.Lerp(originalScale *0.9f, originalScale*1.05f, x*2f);
				x+=Time.deltaTime;
			}
			yield return new WaitForSeconds(.2f);

			x=0;

			while(x<0.1f) {
				transform.localScale = Vector3.Lerp(transform.localScale, originalScale*1, x*10f);
				x+=Time.deltaTime;
			}
		}
	}
}


