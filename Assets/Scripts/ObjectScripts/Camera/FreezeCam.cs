using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeCam : Triggerable
{

	//the location to set the freelocation of the camera
	public Vector2 FreezeLoc;
	//the camera that is following the player
	private FollowPlayer cam;
	//if currently frozen
	private bool isFrozen = false;

	private void Awake()
	{
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowPlayer>();
		
	}

	public override void onTrigger()
	{
		//checks if currently frozen
		if (isFrozen)
		{
			//turns off freeze
			isFrozen = false;
			cam.canMove = true;
		} else {
			//enables freeze
			cam.freezeLocation = new Vector3(FreezeLoc.x, FreezeLoc.y, -10);
			isFrozen = true;
			cam.canMove = false;
		}

	}
}
