using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTest : Trigger {

	public bool triggered;

	// Update is called once per frame
	void Update () {
		if (triggered) {
			trigger();
			triggered = false;
		}
	}
}
