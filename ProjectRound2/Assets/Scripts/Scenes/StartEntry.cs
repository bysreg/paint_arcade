using UnityEngine;
using System.Collections;
using TheBoxWorld;

namespace TheBoxWorld.Scene {
	public class StartEntry : SceneEntry {
		// Use this for initialization
		public void Awake () {
			AddMenuButtonManager ();
			base.Awake ();
			Screen.showCursor = false;
			ActivateGameInSeconds (7f);
		}

		public override void ProcessDoneButton() {
			Debug.Log ("Load Next Scene");
		}
	
	}
}