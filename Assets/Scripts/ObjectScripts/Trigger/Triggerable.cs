using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Triggerable : MonoBehaviour, TriggerableI {
	//inheritable method to call to be triggered
	public abstract void onTrigger ();
}
