using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickupable : MonoBehaviour {

	//variable if item can be pickedup
	public bool canPick = true;
	public bool canPickup {
		get {return canPick;}
		set {canPick = value;}
	}

	//range which item can be picked up
	[Range(1f,3f)]
	public float pickupRange;

	//the player transform component
	Transform player;

	// Use this for initialization
	void Start () {
		//get the player
		player = GameObject.FindGameObjectWithTag ("Player").transform;

		//calls the item onstart function
		onStart ();
	}

	//the methods from monodevlop that prevents overriding them
	protected virtual void onStart() {}
	protected virtual void onFixedUpdatePre() {}
	protected virtual void onFixedUpdatePost() {}
	
	// Update is called once per frame
	void FixedUpdate () {

		//calls the pre fixed update of the subclass
		onFixedUpdatePre ();

		//checks if item can be picked up
		if (canPickup) {
			//checks if player is in range
			if (isPlayerInRange ()) {
				//picks up item
				pickup ();
				onPickup ();
			}
		}

		//calls the post fixed 
		onFixedUpdatePost ();
	}

	//checks if the player is in range of the item
	private bool isPlayerInRange() {
		if (Vector3.Distance (player.position, transform.position) <= pickupRange) {
			return true;
		}
		return false;
	}

	//removes item on pickup
	private void pickup() {
		Destroy (gameObject);
	}

	//methods called to subclass for pickup
	protected virtual void onPickup() {}

}
