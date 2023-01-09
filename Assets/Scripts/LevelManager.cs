using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts;

public class LevelManager : MonoBehaviour {

	//keeps track of the last stage that the player reached during the current play session. The player is allowed to pick back up at this stage if they lose.
	static public int highestStageReached = 0;
	
	private bool isPaused;
	private bool readyToLoadNewLevel;
	private float pauseTimeLeft;
	private bool won;
	private bool lost;
	
	//matrix where the rows represent the preset number of lives and shots respectively for each stage
	static int[,] levelSettingsA = new int[,] {{4,190}, {3,100}, {3,140}, {2,150}, {2,200}};
	//matrix where the rows represent the preset minimum ship speeds, maximum ship speeds, chances of enemy shots, and enemy shot velocities respectively for each stage
	static float[,] levelSettingsB = new float[,] {{0.75f,1f,0.01f,-6f},{0.85f,1.10f,0.01f,-8f},{1.0f,1.25f,0.0115f,-10f},{1.25f,1.50f,0.0125f,-12f},{1.50f,1.75f,0.015f,-14f},{1.75f,2.0f,0.0175f,-16f}};
	//lists that represent an unordered collection of ship types (1hit,2hit,3hit) that each stage should spawn in ShipGenerator
	static int[] shipList1 = {1,1,1,1,1};

	public void LoadLevel(string levelName){
		RefreshSettings();
		Application.LoadLevel(levelName);
	}
	public void QuitRequest(){
		Debug.Log("Quit requested");
	}
	public void LoadNextLevel(){
		Application.LoadLevel(Application.loadedLevel + 1);
		RefreshSettings();
	}
	
	
	void Start(){
		isPaused = false;
		readyToLoadNewLevel = false;
		won = false;
		lost = false;
	}
	
	void Update(){
        UIManager.Instance.IncreaseTime(0.001f);
	}
	
	void RefreshSettings(){
	//returns variables to their original values while a new level is loaded
		isPaused = false;
		readyToLoadNewLevel = false;
		won = false;
		lost = false;
	}
	
	
	//returns the number of lives allotted for the current level
	static public int getInitialNumberOfLives(){
		//each subarray in levelSettingsA has the lives at index 0
		return levelSettingsA[getCurrentStageIndex(), 0];
	}
	//returns the number of shots allotted for the current level
	static public int getInitialNumberOfShots(){
		//each subarray in levelSettingsA has the shots at index 1
		return levelSettingsA[getCurrentStageIndex(), 1];
	}
	//returns the minimum ship speed allowed for the current level
	static public float getMinShipSpeed(){
		//each subarray in levelSettingsB has the min ship speed at index 0
		return levelSettingsB[getCurrentStageIndex(), 0];
	}
	//returns the maximum ship speed allowed for the current level
	static public float getMaxShipSpeed(){
		//each subarray in levelSettingsB has the max ship speed at index 1
		return levelSettingsB[getCurrentStageIndex(), 1];
	}
	//returns the chance of an enemy shot (for each frame) for the current level
	static public float getChanceOfEnemyShot(){
		//each subarray in levelSettingsB has the chance of enemy shot at index 2
		return levelSettingsB[getCurrentStageIndex(), 2];
	}
	//returns the velocity of an enemy shot for the current level
	static public float getEnemyShotVelocity(){
		//each subarray in levelSettingsB has the enemy shot velocity at index 3
		return levelSettingsB[getCurrentStageIndex(), 3];
	}
	//returns an unordered collection of ship types that the current level should spawn in ShipGenerator
	static public int[] getShipList(){
		int[] shipList;
		switch(getCurrentStageIndex()+1){
			case 1:
				shipList = shipList1;
				return shipList;
				break;
		}
		return new int[]{};
	}
	//returns appropriate index to access the current level's information in levelSettings
	static public int getLevelIndex(){
		return (Application.loadedLevel - 1);
	}
	//returns the current stage number
	static public int getCurrentStageIndex(){
		//because each stage has 4 levels and 1 transition scene, integer division by 5 will return the stage number index
		return (Application.loadedLevel / 5);
	}
	
	
	//called by the previous 3 methods whenever the conditions for winning/losing a level are detected; Update() handles what happens after the pause is over
	private void pauseGameBeforeLoadingNewLevel(float pauseDuration){
		isPaused = true;
		pauseTimeLeft = pauseDuration;
	}
	
	private void DetermineHighestStageReached(){
		int thisStage = getCurrentStageIndex() + 1;
		if(thisStage>highestStageReached){
			highestStageReached = thisStage;
		}
	}

}
