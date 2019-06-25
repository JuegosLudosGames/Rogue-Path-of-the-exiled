using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : Trigger, TriggerableI {

	//checks if currently active
	public bool isActive = true;

	//if can be used multiple times
	public bool canBeInteractedMultipleTimes = false;

	//the exclamation object
	public GameObject exclamation;
	private WorldPressable exclamationPress;

	//the collider trigger which triggers the exclamation object
	public Collider2D range;

	//used for intialisation
	void Start() {
		//gets the exclmation script
		exclamationPress = exclamation.GetComponent<WorldPressable> ();

		//sets active state
		exclamationPress.isActive = false;
		exclamationPress.trigger = this;
		exclamation.SetActive (false);
        onStart();
	}

    protected virtual void onStart() {}

	//used when interacted with
	private void Interact () {
		if (!canBeInteractedMultipleTimes) {
			isActive = false;
		}
		onInteract ();
	}

	//inheritable to be called when inherited
	protected virtual void onInteract () {
        trigger();
    }

	//from Collider2D when player enters the trigger area
	void OnTriggerEnter2D () {
		if (isActive) {
			exclamationPress.isActive = true;
			exclamation.SetActive (true);
		}
	}

	//from Collider2D when player exits the trigger area
	void OnTriggerExit2D () {
		exclamationPress.isActive = false;
		exclamation.SetActive (false);
	}

	//inherited from triggerable
	public void onTrigger () {
		Interact ();
	}

}
