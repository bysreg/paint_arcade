using UnityEngine;
using System.Collections;


namespace Kinect.Shape {
	public class Circle : ShapeClass {
		public float diameter;
		public Vector3 center;
		public static string identifier = "Circle";
		
		public Circle(float diameter, Vector3 center) {
			this.diameter = diameter;
			this.center = center;
		}
		
	}
}


