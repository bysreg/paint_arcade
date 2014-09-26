using UnityEngine;
using System.Collections;


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
			} else if(brushIndex == 1) {
				UnselectedTexture = BrushSizeMiddleTexture;
				SelectedTexture = BrushSizeMiddleTexture;
				HoverTexture = BrushSizeMiddleTexture;
			} else if(brushIndex == 2) {
				UnselectedTexture = BrushSizeLargeTexture;
				SelectedTexture = BrushSizeLargeTexture;
				HoverTexture = BrushSizeLargeTexture;
			} else if(brushIndex == 3) {
				UnselectedTexture = BrushSizeVeryLargeTexture;
				SelectedTexture = BrushSizeVeryLargeTexture;
				HoverTexture = BrushSizeVeryLargeTexture;
			}

			UpdateOutlook();
			Invoke ("ReactivateButton", .3f);
		}

		void ReactivateButton() {
			buttonStatus = ButtonStatus.Unselected;
		}

		void OnDestroy() {
			CancelInvoke("ReactivateButton");
		}
	}

}


