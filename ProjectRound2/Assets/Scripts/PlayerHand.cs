using UnityEngine;
using System.Collections;
using Kinect.Button;

public class PlayerHand : MonoBehaviour{

	public Vector2 pos = new Vector2();
	public Vector2 prevPos = new Vector2();
	public bool isHandDown;
	public bool prevIsHandDown;
	public ETool tool = ETool.Brush;
	public Color color = Color.blue;

	public enum ETool {Brush = 0, Eraser, None};

	public Texture BrushOperateTexture;
	public Texture BrushHoldTexture;
	public Texture EraserOperateTexture;
	public Texture EraserHoldTexture;
	
	private MeshRenderer renderer;
	private CircularProgressBar progressBar;

	private void Start() {
		renderer = GetComponent<MeshRenderer> ();
		progressBar = transform.FindChild("ProgressBar").GetComponent<CircularProgressBar>();
	}

	public void UpdateOutLook() {
		if (tool == ETool.Brush) {
			if(isHandDown) {
				renderer.material.mainTexture = BrushOperateTexture;
			} else {
				renderer.material.mainTexture = BrushHoldTexture;
			}
		} else if (tool == ETool.Eraser) {
			if(isHandDown) {
				renderer.material.mainTexture = EraserOperateTexture;
			} else {
				renderer.material.mainTexture = EraserHoldTexture;
			}
		}
	}

	public void UpdatePosition(Vector3 pos) {
		GameObject tbmObj = GameObject.FindGameObjectWithTag ("tool_button_manager");
		GameObject mbmObj = GameObject.FindGameObjectWithTag ("menu_button_manager");

		if (tbmObj != null) {
			ToolButtonManager tbm = tbmObj.GetComponent<ToolButtonManager>();
			ToolButton button = tbm.NearstestButton(pos);
			if(button != null) {
				Vector3 p = pos *.4f + button.transform.position *.6f;
				this.pos = p;
				this.transform.position = p;
			} else {
				this.pos = pos;
				this.transform.position = pos;
			}

		} else {
			this.pos = pos;
			this.transform.position = pos;
		}

		if (mbmObj != null) {
			MenuButtonManager mbm = mbmObj.GetComponent<MenuButtonManager>();
			MenuButton button = mbm.NearstestButton(pos);
			if(button != null) {
				Vector3 p = pos *.4f + button.transform.position *.6f;
				this.pos = p;
				this.transform.position = p;
			} else {
				this.pos = pos;
				this.transform.position = pos;
			}
			
		} else {
			this.pos = pos;
			this.transform.position = pos;
		}
	}

	public void ShowProgressBar() {
		progressBar.Activate();
	}

	public void HideProgressBar() {
		progressBar.Deactivate();
	}

}
