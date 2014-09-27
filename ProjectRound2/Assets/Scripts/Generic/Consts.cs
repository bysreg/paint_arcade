using UnityEngine;
using System.Collections;

namespace PaintArcade.Generic {
	public class Consts {
		public static float KinectDrawingSmoothFactor = 0.2f; // 0~1.0
		public static float ValidOperateDistanceScale = 1f; //hand hold/draw state
		public static float kinectToCanvasScale = 1.2f;
		public static int BrushSizeSmall = 4;
		public static int BrushSizeMiddle = 8;
		public static int BrushSizeLarge = 13;
		public static int BrushSizeVeryLarge = 20;
		public static int EraserSizeSmall = 6;
		public static int EraserSizeMiddle = 10;
		public static int EraserSizeLarge = 17;
		public static int EraserSizeVeryLarge = 24;

		public static float EraserSmallScale = 1f;
		public static float BrushSmallScale = 1f;
		public static float EraserMiddleScale = 1.2f;
		public static float BrushMiddleScale = 1.2f;
		public static float EraserLargeScale = 1.8f;
		public static float BrushLargeScale = 1.8f;
		public static float EraserVeryLargeScale = 2.3f;
		public static float BrushVeryLargeScale = 2.3f;

		public static float EraserCurrentExtraScale = 1f;
		public static float BrushCurrentExtraScale = 1f;

		public static int SelectedShapeButtonID = 0;
		public static int SelectedColorButtonID = 0;
		public static int SelectedSizeButtonIndex = 0;

	}
}
