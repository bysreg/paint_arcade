using UnityEngine;
using System.Collections;

namespace TheBoxWorld.Scene {
	public class SceneFadeIn : MonoBehaviour {
		
		public Texture2D fadeTexture;
		private int fadeTime = 2;
		private float startTime;
		
		void Start() {
			startTime = Time.time;
		}
		
		void Update(){
			if(Time.time - startTime >= fadeTime){
				Destroy(gameObject);
			}
		}
		
		void OnGUI(){
			Color color = Color.white;
			color.a = Mathf.Lerp(1.0f, 0.0f, (Time.time-startTime)/fadeTime);;
			GUI.color = color;
			GUI.DrawTexture(new Rect( 0, 0, Screen.width,Screen.height ), fadeTexture);
		}
	}
}

