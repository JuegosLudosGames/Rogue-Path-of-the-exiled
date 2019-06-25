using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButtonListener : MonoBehaviour{

	//the buttons avalable to the listener
	public GameObject[] UserUiObjects;

	[Obsolete("Use the new system using isPressed()")]
	//returns the pressed buttons
	public List<ButtonPress> getPressed() {

        //checks if the screen is touched
        if (!(isBeingTouched()))
            return null;

		//array holding all the touches on screen
		Touch[] touches = Input.touches;

		//creates list of touches
		List<ButtonPress> list = new List<ButtonPress> ();

		//if we are not paused
		foreach (GameObject test in UserUiObjects) {

			//verifies that the touch is valid
			if (test == null)
				continue;

			//gets the script of the button
			ButtonPress button = test.GetComponent<ButtonPress> ();

			//checks each instance
			foreach (Touch instance in touches) {
				//checks if instance is pressedd
				if (button.isPressed (instance.position)) {
					//adds to pressed buttons
					list.Add (button);
					break;
				}
			}
		}

		return list;
	}

	private List<ButtonType> pressedButtons = new List<ButtonType>();

	public void addToPressed(ButtonPress button) {
		pressedButtons.Add(button.type);
	}

	private void FixedUpdate()
	{
		pressedButtons.Clear();	
	}

	public List<ButtonType> getButtons() {
		return pressedButtons;
	}

	public bool isPressed(ButtonType type) {
		if (pressedButtons.Contains(type))
			return true;
		return false;		
	}

    public bool isBeingTouched() {
        if (!(Input.touchCount > 0))
            return false;

        if ((Input.GetTouch(0).phase == TouchPhase.Ended) || (Input.GetTouch(0).phase == TouchPhase.Canceled))
            return false;

        if (UserUiObjects == null)
            return false;

        return true;
    }

    public bool wasTapped() {
        if (!(Input.touchCount > 0))
            return false;

        if (Input.GetTouch(0).phase == TouchPhase.Ended)
            return true;
        return false;
    }

}
