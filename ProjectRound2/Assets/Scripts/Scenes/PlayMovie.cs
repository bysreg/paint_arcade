using UnityEngine;
using System.Collections;

namespace Kinect.Scene {
	public class PlayMovie : MonoBehaviour {
		
		public MovieTexture movieTexture;
		public Texture StartTexture;
		public GameObject ButtonPanel;
		public GameObject FadeInEffect;

		private bool canMove = false;
		private float progress = 0f;
		// Use this for initialization
		void Start () {
			renderer.material.mainTexture = movieTexture;
			Invoke ("playMovie", .5f);
			Invoke ("showStart", movieTexture.duration);
			
		}
		
		void playMovie() {
			movieTexture.Play ();
		}
		
		void showStart() {
			GameObject effect = Instantiate(FadeInEffect, Vector3.zero, Quaternion.identity) as GameObject;
			effect.transform.parent = transform;
			renderer.material.mainTexture = StartTexture;
			canMove = true;
			Screen.showCursor = true;
		}

		void Update() {
			if (canMove) {
				progress += Time.deltaTime / 3;
				ButtonPanel.transform.position = Vector3.Lerp(ButtonPanel.transform.position, new Vector3(0,0,-0.1f), progress);
			}

			
		}
	}


}

