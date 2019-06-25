using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trigger : MonoBehaviour {

	//if the trigger is active
	protected bool isWorking = true;

	//all the objects which this trigger, triggers
	public Triggerable[] triggerObjects;

	//method to be called when trigger is triggered
	protected void trigger() {
		//checks if trigger is working
		if (isWorking) {
			Debug.Log(gameObject.name + " is triggering");
			//verify's if the trigger array is empty
			if (triggerObjects == null)
				return;
			//triggers each instance if triggerable
			foreach (Triggerable instance in triggerObjects) {
				if (instance == null)
					continue;
				instance.onTrigger ();
			}
		}
	}

}
