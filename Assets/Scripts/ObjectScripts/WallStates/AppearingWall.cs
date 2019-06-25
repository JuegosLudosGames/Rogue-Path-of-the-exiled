using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearingWall : Triggerable {

	//physics collider of the wall
	private Collider2D wall;

	public override void onTrigger()
	{
		appear();
	}

	// Use this for initialization
	void Awake () {
		wall = GetComponent<Collider2D>();
		wall.isTrigger = true;
	}

	//causes wall to appear
	private void appear() {
		wall.isTrigger = false;
	}

}
