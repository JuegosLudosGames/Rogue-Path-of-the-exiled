using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour {

	//if should be waiting at this point
	public bool waitAtPoint = false;

	//how long the enemy waits at the point
	public float timeOfWait = 0f;

	//checks if in range of the point
	public bool isInRange (Vector3 point, float range) {
		if (Vector3.Distance (transform.position, point) <= range)
			return true;
		return false;
	}

}
