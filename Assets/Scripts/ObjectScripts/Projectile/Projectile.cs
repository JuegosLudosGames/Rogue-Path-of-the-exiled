using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	private Rigidbody2D rd;

    private enum direction {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    private direction frontDirection = direction.RIGHT;

    public bool left = false;
    public bool right = false;
    public bool up = false;
    public bool down = false;

    [Range(0f, 1f)]
    public float damage = 0.05f;

    public ControllerPlayer player;

    public GameObject Sender;

    // Use this for initialization
    void Start () {
		rd = GetComponent<Rigidbody2D> ();
        //finds direction
        if (left)
            frontDirection = direction.LEFT;
        else if (right)
            frontDirection = direction.RIGHT;
        else if (up)
            frontDirection = direction.UP;
        else if (down)
            frontDirection = direction.DOWN;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<ControllerPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, getAngleHeading (rd.velocity)));

        //checks if below threshold
        if (isBelow()) {
            Destroy();
            return;
        }

        //checks if hitting player
        if (isHitPlayer()) {
            player.damage(damage);
            Destroy();
            return;
        }

        EnemyMovement enemy;

        //checks if hitting enemy
        if (isHitEnemy(out enemy)) {
            enemy.damage(damage);
            Destroy();
            return;
        }

	}

	protected float getAngleHeading(Vector2 velocityComponents) {
		//gets angle
		//'theta' = arcTan(Vy / Vx) * 100
		float angle = (Mathf.Atan(velocityComponents.y / velocityComponents.x) * 180 / Mathf.PI);

        //checks if value is not valid (going directly up or down)
        if (System.Double.IsNaN(angle))
        {
            if (velocityComponents.y > 0)
                angle = 90;
            else
                angle = -90;
        }

        //checks if heading right
        if (velocityComponents.x > 0) {
            angle += 180;
        }

        //changes depending in side of front
        switch (frontDirection) {
            case direction.RIGHT:
                //rotate 180 degrees
                angle += 180f;
                break;
            case direction.UP:
                //rotate 90 degrees
                angle += 90;
                break;
            case direction.DOWN:
                //rotate -90 degees
                angle += -90;
                break;
            case direction.LEFT:
                break;
        }

        return angle;
        
	}

    //checks if in player
    bool isHitPlayer() {
        return (player.hitBox.OverlapPoint(transform.position) && player.gameObject != Sender);      
    }

    //checks if in enemy
    bool isHitEnemy(out EnemyMovement enemy)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //sets the closest placeholder to null
        GameObject tMin = null;
        float minDis = Mathf.Infinity;

        if (enemies == null || enemies.Length == 0) {
            enemy = null;
            return false;
        }
            

        //gets the current position
        Vector2 current = transform.position;

        //goes through each instace
        foreach (GameObject instance in enemies)
        {
            //gets the distaance of the current to the instace
            float distance = Vector2.Distance(instance.transform.position, current);

            //tests if closer than previous
            if (distance < minDis)
            {
                minDis = distance;
                tMin = instance;
            }
        }

        if (tMin == null)
        {
            enemy = null;
            return false;
        }

        enemy = tMin.GetComponent<EnemyMovement>();

        //return overlap
        return (enemy.hitbox.OverlapPoint(transform.position) && tMin != Sender);

    }

    //checks if too low
    bool isBelow() {
        return (transform.position.y < -100);
    }

    void Destroy()
    {
        GameObject.Destroy(gameObject);
    }

}
