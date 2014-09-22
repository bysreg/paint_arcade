using UnityEngine;
using System.Collections;
using Kinect;

namespace Kinect.Button {


	public class MenuButton : MonoBehaviour {
		public Texture UnselectedTexture;
		public Texture HoverTexture;
		public  ButtonStatus buttonStatus = ButtonStatus.Unselected;


		public MeshRenderer renderer;
		private Rect validRect;
		private float timer;
		private float hoverTime;

		public PlayerHand LeftHand;
		public PlayerHand RightHand;

		public int id;
		// Use this for initialization
		void Start () {
			float width = transform.collider.bounds.size.x;
			float height = transform.collider.bounds.size.y;

			float x = transform.position.x - width * .5f * .95f;
			float y = transform.position.y - height * .5f * .95f;
			validRect = new Rect (x, y, width * .9f, height * .9f);
			timer = 0f;
			hoverTime = 1f;
		}
		
		void UpdateOutlook() {
			if (buttonStatus == ButtonStatus.Unselected) {
				renderer.material.mainTexture = UnselectedTexture;
			} else if(buttonStatus == ButtonStatus.Selected) {
				renderer.material.mainTexture = UnselectedTexture;
			} else if(buttonStatus == ButtonStatus.Hover) {
				renderer.material.mainTexture = HoverTexture;
			}
		}
		
		void Update() {
			//UpdateWithPlayerHands (LeftHand, RightHand);
			if(buttonStatus == ButtonStatus.Hover) {
				timer += Time.deltaTime;
				if(timer > hoverTime) {
					SelectButton();
				}
			} else {
				timer = 0f;
			}

		}
		
		void UpdateWithPlayerHands(PlayerHand leftHand, PlayerHand rightHand) {
			if (buttonStatus == ButtonStatus.Hover) {
				if(!validRect.Contains(rightHand.transform.position) && !validRect.Contains(leftHand.transform.position)) {
					buttonStatus = ButtonStatus.Unselected;
				}
			} else if(buttonStatus == ButtonStatus.Selected) {
				
			} else if(buttonStatus == ButtonStatus.Unselected) {
				if(validRect.Contains(rightHand.transform.position) && validRect.Contains(leftHand.transform.position)) {
					buttonStatus = ButtonStatus.Hover;
				} else if(validRect.Contains(rightHand.transform.position)) {
					buttonStatus = ButtonStatus.Hover;
				} else if(validRect.Contains(leftHand.transform.position)) {
					buttonStatus = ButtonStatus.Hover;
				}
			}
			
			UpdateOutlook();
		}
		
		void SelectButton() {
			buttonStatus = ButtonStatus.Selected;
			UpdateOutlook();

			if (id == 0) {
				Debug.Log("exit");
			} else if (id == 1) {
				Debug.Log("continue");

			}

		}

		void OnMouseDown() {
			//TODO:for temp test
			if (id == 0) {
				Application.Quit();
			} else if (id == 1) {
				SceneManager.instance.asyncLoadScene("Level1");
			}
		}

	}
}