using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPressable : MonoBehaviour {

	//the camera of the scene
	Camera cam;

	//the range in which the interactable could be touched
	public Collider2D range;

	//the trigger which this triggers (allows both the interface and abstract class) must be set by the triggerable
	public TriggerableI trigger;

	//if the interactable is active
	private bool Active = true;
	public bool isActive {
		get {return Active;}
		set {Active = value;}
	}

	// Use this for initialization
	void Start () {
		cam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {

		//checks if the state is active
		if (!Active)
			return;

		//checks through every touch
		foreach (Touch touch in Input.touches) {
			
			//gets location on screen
			Vector3 touchLoc = cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cam.nearClipPlane));

			//checks if overlapping
			if (range.OverlapPoint (new Vector2 (touchLoc.x, touchLoc.y))) {
				trigger.onTrigger ();
				return;
			}
		}
	}

}
