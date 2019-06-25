using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Spawner : Triggerable {

	//if the instnace is active
	public bool isActive = true;

	//the object which should be spawned
	public Transform toSpawn;

	//the path the object to follow
	public PathFollowable pathToFollow;

	//the parent folder of the object
	public Transform parentFolder;

	//the limit the spawner can spawn
	[Tooltip("Set limit Spawn to -1 if you do not want to limit the number of clones spawned")]
	public int limitSpawn = 0;

	//time in-between spawn times
	public float timeBetweenSpawns = 1;

    //max Range to spawn
    public float maxSpawnRange = 0f;

	//time left to spawn next object
	private float timeleft = 0f;

	//number spawned since intialisation
	private int Spawned = 0;

    //time to spawn in one trigger
    private int SpawnNumber;
	
	// Update is called once per frame
	void Update () {
		//subtracts time from time left to spawn
		if (timeleft > 0) {
			timeleft -= Time.deltaTime;
		}
	}

	//method called when triggered
	override
	public void onTrigger () {
		//checks if active
		if (isActive) {
			//checks if reached limit
			if (Spawned < limitSpawn || limitSpawn == -1) {
				//checks if cooldown has ended
				if (timeleft <= 0) {
					//spawns enemy
					EnemyMovement.spawn (toSpawn, generateRandomLocationInRange(), Quaternion.identity, parentFolder, pathToFollow);
					timeleft = timeBetweenSpawns;
					Spawned = (Spawned == -1) ? -1 : Spawned + 1;
				}
			}
		}
	}

    Vector3 generateRandomLocationInRange() {
        //gets random generator
        System.Random rnd = new System.Random();

        //creates an int out of float (multiply by 100)
        int max = (int)(maxSpawnRange * 100f);
        int min = max * -1;

        //creates vector
        Vector3 output = new Vector3();



        //sets x direction to random
        output.x = transform.position.x + ((rnd.Next(min, max)) / 100f);
        output.y = transform.position.y + ((rnd.Next(min, max)) / 100f);

        //returns vector
        return output;
    }

}
