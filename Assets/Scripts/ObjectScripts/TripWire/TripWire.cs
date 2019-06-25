using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripWire : Trigger {

	//if should only activate once
	public bool activeSingle = false;
	//if been set active before
	private bool hasActive = false;

	//if should clear enemies
	public bool clearEnemies = false;

	//sets active state in Trigger
	public bool isActive {
		get {return isWorking;}
		set {isWorking = value;}
	}

	//inherited from Collider2D and activates trigger
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag.Equals("Player")) {
			if (other.isTrigger == false)
			{
				if (activeSingle)
				{
					if (!hasActive)
					{
						trigger();
						hasActive = true;
						if (clearEnemies)
						{
							EnemyMovement.killAll();
						}
					}
				}
				else
				{
					trigger();
					if (clearEnemies)
					{
						EnemyMovement.killAll();
					}
				}
			}
		}
	}

}
