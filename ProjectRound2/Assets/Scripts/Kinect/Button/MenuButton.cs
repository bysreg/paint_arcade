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
		private Vector3 originalScale;
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
			originalScale = transform.localScale;
		}
		
		void UpdateOutlook() {
			if (buttonStatus == ButtonStatus.Unselected) {
				renderer.material.mainTexture = UnselectedTexture;
			} else if(buttonStatus == ButtonStatus.Selected) {
				renderer.material.mainTexture = HoverTexture;
			} else if(buttonStatus == ButtonStatus.Hover) {
				renderer.material.mainTexture = HoverTexture;
			}
		}
		
		void Update() {
			if(buttonStatus == ButtonStatus.Hover) {
				timer += Time.deltaTime;
				if(timer > hoverTime) {
					SelectButton();
					StartCoroutine(PlaySelectAnimation());
				}
			} else {
				timer = 0f;
			}
		}
		
		public void UpdateWithPlayerHand(PlayerHand rightHand) {
			if (buttonStatus == ButtonStatus.Hover) {
				if(!validRect.Contains(rightHand.transform.position)) {
					buttonStatus = ButtonStatus.Unselected;
					rightHand.HideProgressBar();
				}
			} else if(buttonStatus == ButtonStatus.Unselected) {
				if(validRect.Contains(rightHand.transform.position)) {
					buttonStatus = ButtonStatus.Hover;
					rightHand.ShowProgressBar();
				} 
			} else {
			
			}
			
			UpdateOutlook();
		}
		
		void SelectButton() {
			buttonStatus = ButtonStatus.Selected;
			UpdateOutlook();

			if (id == 0) {
				Application.Quit();
			} else if (id == 1) {
				SceneEntry entry = GameObject.FindGameObjectWithTag ("scene_entry").GetComponent<SceneEntry>();
				entry.ProcessDoneButton ();
			}

		}

		void OnMouseDown() {
			if (id == 0) {
				Application.Quit();
			} else if (id == 1) {
				SceneManager.instance.asyncLoadScene("Level1");
				SoundManager.instance.PlayButtonSound (4);
			}
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