using UnityEngine;
using System.Collections;
using Kinect;

namespace TheBoxWorld.Scene {
	public class StartEntry : SceneEntry {
		// Use this for initialization
		public void Awake () {
			AddMenuButtonManager ();
			base.Awake ();
			ActivateGameInSeconds (5f);
			SceneManager.instance.NextSceneName = "Level1";
		}

		public override void ProcessDoneButton() {
			DeactivateGame ();
			Invoke ("LoadNext", .5f);
		}

		void LoadNext() {
			SceneManager.instance.asyncLoadNextSceneWithDelay (0f);
		}
	
	}
}