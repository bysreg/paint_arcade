using UnityEngine;
using System.Collections;
using Kinect.Button;


public abstract class SceneEntry : MonoBehaviour {

	public abstract void ProcessDoneButton();
	protected GameObject kinectObj;
	protected GameObject tbmObj;
	protected GameObject mbmObj;
	protected GameObject gcObj;
	public bool ShowCursor;

	protected void Awake() {
		kinectObj = GameObject.FindGameObjectWithTag("kinect");
		tbmObj = GameObject.FindGameObjectWithTag("tool_button_manager");
		mbmObj = GameObject.FindGameObjectWithTag("menu_button_manager");
		gcObj = GameObject.FindGameObjectWithTag ("GameController");
		Screen.showCursor = ShowCursor;
	}

	public void AddToolButtonManager() {
		GameObject rhObj = GameObject.FindGameObjectWithTag ("right_hand");
		if (rhObj != null) {
			PlayerHand rightHand = rhObj.GetComponent<PlayerHand>();
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
		GameObject rhObj = GameObject.FindGameObjectWithTag ("right_hand");
		if (rhObj != null) {
			PlayerHand rightHand = rhObj.GetComponent<PlayerHand>();
			GameObject mbm = new GameObject ("MenuButtonManager");
			mbm.AddComponent<MenuButtonManager> ();
			mbm.GetComponent<MenuButtonManager>().RightHand = rightHand;
			mbm.tag = "menu_button_manager";
			mbm.transform.parent = transform.parent;
		}
	}

	public void ActivateGameInSeconds(float t) {
		EnableKinectInSeconds(t);
		EnableMenuButtonManagerInSeconds(t);
		EnableToolButtonManagerInSeconds(t);
		EnableGameControllerInSeconds(t+1f);
	}

	public void DeactivateGame() {
		DisableKinect();
		DisableMenuButtonManager();
		DisableToolButtonManager();
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

	void DisableMenuButtonManager() {
		if (mbmObj != null) {
			mbmObj.SetActive(false);
		}
	}

	void EnableToolButtonManagerInSeconds(float t) {
		if (tbmObj != null) {
			tbmObj.SetActive(false);
			Invoke("EnableToolButtonManager", t);
		}
		
	}

	void EnableGameControllerInSeconds(float t) {
		if (gcObj != null) {
			Invoke("EnableGameController", t);
		}
		
	}

	void EnableGameController() {
		if (gcObj != null) {
			GameController controller = gcObj.GetComponent<GameController>();
			if(controller != null) { 
				controller.SetActivate();
			}
		}
	}
	
	void EnableToolButtonManager() {
		if (tbmObj != null) {
			tbmObj.SetActive(true);
		}
	}

	void DisableToolButtonManager() {
		if (tbmObj != null) {
			tbmObj.SetActive(false);
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

	void DisableKinect() {
		if (kinectObj != null) {
			kinectObj.SetActive(false);
		}
	}

	void OnDestroy() {
		CancelInvoke ("EnableKinect");
		CancelInvoke ("EnableToolButtonManager");

	}
}
