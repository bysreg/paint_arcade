﻿using UnityEngine;
using System.Collections;


namespace Kinect.Button {
	public delegate void OnButtonSelectedHandler(int id);
	
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
		
		private ButtonStatus buttonStatus = ButtonStatus.Unselected;
		private MeshRenderer renderer;
		private Rect validRect;
		private float timer;
		private float hoverTime;
		
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
					onButtonSelected(id);
				}
			} else {
				timer = 0f;
			}
		}
		
		public void UpdateWithPlayerHands(PlayerHand leftHand, PlayerHand rightHand) {
			if (buttonStatus == ButtonStatus.Hover) {
				if(!validRect.Contains(rightHand.transform.position)) {
					buttonStatus = ButtonStatus.Unselected;
				}
			
			} else if(buttonStatus == ButtonStatus.Selected) {
			
			} else if(buttonStatus == ButtonStatus.Unselected) {
				if(validRect.Contains(rightHand.transform.position)) {
					buttonStatus = ButtonStatus.Hover;
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

