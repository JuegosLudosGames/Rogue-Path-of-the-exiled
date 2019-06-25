using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigateAgent : MonoBehaviour {

	//Path Finding
	public Vector2 findPath(Vector3 target) {

		//gets the point where the target is located
		Vector3 point = target;

		//the raycast info
		RaycastHit raycast;

		//grabs the raycast info via linecasting
		if (Physics.Linecast (transform.position, target, out raycast, 8, QueryTriggerInteraction.Ignore)) {

			//check if direct path has blocking points
			if (raycast.transform != null && !(raycast.transform.position.Equals (Vector3.zero)) && raycast.transform.position.Equals (target)) {

				//gets the transform of object
				Transform hitTransform = raycast.transform;

				//if not continue
				point = findAvoidingPoint (hitTransform);
				if (point == Vector3.zero)
					point = transform.position;
			}
		}
			
		return findFloatSlope(point);
	}

	public Vector2 findPathIgnoreWalls(Vector3 target) {
		//Vector2 oldSlope = findFloatSlope(target);
		//float x = oldSlope.x;
		//float y = oldSlope.y;

		//while (x > 1 || y > 1) {
		//	x /= 10;
		//	y /= 10;
		//}

		//calculates x and y movement
		float rise = target.y - transform.position.y;
		float run = target.x - transform.position.x;

		return new Vector2(run, rise);
	}

	//finds reduced slope
	Vector2 findFloatSlope(Vector3 point) {
		//calculates x and y movement
		float rise = point.y - transform.position.y;
		float  run = point.x - transform.position.x;

		//reduces
		float divider = GCD (Mathf.Abs (rise), Mathf.Abs (run));

		rise /= divider;
		run /= divider;

		return new Vector2 (run, rise);
	}

	//finds the greatest common divisor
	private float GCD (float a, float b) {
		return b == 0 ? a : GCD (b, a % b);
	}

	//find path around obstacle
	Vector3 findAvoidingPoint(Transform obstacle) {

		//the point the cast is currently checking
		Vector3 checkpoint = obstacle.position;

		//the last offset that has bee checked
		float LastOffsetCheck = 0;

		//the raycast info of last instance
		RaycastHit raycast;

		//the amount of times raycast failed
		int numberOfFailHits = 0;

		//if the algorithum is still working
		bool isSearching = true;

		while (isSearching) {

			//grabs the raycast
			Physics.Linecast (transform.position, checkpoint, out raycast, 8, QueryTriggerInteraction.Ignore);

			//checks if the raycast is hitting a diffrent obstacle
			if(raycast.transform.gameObject != obstacle.gameObject) {
				//verifies that the path is not backtracking
				if(Length(transform.position, checkpoint) > Length(transform.position, obstacle.position)) {
					isSearching = false;
					break;
				} else {
					if(numberOfFailHits >= 30) {
						checkpoint = obstacle.position;
						break;
					} else {
						numberOfFailHits++;
					}
				}
			}

			//checks which side has been checked, negitive or positive
			if (LastOffsetCheck < 0) {
				LastOffsetCheck = LastOffsetCheck * -1;
				LastOffsetCheck++;
				continue;
			} else {
				LastOffsetCheck *= -1;
				continue;
			}

		}

		return checkpoint;
	}

	//grabs the distance of both points
	public float Length (Vector3 point1, Vector3 point2) {
		return (Mathf.Sqrt ((Mathf.Pow(point1.x - point2.x, 2)) + (Mathf.Pow(point1.y - point2.y, 2))));
	}

}
