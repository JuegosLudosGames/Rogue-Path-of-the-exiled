using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerPlayer : MonoBehaviour {

	//enables or disables if the player can move
	public bool canMove = true;

    //if the player is slowed
    private bool isSlowed = false;

    //time has be slowed
    private float timeSlowedLeft = 0f;

	//the speed of the player
	public float speed = 6.0F;

    //the decimal value of the percentage to be slowed
    [Range(0f, 1f)]
    public float slowPerCent = 0f;

	//player health
	[Range(0f, 1f)]
	public float StartingHealth = 1f;
	public float health = 0f;
	private GameObject healthBar;

	//attack damage
	[Range(0f, 1f)]
	public float heavyAttackDamage = 0.25f;
	[Range(0f, 1f)]
	public float lightAttackDamage = 0.1f;
	public float HeavyStunTime = 1.5f;

	//ranged attack ammo
	public JoyStick rangedStick;
	private bool joystickWasHeld = false;

    //ammo counts
    public RangeSelectable PrimaryAmmoCountSel;
    public RangeSelectable SecondAmmoCountSel;

    public bool PrimarySelected;

    //trajectory object
    public GameObject trajectoryObject;

    //line renderer
    private LineRenderer lineRender;

	//the direction the player moves
	private Vector2 moveDirection;

	//the animator of the player
	private Animator animator;

	//the ridgdbody physics controller of the player
	private Rigidbody2D controller;

	//attack ranges
	public CapsuleCollider2D lightAttack;
	public CapsuleCollider2D heavyAttack;

	//attack hitbox
	public BoxCollider2D hitBox;

	//the button input listener
	private ButtonListener buttonListen;
	public JoyStick Stick;

    //shooting controller
    private ShootingController shootCon;

	//attack delay timer
	private float timeToNextAttack = 0;

    //temp variables for shooting
    private float powerPrev = 0f;
    private bool isLeftPrev = false;
    private Vector2 slopePrev = Vector2.zero;

	//sounds
	private AttackSoundEffect ASound;

    //called at intialisation
    void Start() {
		
		//gets the button listener
		buttonListen = (GameObject.FindGameObjectWithTag ("GameController")).GetComponent<ButtonListener> ();

		//sets the movedirection to zero
		moveDirection = Vector2.zero;
        
		//gets the physics controller
		controller = GetComponent<Rigidbody2D>();

		//gets the animator
		animator = GetComponent<Animator> ();

		//sets health and healthbar
		health = StartingHealth;
		healthBar = GameObject.FindGameObjectWithTag ("HealthBar");
		healthBar.GetComponent<Slider> ().value = health;

        //sets ammo and ammo count
        //addAmmoCount(StartingRangeAmmo);
		//RangeAmmoCountBox = GameObject.FindGameObjectWithTag ("AmmoCount");

        //gets objects for shooting
        shootCon = GetComponent<ShootingController>();
        lineRender = trajectoryObject.GetComponent<LineRenderer>();

        //closes line renderer
        lineRender.gameObject.SetActive(false);

        //sets defualt of active
        //if (activeCount == null)
        //    activeCount = PrimaryAmmoCount;
        if (PrimarySelected)
            setActiveAmmoPrimary(true);

		//gets sound object
		ASound = GetComponent<AttackSoundEffect>();
	}

	//updates before physics updates
	void FixedUpdate() {

		//tests of time to next attack is being used
		if(!(timeToNextAttack <= 0)) {
			timeToNextAttack -= Time.deltaTime;
			//tests if enough time went by
			if(timeToNextAttack <= 0) {
				canMove = true;
			}
		}

        //defualt slow is off
        isSlowed = false;

        //tests if should be slowed
        if (timeSlowedLeft > 0) {
            timeSlowedLeft -= Time.deltaTime;
            if (timeSlowedLeft <= 0)
            {
                isSlowed = false;
            }
            else {
                isSlowed = true;
            }
        }

		//variables for speed in each direction
		float vertical = 0.0f;
		float horizontal = 0.0f;

		//tests if player is allowed to move
		if (canMove) {

			//checks if paused pressed
			if (buttonListen.isPressed(ButtonType.PAUSE))
			{
				//pauses game and stops player
				PauseMenuControl.pause();
				return;
			}

			//gets the angle of how the stick was moved
			horizontal = Stick.Horizontal();
			vertical = Stick.Vertical();

			//checks attack buttons
			if (buttonListen.isPressed(ButtonType.ATTACK_LIGHT) && timeToNextAttack <= 0)
			{
				attack(lightAttack, lightAttackDamage, false);
				animator.SetTrigger("lightattack");
				timeToNextAttack = 1;
				ASound.playAttackSound();
			}

			if (buttonListen.isPressed(ButtonType.ATTACK_HEAVY) && timeToNextAttack <= 0)
			{
				attack(heavyAttack, heavyAttackDamage, true);
				animator.SetTrigger("heavyattack");
				timeToNextAttack = 2;
				canMove = false;
				ASound.playAttackSound();
			}

			//Ammo selector checks
			if (buttonListen.isPressed(ButtonType.AMMO_PRIMARY))
			{
				setActiveAmmoPrimary(true);
			}

			if (buttonListen.isPressed(ButtonType.AMMO_SECONDARY))
			{
				setActiveAmmoPrimary(false);
			}

		}
	

         //checks ranged stick
         //if player is aiming
         if (isAiming ()) {
            //slows player movement
            isSlowed = true;

            //calculates trajectory
            calculateTrajectoryStart(out powerPrev, out isLeftPrev, out slopePrev);

            //draws trajectory line
            lineRender.gameObject.SetActive(true);
            shootCon.drawTrajectory(transform.position, powerPrev, slopePrev, isLeftPrev, lineRender);
         }
        //if player has attempted to shoot
        if (shouldShoot())
        {
            //shoots in trajectory using previous settings
            lineRender.gameObject.SetActive(false);

            //checks if ammo is left
            if (getActiveSelect().canConsume(1))
            {
                //shoots
                shootCon.shoot(transform.position, powerPrev, slopePrev, isLeftPrev, gameObject);
                //updates counter
                getActiveSelect().consume(1);
            }
         }

		//set variable for speed(in each direction) needed to move
		moveDirection = new Vector2(horizontal, vertical);

        //if slowed reduces the speed
        if (isSlowed) {
            moveDirection -= (moveDirection * slowPerCent);
        }

        //sets the animation speed variable
		moveDirection *= speed;

		//gets total speed in float
		float speedmove = horizontal + vertical;

		//changes direction sprite is facing

		//checks for facing right and changes to move left (negative value(speed) is facing left)
		if (transform.localScale.x > 0) {
			if (speedmove < 0) {
				//sets facing direction to the left
				Vector3 scale = gameObject.transform.localScale;
				scale.Set (gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
				gameObject.transform.localScale = scale;
			}
		//checks for facing left and changes to move right
		} else if (transform.localScale.x < 0) {
			if (speedmove > 0) {
				//sets facing direction to the right
				Vector3 scale = gameObject.transform.localScale;
				scale.Set (gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
				gameObject.transform.localScale = scale;
			}
		}

		//gets absolute value of speedmove
		speedmove = Mathf.Abs(speedmove);

		//sets animator speed parameter
		animator.SetFloat ("speed", speedmove);

		//sets velocity for movement
		controller.velocity = moveDirection;
	}

	//damages player
	public void damage(float damage) {
		//reduces health and healthbar
		health -= damage;
		healthBar.GetComponent<Slider> ().value = health;

		//plays sound
		ASound.playDamageSound();

		//checks if dead
		if (health <= 0 && canMove)
			death ();
	}

	//death reaction
	void death() {
		//disables player
		canMove = false;

		//set death animation
		animator.SetBool ("death", true);

		//display death menu
        DeathMenuControl.died();
	}

	//attack command
	void attack(Collider2D area, float dam, bool shouldStun) {

		//gets all the enemies in the range
		EnemyMovement[] enemies = getEnemiesInArea (area);

		//damages each enemy
		foreach(EnemyMovement instance in enemies) {
			if (instance == null)
				continue;
			//does damage on enemy
			instance.damage(dam);

            //tests if should stun
            if (shouldStun) {
                instance.stun(HeavyStunTime);
            }
		}
	}

	//returns an array of all enemies in range
	EnemyMovement[] getEnemiesInArea(Collider2D area) {

		//creates layer mask to include only attackables
		LayerMask mask = 1 << 9;

		//creates filter
		//ignores depth, includes mask, includes triggers
		ContactFilter2D filter = new ContactFilter2D ();
		filter.ClearDepth ();
		filter.SetLayerMask (mask);
		filter.useTriggers = true;

		//creates array
		Collider2D[] colliders = new Collider2D[50];

		//puts into array all the overlapping colliders
		Physics2D.OverlapCollider (area, filter, colliders);

		//creates temp list for collider
		List<EnemyMovement> list = new List<EnemyMovement> ();

		//adds colliders into list
		foreach (Collider2D collide in colliders) {
			
			//tests if the collider is null
			if (collide == null)
				continue;
			
			//tests if the collider is a trigger
			if (collide.isTrigger)
				continue;
			
			//tests if the collider is a hitbox by checking if it is a box2d
			if (collide is BoxCollider2D) {
				//adds to list
				list.Add (collide.gameObject.GetComponent<EnemyMovement> ());

			}
		}

		//returns list as array
		return list.ToArray ();
	}

	//tests if the player is looking left (-x direction)
	public bool isLookingLeft() {
		return (transform.lossyScale.x < 0);
	}

    public bool isLeftTo(Vector3 point) {
        return (transform.position.x < point.x);
    }

    public bool isLeftTo(Vector2 point)
    {
        return (transform.position.x < point.x);
    }

    //heals player for x amount of hp
    public void heal(float hp) {
		//adds health
		health += hp;

		//checks for overflow
		if(health > 1)
			health = 1;

		//sets the healthbar
		healthBar.GetComponent<Slider> ().value = health;
	}

    public void stun(float time) {
        isSlowed = true;
        timeSlowedLeft = time;
    }

    public void reviveAni() {
		animator.SetBool("death", false);
        animator.SetTrigger("Revive");
        canMove = true;
    }

	//ranged

    //tests if controller should shoot
	bool shouldShoot() {
		if ((rangedStick.Horizontal () == 0f) && (rangedStick.Vertical () == 0f)) {
			if (joystickWasHeld) {
				joystickWasHeld = false;
				return true;
			}
		}

		return false;
	}

    //checks if joystick is currently being held
    public bool isAiming() {
        if ((rangedStick.Horizontal() != 0f) && (rangedStick.Vertical() != 0f)) {
            joystickWasHeld = true;
            return true;
        }
        return false;
    }

    //calculates the required variables for shooting
    void calculateTrajectoryStart(out float power, out bool isLeft, out Vector2 slope) {
        //location of the stick
        Vector2 stickRange = new Vector2(rangedStick.Horizontal(), rangedStick.Vertical());

        //power calculation
        //checks distance away from center
        //Sqrt ( x^2 + y^2 )
        power = Mathf.Sqrt(Mathf.Pow(stickRange.x, 2) + Mathf.Pow(stickRange.y, 2));

        //does multiplier
        power *= getActiveSelect().powerMultiplier; ;

        //gets direction
        isLeft = false;

        //checks if in 3rd quadrant of stick
        if (stickRange.x < 0f && stickRange.y < 0f)
            isLeft = false;
        else if (stickRange.x < 0f && stickRange.y > 0f)
            isLeft = true;
        else if (stickRange.x > 0f && stickRange.y < 0f)
            isLeft = true;
        else if (stickRange.x > 0f && stickRange.y > 0f)
            isLeft = false;

        //gets slope
        slope = new Vector2(stickRange.x, stickRange.y);
        
    }

    //sets current ammo type
    void setActiveAmmoPrimary(bool status) {

        //sets variable
        //activeCount = status ? PrimaryAmmoCount : SecondAmmoCount;
        PrimarySelected = status;

        //sets state to primary
        PrimaryAmmoCountSel.isSelected = status;

        //sets unactive to secondary
        SecondAmmoCountSel.isSelected = !status;

        //sets projectile in controller
        shootCon.projectile = getActiveSelect().projectile;
    }

    //grabs the active ammo type
    RangeSelectable getActiveSelect() {
        return (PrimarySelected) ? PrimaryAmmoCountSel : SecondAmmoCountSel;
    }

    //returns the count of ammo
    public int getAmmoCount(bool isPrimary) {
        return isPrimary ? PrimaryAmmoCountSel.Count : SecondAmmoCountSel.Count;
    }

    //tests if ammo type is full
    public bool isAmmoCountFull(bool isPrimary) {
        return isPrimary ? PrimaryAmmoCountSel.isFull() : SecondAmmoCountSel.isFull();
    }

    //adds ammo to ammo count
    public void addAmmo(bool isPrimary, int Amount) {
        if (isPrimary)
        {
            PrimaryAmmoCountSel.addAmmo(Amount);
        }
        else {
            SecondAmmoCountSel.addAmmo(Amount);
        }
    }

}