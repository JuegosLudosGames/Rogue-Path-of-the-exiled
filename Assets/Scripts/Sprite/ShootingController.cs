using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour {

	public Transform projectile;
	public GameObject parentFolder;

	// Use this for initialization
	void Start () {
		Physics.gravity = new Vector3 (0.0f, -9.8f);
	}

	//shoots the projectile
	public void shoot(Vector3 origin, Vector3 target, Vector2 slope, bool isLeft, GameObject sender, float overShoot) {
		
		//gets the tragectory angle
		float angle = getAngleFromSlope(slope);

		//gets trajectory distance
		float distance = length(origin.x, target.x);

		//calculates the velocity
		//Sqrt ( (dg) / (sin (2'theta')) )
		float velocity = Mathf.Sqrt ( ((distance * Mathf.Abs (Physics.gravity.y)) / Mathf.Sin(2 * angle)) );

		if (System.Double.IsNaN (velocity))
			velocity = 3f;

        velocity += overShoot;

		//drawTrajectory (origin, velocity, slope, isLeft, GetComponent<Shooter> ().line);

		//spawn the projectile
		InstantiateObjectWithVelocity (velocity, angle, origin, isLeft, sender);
	}

	//shoots a projectile
	public void shoot(Vector3 origin, float velocity, Vector2 slope, bool isLeft, GameObject sender) {

		//gets the tragectory angle
		float angle = getAngleFromSlope(slope);

		//spawn the projectile
		InstantiateObjectWithVelocity (velocity, angle, origin, isLeft, sender);
	}

	//returns an angle from a given slope
	float getAngleFromSlope(Vector2 slope) {
		return Mathf.Abs(Mathf.Atan (slope.y / slope.x) * 180 / Mathf.PI);
	}

	//gets the velocity horizontal and vertical components
	Vector2 getVelocityComponents(float velocity, float angle, bool isLeft) {
		//the float values for each
		float horizontal = 0f;
		float vertical = 0f;

        angle = angle * Mathf.PI;
        angle = angle / 180;

		//calculate horizontal
		//Vx = velocity[Total] * cos(angle)
		horizontal = velocity * Mathf.Cos(angle);

		//calculate vertical
		//Vy = velocity[Total] * sin (angle)
		vertical = velocity * Mathf.Sin(angle);

		//if going left, set horizontal to negitive value
		if (isLeft)
			horizontal *= -1;

		//returns value
		return new Vector2(horizontal, vertical);
	}

	//spawns gameobjet
	void InstantiateObjectWithVelocity(float velocity, float angle, Vector3 position, bool isLeft, GameObject sender) {

        Transform Spawned;

        //creates clone
        if (parentFolder != null)
            Spawned = GameObject.Instantiate(projectile, position, Quaternion.identity, parentFolder.transform);
        else
            Spawned = GameObject.Instantiate(projectile, position, Quaternion.identity);

        //sets velocity
        Spawned.gameObject.GetComponent<Rigidbody2D> ().velocity = getVelocityComponents(velocity, angle, isLeft);
        Spawned.gameObject.GetComponent<Projectile>().Sender = sender;
	}

	//draws trajectory parabola
	public void drawTrajectory(Vector3 origin, float velocity, Vector2 slope, bool isLeft, LineRenderer line) {

		//gets the velocity
		Vector2 vel = getVelocityComponents (velocity, getAngleFromSlope (slope), isLeft);

		//sets position to current
		Vector2 pos = new Vector2(origin.x, origin.y);

		//gets gravity
		Vector2 grav = new Vector2(Physics.gravity.x, Physics.gravity.y);

		//gets number of vertices
		int vertices = line.positionCount;

		line.SetPositions(new Vector3[line.positionCount]);

		//goes through each vertex
		for(int x = 0; x < vertices; x++) {
			//draws line, updates to next position
			line.SetPosition (x, new Vector3 (pos.x, pos.y));
			vel = vel + grav * Time.fixedDeltaTime;
			pos = pos + vel * Time.fixedDeltaTime;
		}
	}

	float length(float a, float b) {
		return Mathf.Abs (a - b);
	}

}
