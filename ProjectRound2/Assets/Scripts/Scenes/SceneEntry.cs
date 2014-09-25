using UnityEngine;
using System.Collections;
using Kinect.Button;


public abstract class SceneEntry : MonoBehaviour {

	public abstract void ProcessDoneButton();

	public void AddToolButtonManager() {
		PlayerHand rightHand = GameObject.FindGameObjectWithTag ("right_hand").GetComponent<PlayerHand>();
		if (rightHand != null) {
			GameObject tbm = new GameObject ("ToolButtonManager");
			tbm.AddComponent<ToolButtonManager> ();
			tbm.tag = "tool_button_manager";
			tbm.GetComponent<ToolButtonManager>().RightHand = rightHand;
			tbm.transform.parent = transform.parent;
		} else {
			Debug.Log("cannot find right hand");
		}

	}

	public void AddMenuButtonManager() {
		PlayerHand rightHand = GameObject.FindGameObjectWithTag ("right_hand").GetComponent<PlayerHand>();
		GameObject mbm = new GameObject ("MenuButtonManager");
		mbm.AddComponent<MenuButtonManager> ();
		mbm.GetComponent<MenuButtonManager>().RightHand = rightHand;
		mbm.tag = "menu_button_manager";
		mbm.transform.parent = transform.parent;
	}
}
