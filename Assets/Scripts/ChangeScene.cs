using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

	public void StraightLevelClick()
    {
        SceneManager.LoadScene("straightPathsLevel");
    }

    public void LeaderboardClick()
    {
        SceneManager.LoadScene("leaderboard");
    }
}
