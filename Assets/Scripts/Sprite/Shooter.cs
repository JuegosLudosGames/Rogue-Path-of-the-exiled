using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

	public bool trigger = false;

	public Transform target;

	[Range(0f,100f)]
	public float velocity;

	private Vector2 slope;

	[Range(0f,10)]
	public float x;
	[Range(0f,10f)]
	public float y;

	private ShootingController con;
	public LineRenderer line;

    public float overShoot = 0f;

	// Use this for initialization
	void Start () {
		con = GetComponent<ShootingController> ();
		line = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		slope = new Vector2 (x, y);
		if (trigger) {
			con.shoot (transform.position, target.position, slope, isTargetLeft(), gameObject, overShoot);
			trigger = false;
		}
		//con.drawTrajectory (transform.position, velocity, slope, false, line);
	}

	bool isTargetLeft() {
		if (target.transform.position.x < transform.position.x)
			return true;
		return false;
	}

}
