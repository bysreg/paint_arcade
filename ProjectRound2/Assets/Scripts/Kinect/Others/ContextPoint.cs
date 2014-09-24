/// <summary>
/// ContextPoint.
/// Modified by Thomas for PaintArcade Project, Approved by original author Chiu.
/// </summary>
/// 
using UnityEngine;
using System.Collections;
using System;

namespace Kinect {
	public class ContextPoint {
		public DateTime Time { get; set;}
		public Vector3 Position { get; set; }

		public ContextPoint () {}

		public ContextPoint (Vector3 position, DateTime time) {
			this.Position = position;
			this.Time = time;
		}
	}
}