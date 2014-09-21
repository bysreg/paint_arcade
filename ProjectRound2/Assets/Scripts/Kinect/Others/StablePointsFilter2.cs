using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StablePointsFilter2 {
	
	private int windowSize;
	private List<Vector3> points;
	private List<Vector2> velocitys;
	private List<Vector2> acclerations;

	private float accelerationX;
	private float accelerationY;
	private bool hasFillUp = false;

	public StablePointsFilter2() {
		points = new List<Vector3>();
		windowSize = 40;
	}
	
	public bool CheckPointValidation(Vector3 point) {
		
		if (points.Count < windowSize) {
			points.Add(point);
			return false;
		} else {
			if(!hasFillUp) {
				hasFillUp = true;
				ComputeVelocitys();
				ComputeAccelerations();
			}
		}
		
		if (Validation (point)) {
			points.Add (point);
			points.RemoveAt(0);
			velocitys.Add(new Vector2((points[windowSize].x-points[windowSize-1].x)/Time.deltaTime,(points[windowSize].y-points[windowSize-1].y)/Time.deltaTime));              
			velocitys.RemoveAt(0);
			acclerations.Add(new Vector2((velocitys[windowSize].x-velocitys[windowSize-1].x)/Time.deltaTime,(velocitys[windowSize].y-velocitys[windowSize-1].y)/Time.deltaTime));              
			acclerations.RemoveAt(0);
			return true;
		}
		
		
		
		return false;
	}
	
	bool Validation(Vector3 point) {
		return (ifXValueValid(point.x))&&(ifYValueValid(point.y)) ;
	}
	

	bool ifXValueValid(float x) {
		float v = (x - points[windowSize].x)/Time.deltaTime;
		float avgA = 0f;
		for(int i =windowSize-6;i<windowSize-1;i++) {
			avgA += acclerations[i].x;
		}
		avgA /= 5;

		if(avgA > 0) {
			if(acclerations[windowSize-1].x > avgA) {
				if(v < velocitys[windowSize-1].x) {
					return false;
				}
			}
		}

		if(avgA < 0) {
			if(acclerations[windowSize-1].x < avgA) {
				if(v > velocitys[windowSize-1].x) {
					return false;
				}
			}
		}

		return true;
	}

	bool ifYValueValid(float y) {
		float v = (y - points[windowSize].y)/Time.deltaTime;
		float avgA = 0f;
		for(int i =windowSize-6;i<windowSize-1;i++) {
			avgA += acclerations[i].y;
		}
		avgA /= 5;
		
		if(avgA > 0) {
			if(acclerations[windowSize-1].y > avgA) {
				if(v < velocitys[windowSize-1].y) {
					return false;
				}
			}
		}
		
		if(avgA < 0) {
			if(acclerations[windowSize-1].y < avgA) {
				if(v > velocitys[windowSize-1].y) {
					return false;
				}
			}
		}
		
		return true;
	}

	void ComputeVelocitys() {
		for(int i=0;i<windowSize-1;i++) {
			velocitys.Add(new Vector2((points[i+1].x-points[i].x)/Time.deltaTime,(points[i+1].y-points[i].y)/Time.deltaTime));              
		}
	}

	void ComputeAccelerations() {
		for(int i=0;i<windowSize-1;i++) {
			acclerations.Add(new Vector2((velocitys[i+1].x-velocitys[i].x)/Time.deltaTime,(velocitys[i+1].y-velocitys[i].y)/Time.deltaTime));              
		}
	}

}
