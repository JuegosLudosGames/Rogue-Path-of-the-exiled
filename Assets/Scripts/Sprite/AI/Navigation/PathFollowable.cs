using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowable : MonoBehaviour {

	//list of points in order the follower follows
	public PathPoint[] path;

	//the point the follower is currently running
	private int currentPoint = 0;

	//the range to the point required to change point objective
	public float rangeOfChange = 1.0f;

	//returns the current point 
	public PathPoint getCurrent() {
		return path [currentPoint];
	}

	//iriterates through points and returns the current
	public PathPoint next() {
		IriteratePoints ();
		return getCurrent ();
	}

	//runs through points
	void IriteratePoints() {
		//iriterates the array
		currentPoint++;

		//checks if reached the end
		if(currentPoint >= path.Length) 
			currentPoint = 0;
	}
}
