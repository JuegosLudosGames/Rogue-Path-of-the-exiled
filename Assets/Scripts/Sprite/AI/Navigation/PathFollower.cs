using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour {

	//a followable path the follower is connected to
	public PathFollowable path;

	//if the follower is in the state of waiting
	private bool hasStartedWaiting = false;

	//the time left for the follower to leave the wait state
	private float timeLeftToWait;

	//returns the vector of the next point
	public Vector3 getCurrentPathPoint (Vector3 currentPos) {

		//verifies that there is a path
		if (path == null)
			return Vector3.zero;

		//grabs the path point
		PathPoint point = path.getCurrent ();

		//checks if able to change path
		if(point.isInRange(currentPos, path.rangeOfChange)) {
			return path.next ().transform.position;
		} else {
			return point.transform.position;
		}
	}

	//checks if there is an avilable path to follow
	public bool hasPathToFollow() {
		return (path == null) ? false : true;
	}

	//checks if should be waiting at the point
	bool shouldWaitAtPoint(PathPoint point) {
		
		//checks if should wait at current point
		if (point.waitAtPoint) {
			//checks if already started waiting at point
			if (hasStartedWaiting) {
				timeLeftToWait -= Time.deltaTime;
				//checks if enough time passed
				if (timeLeftToWait <= 0) {
					hasStartedWaiting = false;
					return false;
				} else {
					return true;
				}
			} else {
				timeLeftToWait = point.timeOfWait;
				hasStartedWaiting = true;
				return true;
			}
		}
		return false;
	}

	//checks if should be moving
	public bool shouldMove(Vector3 currentPos) {
		//checks if the current point requires to wait
		if (path.getCurrent ().waitAtPoint) {
			//checks if should wait at the point
			if (shouldWaitAtPoint (path.getCurrent ())) {
				//checks distance
				if (Vector3.Distance (currentPos, path.getCurrent ().transform.position) <= (path.rangeOfChange + 0.1f)) {
					return false;
				} else {
					return true;
				}
			} else {
				return true;
			}
		} else {
			return true;
		}
	}

}
