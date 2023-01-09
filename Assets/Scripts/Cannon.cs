using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts;


public class Cannon : MonoBehaviour 
{

	static public int livesLeft;
	static public bool firstShotFired = false;
	
	public GameObject shot;
	private LevelManager levelManager;
	public GameObject cannonExplosion;
	public AudioClip explosionSound;
    public GameObject[] VideoPanels;
    private GameObject HoldPanel;
    private int index = 0;
    private BleInput sensordata;
    private SliderController value1;
    public float intensity;
    private float baseline1;
    private int indicator;

    void Start()
    {
        levelManager = GameObject.FindObjectOfType<LevelManager>();
        StartCoroutine(Order());
    }


    IEnumerator Order()
	{
        indicator = 0;

        VideoPanels = GameObject.FindGameObjectsWithTag("Panels");

        HoldPanel = GameObject.Find("Panel1");

        HoldPanel.gameObject.SetActive(false);

        UIManager.Instance.SetStatus("Loading");
        UIManager.Instance.SetSubstatus("Sensitivity Adjustment");

        yield return new WaitForSeconds(10f);

        UIManager.Instance.SetStatus("Calibrating");
        UIManager.Instance.SetSubstatus("Please stay still");

        yield return new WaitForSeconds(3f);

        baseline1 = sensordata.sensorArray[0];

        yield return new WaitForSeconds(3f);

        HoldPanel.gameObject.SetActive(true);

        UIManager.Instance.SetStatus(string.Empty);

        UIManager.Instance.SetSubstatus("Seated Static Hold");

        indicator = 1;

        yield return new WaitForSeconds(10f);

        HoldPanel.gameObject.SetActive(false);

        UIManager.Instance.SetSubstatus(string.Empty);

        UIManager.Instance.SetStatus("Ready");

        yield return new WaitForSeconds(2f);

        UIManager.Instance.SetStatus("3");

        yield return new WaitForSeconds(1f);

        UIManager.Instance.SetStatus("2");

        yield return new WaitForSeconds(1f);

        UIManager.Instance.SetStatus("1");

        yield return new WaitForSeconds(1f);

        UIManager.Instance.SetStatus(string.Empty);
    }


    void Update () {
        sensordata = GameObject.FindGameObjectWithTag("SensorData").GetComponent<BleInput>();
        value1 = GameObject.FindGameObjectWithTag("Slider").GetComponent<SliderController>();
        intensity = ((value1.basevalue) / 100) + 1;
        if (sensordata.sensorArray[0] != null && sensordata.sensorArray[0] > baseline1*intensity && indicator == 1)
        //if (Input.GetMouseButtonDown(0))
            {
			//shoot once when the user clicks
			ShootCannon();

        }
    }


    void ShootCannon()
    {
        CreateShot(shot, gameObject.transform.position);
        //GetComponent<AudioSource>().Play();
    }


    void CreateShot(GameObject s, Vector3 position){
		//spawns a shot at the specified position with upward velocity
		//places the shot initially right above the cannon without touching it
		Vector3 newShotPosition = new Vector3(position.x, position.y+2.01f, 0);
		GameObject newShot = Instantiate(s, newShotPosition, Quaternion.identity) as GameObject;
		newShot.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 20f);
		newShot.transform.parent = transform; //makes the player's shots into children of the cannon
	}
	
	
	void OnTriggerEnter2D(Collider2D collider){
        HandleFatalHits();
	}
	
	void HandleFatalHits(){
		//spawn explosion sound and effect at location of cannon before it is destroyed
		Instantiate(cannonExplosion, new Vector2(transform.position.x, transform.position.y+2f), Quaternion.identity);
		AudioSource.PlayClipAtPoint(explosionSound, transform.position);
		Destroy(gameObject); 
	}
	
}
