using UnityEngine;
using System.Collections;
using Kinect.Button;


public abstract class SceneEntry : MonoBehaviour {

	public abstract void ProcessDoneButton();
	protected GameObject kinectObj;
	protected GameObject tbmObj;
	protected GameObject mbmObj;

	protected void Awake() {
		kinectObj = GameObject.FindGameObjectWithTag("kinect");
		tbmObj = GameObject.FindGameObjectWithTag("tool_button_manager");
		mbmObj = GameObject.FindGameObjectWithTag("menu_button_manager");
	}

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

	public void ActivateGameInSeconds(float t) {
		EnableKinectInSeconds(t);
		EnableMenuButtonManagerInSeconds(t);
		EnableToolButtonManagerInSeconds(t);
	}

	void EnableMenuButtonManagerInSeconds(float t) {
		if (mbmObj != null) {
			mbmObj.SetActive(false);
			Invoke("EnableMenuButtonManager", t);
		}
	}
	
	void EnableMenuButtonManager() {
		if (mbmObj != null) {
			mbmObj.SetActive(true);
		}
	}

	void EnableToolButtonManagerInSeconds(float t) {
		if (tbmObj != null) {
			tbmObj.SetActive(false);
			Invoke("EnableToolButtonManager", t);
		}
		
	}
	
	void EnableToolButtonManager() {
		if (tbmObj != null) {
			tbmObj.SetActive(true);
		}
	}


	void EnableKinectInSeconds(float t) {
		if (kinectObj != null) {
			kinectObj.SetActive(false);
			Invoke("EnableKinect", t);
		}

	}

	void EnableKinect() {
		if (kinectObj != null) {
			kinectObj.SetActive(true);
		}
	}

	void OnDestroy() {
		CancelInvoke ("EnableKinect");
		CancelInvoke ("EnableToolButtonManager");

	}
}
