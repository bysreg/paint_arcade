using UnityEngine;
using System.Collections;
using PaintArcade.Generic;

namespace Kinect.Button {

	public class SizeButton : ToolButton{
		public Texture BrushSizeSmallTexture;
		public Texture BrushSizeMiddleTexture;
		public Texture BrushSizeLargeTexture;
		public Texture BrushSizeVeryLargeTexture;

		private int brushIndex = 0;
		private int maxIndex = 3;

		public override void SelectButton() {
			buttonStatus = ButtonStatus.Selected;

			brushIndex ++;
			if (brushIndex > maxIndex) {
				brushIndex = 0;
			}

			if (brushIndex == 0) {
				UnselectedTexture = BrushSizeSmallTexture;
				SelectedTexture = BrushSizeSmallTexture;
				HoverTexture = BrushSizeSmallTexture;
				GameObject.Find ("GameController").GetComponent<GameController>().ChangeBrushRadius(Consts.BrushSizeSmall);
				GameObject.Find ("GameController").GetComponent<GameController>().ChangeEraserRadius(Consts.EraserSizeSmall);
				Consts.BrushCurrentExtraScale = Consts.BrushSmallScale;
				Consts.EraserCurrentExtraScale = Consts.EraserSmallScale;
			} else if(brushIndex == 1) {
				UnselectedTexture = BrushSizeMiddleTexture;
				SelectedTexture = BrushSizeMiddleTexture;
				HoverTexture = BrushSizeMiddleTexture;
				GameObject.Find ("GameController").GetComponent<GameController>().ChangeBrushRadius(Consts.BrushSizeMiddle);
				GameObject.Find ("GameController").GetComponent<GameController>().ChangeEraserRadius(Consts.EraserSizeMiddle);
				Consts.BrushCurrentExtraScale = Consts.BrushMiddleScale;
				Consts.EraserCurrentExtraScale = Consts.EraserMiddleScale;
			} else if(brushIndex == 2) {
				UnselectedTexture = BrushSizeLargeTexture;
				SelectedTexture = BrushSizeLargeTexture;
				HoverTexture = BrushSizeLargeTexture;
				GameObject.Find ("GameController").GetComponent<GameController>().ChangeBrushRadius(Consts.BrushSizeLarge);
				GameObject.Find ("GameController").GetComponent<GameController>().ChangeEraserRadius(Consts.EraserSizeLarge);
				Consts.BrushCurrentExtraScale = Consts.BrushLargeScale;
				Consts.EraserCurrentExtraScale = Consts.EraserLargeScale;
			} else if(brushIndex == 3) {
				UnselectedTexture = BrushSizeVeryLargeTexture;
				SelectedTexture = BrushSizeVeryLargeTexture;
				HoverTexture = BrushSizeVeryLargeTexture;
				GameObject.Find ("GameController").GetComponent<GameController>().ChangeBrushRadius(Consts.BrushSizeVeryLarge);
				GameObject.Find ("GameController").GetComponent<GameController>().ChangeEraserRadius(Consts.EraserSizeVeryLarge);
				Consts.BrushCurrentExtraScale = Consts.BrushVeryLargeScale;
				Consts.EraserCurrentExtraScale = Consts.EraserVeryLargeScale;
			}

			Consts.SelectedSizeButtonIndex = brushIndex;

			UpdateOutlook();
			Invoke ("ReactivateButton", .3f);
		}

		void ReactivateButton() {
			buttonStatus = ButtonStatus.Unselected;
		}

		void OnDestroy() {
			CancelInvoke("ReactivateButton");
		}

		public void SelectButtonIndex(int index) {
			brushIndex = index - 1;
			if (brushIndex < 0)
				brushIndex = maxIndex;
			SelectButton ();
		}
	}

}


