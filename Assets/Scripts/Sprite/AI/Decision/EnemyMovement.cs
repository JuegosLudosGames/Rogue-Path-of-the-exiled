using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {

	//if enemy is active or not
	public bool isActive;

	//if enemy can move
	public bool canMove = true;

    //if enemy can shoot
    public bool canShoot = true;

    //if enemy is stunned
    private bool isSlowed = false;

    //time left for slow
    private float timeSlowedLeft = 0f;

    //decimal percentage of slow amount
    [Range(0f, 1f)]
    public float slowPerCent = 0f;

	//speed in which enemy moves
	public float speed;

	//enemy health
	[Range(0f, 1f)]
	public float StartingHealth = 1f;
	public float health = 1f;

	//enemy attack damage
	[Range(0f, 1f)]
	public float heavyAttackDamage = 0.25f;
	[Range(0f, 1f)]
	public float lightAttackDamage = 0.1f;
    [Range(0f, 1f)]
    public float rangeAttackDamage = 0.1f;
    public float heavyStunTime = 1.5f;

    //distance which the enemy can see
    public float SightDistance = 10f;

	//time needed for the enemy to "forget" about the player
	public float forgetTime = 15f;

	//time since enemy last seen the player
	private float timeSinceLastSeen = 0f;

	//ranges for attacks
	public CapsuleCollider2D lightAttack;
	public CapsuleCollider2D heavyAttack;

	//range for being attacked
	public BoxCollider2D hitbox;

	//damage animation overlay
	public GameObject overlay;

	//damage overlay timer
	private float timeToStopDamage = 0f;

	//the player target
	Transform target;

	//navigation agent for PathFinding for this instance
	NavigateAgent nav;

	//the path follow manager for this instance
	PathFollower follow;

	//the animation controller for this instance
	Animator animator;

	//the rigidbody and physics controller for this instance
	Rigidbody2D controller;

	//the state the Ai is in
	public AiState state = AiState.FOLLOW_PLAYER;

	//if enemy is dead
	private bool onDeath;
	//time since enemy has died
	private float timeSinceDeath;

	//attack cooldown of player
	private float cooldown;

	//the gameobject the instance is running from
	private GameObject RunFrom;

	//if instance is running away after attack
	private bool isRunningFromAttack = false;

	//if the enemy was recently damaged for overlay
	private bool wasDamaged = false;

	//the hitbox of the player
	private Collider2D playerHitBox;

	//if the instance "remembers" that it has seen the players
	private bool seenPlayer = false;

    //shooting controller
    private ShootingController shootCon;

    //shooting angle
    public Vector2 rangeAngle;

    //shoot cooldown
    private float timeToNextShot = 0f;
    public float rangeCoolDown = 5f;

    //shooting adjust
    public float overShoot = 0f;

	//sound
	AttackSoundEffect ASound;

	// Use this for initialization
	void Awake () {
		
		//closes overlay
		overlay.SetActive (false);

		//sets target to player
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		playerHitBox = GameObject.FindGameObjectWithTag ("Player").GetComponent<ControllerPlayer> ().hitBox;

		//sets instance nav agent
		nav = GetComponent<NavigateAgent> ();

		//gets the path follower
		follow = GetComponent<PathFollower> ();

		//gets animator of instance
		animator = GetComponent <Animator> ();

		//gets rigidbody
		controller = GetComponent<Rigidbody2D> ();

        //gets shooting controller
        shootCon = GetComponent<ShootingController>();

		//sets health and death state
		health = StartingHealth;
		onDeath = false;

        if (rangeAngle == Vector2.zero)
            rangeAngle = new Vector2(1f, 1f);

		//gets sound
		ASound = GetComponent<AttackSoundEffect>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		//tests if the enemy is in the middle of dying
		if(onDeath) {
            //tests if animations is finished
            timeSinceDeath -= Time.deltaTime;
			if(timeSinceDeath <= 0) {
				Despawn ();
			}
            return;
		}

		//tests of time to next attack is being used
		if(!(cooldown <= 0)) {
			cooldown -= Time.deltaTime;
			//tests if enough time went by
			if(cooldown <= 0) {
				canMove = true;
			}
		}

        //defualt slow is off
        isSlowed = false;

        //tests if should be slowed
        if (timeSlowedLeft > 0)
        {
            timeSlowedLeft -= Time.deltaTime;
            if (timeSlowedLeft <= 0)
            {
                isSlowed = false;
            }
            else
            {
                isSlowed = true;
            }
        }

        //tests if overlay has been on
        if (timeToStopDamage > 0) {
			timeToStopDamage -= Time.deltaTime;
			//tests if enough time has passed
			if (timeToStopDamage <= 0) {
				overlay.SetActive (false);
			}
		}

        if (timeToNextShot > 0)
            timeToNextShot -= Time.deltaTime;

		//deciding state
		state = decideState ();

		//resets speed and movement direction
		float speedmove = 0f;
		Vector2 moveDirection = Vector2.zero;

		//tests if enemy can move
		if (canMove) {

			//move away from player decision
			if(state == AiState.DISTANCE_PLAYER) {
				//gets line to player
				getReversedTargetDirection (target.position, out moveDirection, out speedmove);
			}

			//move from ally decision
			if(state == AiState.DISTANCE_ALLY) {
				getReversedTargetDirection (RunFrom.transform.position, out moveDirection, out speedmove);
			}

			//light attack decision
			if (state == AiState.ATTACK_LIGHT) {
				attack (lightAttack, lightAttackDamage, 1, false);
				animator.SetTrigger ("lightattack");
			}

			//heavy attack decision
			if (state == AiState.ATTACK_HEAVY) {
				attack (heavyAttack, heavyAttackDamage, 2, true);
				animator.SetTrigger ("heavyattack");
				canMove = false;
			}

            if (state == AiState.ATTACK_SHOOT) {

                float overUnder = 0f;

                //checks if should under shoot or over shoot
                //if player heading to enemy, undershoot, away, overshoot respectively
                if (isPlayerLookingAway())
                    overUnder = overShoot;
                //shoots player
                shootCon.shoot(transform.position, target.position, rangeAngle, target.gameObject.GetComponent<ControllerPlayer>().isLeftTo(point: transform.position), gameObject, overUnder);
            }

			//move to decision
			if (state == AiState.FOLLOW_PLAYER) {
				getTargetDirection (target.position, out moveDirection, out speedmove);
			}

			//follow decision
			if (state == AiState.FOLLOW_PATH) {
				//tests if has path to follow
				if (follow.hasPathToFollow ()) {
					//checks if should move on path
					if (follow.shouldMove (transform.position))
						getTargetDirection (follow.getCurrentPathPoint (transform.position), out moveDirection, out speedmove);
					else {
						moveDirection = Vector2.zero;
						speedmove = 0;
					}
				} else {
					getTargetDirection (target.position, out moveDirection, out speedmove);
				}
			}

		}

        //after decision

        

        //checks for facing right and changes to move left
        if (transform.localScale.x > 0) {
			if (speedmove < 0) {
				//sets to look left
				Vector3 scale = gameObject.transform.localScale;
				scale.Set (gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
				gameObject.transform.localScale = scale;
			}
		}

		//checks for facing left and changes to move right
		if (transform.localScale.x < 0) {
			if (speedmove > 0){
				//sets to move right
				Vector3 scale = gameObject.transform.localScale;
				scale.Set (gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
				gameObject.transform.localScale = scale;
			}
		}

		//gets the absolute value of speedmove
		speedmove = Mathf.Abs(speedmove);

		//sets animator parameter for speed
		animator.SetFloat ("speed", speedmove);

		//checks if speed is too high
		while(Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.y) > 2) {
			moveDirection = new Vector2((moveDirection.x *0.1f), (moveDirection.y * 0.1f));
		}

		//sets speed
		moveDirection *= speed;

        //if slowed reduces the speed
        if (isSlowed)
        {
            moveDirection -= (moveDirection * slowPerCent);
        }

		//sets velocity to move player
		controller.velocity = moveDirection;

	}

	//gets the direction the target is relative to the instance and the best speed
	void getTargetDirection (Vector3 target, out Vector2 directionOut, out float speedOut) {
		//gets line to player
		Vector2 slope = nav.findPath (target);

		//set variable for speed needed to move
		directionOut = new Vector2 (slope.x, slope.y);

		//gets general speed in float
		speedOut = slope.x + slope.y;
	}

	//gets the opposite direction  the target is relative to the instance and the best speed
	void getReversedTargetDirection (Vector3 target, out Vector2 directionOut, out float speedOut) {
		//gets line to player
		Vector2 slope = nav.findPath (target);

		//set variable for speed needed to move
		directionOut = new Vector2 (-slope.x, -slope.y);

		//gets general speed in float
		speedOut = slope.x + slope.y;
	}

	//damages the instace
	public void damage (float taken) {
		health -= taken;
		ASound.playDamageSound();
		if (health <= 0f) 
			death ();
		wasDamaged = true;
		timeToStopDamage = 0.5f;
		overlay.SetActive (true);
        seenPlayer = true;
	}

    //death action
    void death() {

        //adds to death count to active conditionals
        GameObject[] conditionals = GameObject.FindGameObjectsWithTag("DeathConditional");
        if (conditionals != null || conditionals.Length > 0)
        {
            foreach (GameObject con in conditionals)
            {
                con.GetComponent<EnemyDeathConditional>().addKill(1);
            }
        }

        //death function
		onDeath = true;
		canMove = false;
		timeSinceDeath = 3f;
		animator.SetBool("death", true);
        //disable hitbox
        GetComponent<BoxCollider2D>().isTrigger = true;
        
	}

	//despawns the gameobject
	private void Despawn () {
		Destroy (gameObject);
	}

	//has the instance attack
	private void attack(Collider2D collider, float dam, float cooldown, bool shouldStun) {

		//tests if the player is in attack range
		if (isPlayerInRange (collider)) {
			playerHitBox.gameObject.GetComponent<ControllerPlayer> ().damage (dam);
            //tests if shouls be stunned
            if (shouldStun) {
                playerHitBox.gameObject.GetComponent<ControllerPlayer>().stun(heavyStunTime);
            }
		}

		//sets the cooldown
		this.cooldown = cooldown;	
		isRunningFromAttack = true;

		//plays sound
		ASound.playAttackSound();
	}

    public void stun(float time)
    {
        isSlowed = true;
        timeSlowedLeft = time;
    }

    //tests if the player is in range for attack
    private bool isPlayerInRange(Collider2D collider) {
		return collider.IsTouching (playerHitBox);
	} 

	//sets the state the Ai will run
	private AiState decideState() {
		//tests if can move
		if (!isActive || !canMove)
			return AiState.NOTHING;
        if(seenPlayer)
            timeSinceLastSeen += Time.deltaTime;

        //gets all allies in scene
		GameObject[] allies = GameObject.FindGameObjectsWithTag ("Enemy");

		//checks if any found
		if (allies != null) {
			
			//gets closest ally
			GameObject cAlly = getClosestAlly (allies);

			//checks if one was found
			if (cAlly == null) {
				//decides is should move away from ally that is too close
				if (Vector2.Distance (transform.position, cAlly.transform.position) < 1.5f) {
					RunFrom = cAlly;
					return AiState.DISTANCE_ALLY;
				}
			}
		}

        //checks if cannot see player
        if (!(canSeePlayer())) {
			//checks if had seen player
			if (seenPlayer) {
				//checks if time seen player is beyond its "forgetTime"
				if (timeSinceLastSeen >= forgetTime) {
					seenPlayer = false;
					timeSinceLastSeen = 0f;
					return AiState.FOLLOW_PATH;
				} else {
					//if ai remembers the player and moves that way
					//return AiState.FOLLOW_PLAYER;
				}

			} else {
				//if player is not remembered and ai oes not see the player
				return AiState.FOLLOW_PATH;
			}
		}

		//sets that ai has seen the player
		seenPlayer = true;

		//gets distance from player
		float distance = hitbox.Distance (playerHitBox).distance;

		//decides if enemy should distance away from player due to distance
		if (distance < 0.5f)
			return AiState.DISTANCE_PLAYER;

		//decides if should distance away from player after attacking
		if (isRunningFromAttack) {
			//checks if distance is good enough
			if (!(distance >= 4f))
				return AiState.DISTANCE_PLAYER;
			else
				isRunningFromAttack = false;
		}

		//decides if should distance from player due to damage
		if(wasDamaged) {
			wasDamaged = false;
			isRunningFromAttack = true;
			return AiState.DISTANCE_PLAYER;
		}


		//decides if should to attack
		if(distance > 0.5f && distance < 12f) {
			
			//decides to light or heavy attack
			//checks if able to light attack
			if (distance < 1f)
				return AiState.ATTACK_LIGHT;
			
			//checks if player is looking away for heavy attack
			if (isPlayerLookingAway () && (distance < 1.5f))
				return AiState.ATTACK_HEAVY;

            if (canShoot)
            {
                //checks if affected by cooldown
                if (timeToNextShot <= 0)
                {
                    timeToNextShot = rangeCoolDown;
                    return AiState.ATTACK_SHOOT;
                }
            }
		}

		//by default, moves torwards the player
		return AiState.FOLLOW_PLAYER;

	}

	//tests if the instance can see the player
	private bool canSeePlayer() {
		//checks if looking at player
		if (isLookingInDirectionOfPlayer ()) {
			//checks if player is obstructed
			if (isPlayerObstructed ()) {
				return false;
			} else {
				//tests if the player is within the sight range
				if (isPlayerCloseToSee ()) {
					return true;
				} else {
					return false;
				}
			}
		} else {
			return false;
		}
	}

	//checks if the player is looking away from the instance
	private bool isPlayerLookingAway() {
		//checks what side of player enemy is on
		if (transform.position.x < playerHitBox.transform.position.x) {
			//if on left side
			return !(playerHitBox.gameObject.GetComponent<ControllerPlayer> ().isLookingLeft ());
		} else {
			return (playerHitBox.gameObject.GetComponent<ControllerPlayer> ().isLookingLeft ());
		}
	}

	//checks if the instance's view of the player is obstructed by a wall
	private bool isPlayerObstructed() {
		//checks a raycast if blocked
		if (Physics2D.Raycast (transform.position, 
				target.position, 
				Vector3.Distance (transform.position, target.position),
				LayerMask.GetMask ("Blocking"))) 
		{
			return true;
		}
		return false;
	}

	//checks if player is close enough to see
	private bool isPlayerCloseToSee() {
		if (Vector3.Distance (transform.position, target.position) >= SightDistance)
			return false;
		return true;
	}

	//gets the ally closest to the instance
	private GameObject getClosestAlly(GameObject[] allies) {

		//sets the closest placeholder to null
		GameObject tMin = null;
		float minDis = Mathf.Infinity;

		//gets the current position
		Vector2 current = transform.position;

		//goes through each instace
		foreach (GameObject instance in allies) {
			//gets the distaance of the current to the instace
			float distance = Vector2.Distance (instance.transform.position, current);

			//tests if closer than previous
			if(distance < minDis) {
				minDis = distance;
				tMin = instance;
			}
		}
		//returns final placeholder
		return tMin;
	}

	//tests if instance is looking left
	public bool isLookingLeft() {
		return (transform.lossyScale.x < 0);
	}

	//tests if the instance is looking in the direction of the player
	public bool isLookingInDirectionOfPlayer () {
		//checks what side of player enemy is on
		if (transform.position.x < playerHitBox.transform.position.x) {
			//if on left side
			return !isLookingLeft();
		} else {
			//if on right side
			return isLookingLeft();
		}
	}

	//static methods

	//spawns an instance of an enemy
	public static void spawn(Transform toSpawn, Vector3 location, Quaternion quad, Transform parent, PathFollowable pathToFollow) {
		Transform spawned = (Instantiate (toSpawn, location, quad, parent) as Transform);
		spawned.gameObject.GetComponent<PathFollower> ().path = pathToFollow;
	}

	public static void killAll() {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

		foreach (GameObject e in enemies) {
			e.GetComponent<EnemyMovement>().damage(1000f);
		}
	}

}