using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class Ship : MonoBehaviour {
	
	//defined in the inspector
	public GameObject shot;
	public GameObject explosion;
    public GameObject shipFire;
	public AudioClip explosionSound;
	
	//settings for each individual ship
	private LevelManager levelManager;
	private int hitsLeft;
	private float height; //defined differently for each row in Start()
	private float speed; //the speed for each ship is random with each start()
	private bool movingRight; //the direction for each ship is random with each start()
	private float leftBoundary; //the horizontal boundaries for each ship are randomly chosen in start()
	private float rightBoundary;
	private bool isDamaged; //initially defined to false for ships that can take multiple hits; defined as true once they take their first hit
	private GameObject fire; //defined in createShipFire() for multi-hit ships that are damaged
	
	
	void Start () {
		//defining general properties for all ships
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		isDamaged = false;
		height = transform.position.y;
		speed = 0.2f;
		if(Random.Range(0f,1f)>0.5f) movingRight = true;
        leftBoundary = 1f;
        rightBoundary = 15f;

    }
	
	void Update () {
		HandleMovement();
		//if the ship is damaged (implying that it is a multi-hit ship currently on fire) keep the fire's location equal to the damaged ship's location
		if(isDamaged){
			fire.transform.position = transform.position;
		}
		
	}
	
	void HandleMovement(){
		//the movement of each ship is handled in this method
		Vector2 shipPosition = gameObject.transform.position;
		
		//handle changes in direction once horizontal boundaries are reached
		if(shipPosition.x >= rightBoundary){
			//the right boundary has been crossed
			movingRight = false;
            Destroy(gameObject);
        }
		if(shipPosition.x <= leftBoundary){
			//the left boundary has been crossed
			movingRight = true;
			//the speed of each ship increases with each change in direction; every ship gets faster over time
			//speed+=0.02f;
            rightBoundary = 15f;
        }
		
		//move the ship left or right depending on its current direction of movement
		if(movingRight){
			gameObject.transform.position = new Vector2(shipPosition.x + 0.1f*speed, height);
		}

	}
	
	
	void OnTriggerEnter2D(Collider2D collider){
		if(collider.tag == "ship"){
			//this trigger is two ships in the same row moving past eachother, so change direction of both ships
			return;
		}
		else{ //this was a trigger from a player shot
			if(hitsLeft > 1){
				HandleNonFatalHits();
			}
			else{
				HandleFatalHits();
			}
		}
	}
	
	//called when a hit to a ship would destroy it
	void HandleFatalHits(){
		//if this is a multi-hit ship taking its last hit, destroy the fire before destroying the ship.
		if(isDamaged){
			Destroy(fire);
		}
		
		//spawn an explosion and explosion sound at the location of the ship before it is destroyed
		AudioSource.PlayClipAtPoint(explosionSound, transform.position);
		Instantiate(explosion, transform.position, Quaternion.identity);
		Destroy(gameObject);
        UIManager.Instance.IncreaseScore(10);
    }
	
	//called when a hit to a ship would only damage it and not destroy it
	void HandleNonFatalHits(){
		if(!isDamaged){
			//if this is the multi-hit ship's first hit, spawn the fire
			createShipFire();
			isDamaged = true;
		}
		else{
			//if this is not the multi-hit ship's first hit, make the fire bigger
			//fire.particleSystem.emissionRate += 10f;
			//fire.GetComponent<ParticleSystem>().duration += 0.1f;
		}
	}
	
	//creates a shipFire at the current location of the damaged ship. The shipFire's location will be continuously kept equal to this ship's location in the ship's Update()
	void createShipFire(){
		fire = Instantiate(shipFire, transform.position, Quaternion.identity) as GameObject;
	}
	
}
