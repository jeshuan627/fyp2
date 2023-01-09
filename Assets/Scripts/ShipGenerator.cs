using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class ShipGenerator : MonoBehaviour 
{
	
	//defined in the inspector as the respective ship prefabs
	public GameObject oneHit;
	//public GameObject twoHit;
	//public GameObject threeHit;

    //these are unique to each level and are definied in Start()
	private int numberOfShips = 0;
	private int numberofTurns = 0;

    void Start()
    {
        StartCoroutine(Order());
    }

    IEnumerator Order()
    {
        yield return new WaitForSeconds(31f);
        while (numberofTurns < 3)
		{
            while (numberOfShips < 61)
            {
                GameObject enemy1Ship = Instantiate(oneHit, new Vector3(0, 7, 0), Quaternion.identity) as GameObject;
                numberOfShips++;
                yield return new WaitForSeconds(0.5f);
            }
			numberOfShips = 0; 
			numberofTurns++;
            if (numberofTurns == 3)
            {
                yield return new WaitForSeconds(5f);
                End();
            }
            else
            {
                yield return new WaitForSeconds(20.0f);
            }

        }

    }

    public void End()
    {
        PlayfabManager updatescore1 = new PlayfabManager();
        updatescore1.SendLeaderboard((int)UIManager.Instance.score);
        ChangeScene ending = new ChangeScene();
        ending.LeaderboardClick();
    }
}
