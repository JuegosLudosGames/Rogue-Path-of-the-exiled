using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
	//class for the camera

	public float startZoom = 10f;

	//if can move
	public bool canMove = true;

	//place where Camera freezes location
	public Vector3 freezeLocation;

	//the player gameobject
	public GameObject player;

	//max speed of camera
	public float speed;

	//acceptable range to stop moving
	public float minRangeOfPoint;

	//nav agent
	private NavigateAgent nav;

	//rigid body for movement
	private Rigidbody2D controller;

	private void Start() {
		nav = GetComponent<NavigateAgent>();
		controller = GetComponent<Rigidbody2D>();
		GetComponent<Camera>().orthographicSize = startZoom;
	}

	// Update is called once per frame
	void Update () {

		//test if camera can move
		if (!canMove) {
			if (!(nav.Length(transform.position, freezeLocation) <= minRangeOfPoint)) {

				//transform.position = freezeLocation;

				//finds direction of walking
				Vector2 moveDirection = nav.findPathIgnoreWalls(freezeLocation);
				float speedmove = moveDirection.x + moveDirection.y;

				//grabs absolute value of speed
				speedmove = Mathf.Abs(speedmove);

				//checks if speed is too high
				while (Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.y) > 2) {
					moveDirection = new Vector2((moveDirection.x * 0.1f), (moveDirection.y * 0.1f));
				}

				//sets speed
				moveDirection *= speed;

				//sets velocity to move camera
				//controller.velocity = moveDirection;
				transform.position = new Vector3(moveDirection.x + transform.position.x, moveDirection.y + transform.position.y, transform.position.z);

				Debug.Log("distance: " + nav.Length(transform.position, freezeLocation) + " with " + transform.position + " " + freezeLocation);
				Debug.Log("moving at " + moveDirection);

				return;
			} else {
				//controller.velocity = new Vector2(0,0);
				transform.position = freezeLocation;
				Debug.Log("stopped");
			}
			return;
		}

		Debug.Log("going to " + player.transform.position);

		//change offset to match player's offset
		transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, transform.position.z);
	}
}
