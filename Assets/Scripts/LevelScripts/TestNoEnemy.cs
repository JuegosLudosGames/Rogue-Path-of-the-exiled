using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNoEnemy : Trigger, TriggerableI {

	//if active on startup
	public bool startOnAwake = false;
	//current active state
	private bool working = false;

	//trigger on start
	public Triggerable[] TriggerOnStart;

	//changes state
	public void onTrigger()
	{
		if (working)
			working = false;
		else
			working = true;

		//does triggers before start
		foreach (Triggerable obj in TriggerOnStart) {
			obj.onTrigger();
		}
	}

	//calls on every frame
	void Update() {
		//checks if working
		if (working) {
			//grabs enemies into array
			GameObject[] objs = GameObject.FindGameObjectsWithTag("Enemy");

			//tests if objects found
			if (objs == null || objs.Length == 0) {
				trigger();
				working = false;
			}
		}
	}

	//call of initialisation
	void Awake() {
		if (startOnAwake)
			working = true;
		
	}
}
