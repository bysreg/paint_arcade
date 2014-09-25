using UnityEngine;
using System.Collections;
using TheBoxWorld;

namespace TheBoxWorld.Scene {
	public class StartEntry : SceneEntry {
		// Use this for initialization
		void Awake () {
			Screen.showCursor = false;
			AddMenuButtonManager ();
		}

		public override void ProcessDoneButton() {
		
		}
	
	}
}