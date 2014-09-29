using UnityEngine;
using System.Collections;
using Kinect;

public class CreditEntry : SceneEntry {
	public void Awake () {
		AddToolButtonManager ();
		base.Awake ();
		ActivateGameInSeconds (2f);
		//SoundManager.instance.PlayBGMusic (4);	
	}
	
	public override void ProcessDoneButton() {

	}
	
	
}
