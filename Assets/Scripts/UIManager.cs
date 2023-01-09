using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    //singleton implementation
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                instance = new UIManager();
            
            return instance;
        }
    }

    protected UIManager()
    {
    }

    public float time = 0;

    public float score = 0;


    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }
    
    public void SetScore(float value)
    {
        score = value;
        UpdateScoreText();
    }

    public void IncreaseScore(float value)
    {
        score += value;
        UpdateScoreText();
    }

    public void DecreaseScore(float value)
    {
        score -= value;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        ScoreText.text = score.ToString();
    }

    public void ResetTime()
    {
        time = 0;
        UpdateTimeText();
    }

    public void SetTime(float value)
    {
        time = value;
        UpdateTimeText();
    }

    public void IncreaseTime(float value)
    {
        time += value;
        UpdateTimeText();
    }

    public void UpdateTimeText()
    {
        TimeText.text = time.ToString();
    }

    public void SetStatus(string text)
    {
        StatusText.text = text;
    }

    public void SetSubstatus(string text)
    {
        SubstatusText.text = text;
    }

    public Text ScoreText, TimeText, StatusText, SubstatusText;


}
